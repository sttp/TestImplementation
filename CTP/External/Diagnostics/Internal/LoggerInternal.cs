//******************************************************************************************************
//  LoggerInternal.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/24/2016 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GSF.Collections;
using GSF.Threading;

namespace GSF.Diagnostics
{
    /// <summary>
    /// The fundamental functionality of <see cref="Logger"/>.
    /// </summary>
    internal class LoggerInternal
        : IDisposable
    {
        private bool m_disposing;
        private readonly object m_syncRoot;
        private readonly Dictionary<Type, LogPublisherInternal> m_typeIndexCache;
        private readonly List<LogPublisherInternal> m_allPublishers;
        private readonly List<NullableWeakReference> m_subscribers;
        private readonly ConcurrentQueue<LogMessage> m_messages;
        private readonly ScheduledTask m_routingTask;
        private readonly ScheduledTask m_calculateRoutingTable;
        private MessageAttributeFilter m_filterCollection;

        public bool RoutingTablesValid { get; private set; }

        /// <summary>
        /// Creates a <see cref="LoggerInternal"/>.
        /// </summary>
        public LoggerInternal(out LoggerInternal loggerClass)
        {
            //Needed to set the member variable of Logger. 
            //This is because ScheduleTask will call Logger before Logger's static constructor is completed.
            loggerClass = this;

            m_syncRoot = new object();
            m_typeIndexCache = new Dictionary<Type, LogPublisherInternal>();
            m_allPublishers = new List<LogPublisherInternal>();
            m_subscribers = new List<NullableWeakReference>();
            m_messages = new ConcurrentQueue<LogMessage>();

            // Since ScheduledTask calls ShutdownHandler, which calls Logger. This initialization method cannot occur
            // until after the Logger static class has finished initializing.
            m_calculateRoutingTable = new ScheduledTask();
            m_calculateRoutingTable.Running += CalculateRoutingTable;
            m_calculateRoutingTable.IgnoreShutdownEvent();
            m_routingTask = new ScheduledTask(ThreadingMode.DedicatedForeground);
            m_routingTask.Running += RoutingTask;
            m_routingTask.IgnoreShutdownEvent();
        }

        /// <summary>
        /// Recalculates the entire routing table on a separate thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateRoutingTable(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            lock (m_syncRoot)
            {
                //Some other thread won on the race condition.
                if (RoutingTablesValid)
                    return;

                var subscribers = new List<LogSubscriberInternal>(m_subscribers.Count);
                m_subscribers.RemoveWhere(x =>
                    {
                        LogSubscriberInternal subscriber = (LogSubscriberInternal)x.Target;
                        if (subscriber != null)
                        {
                            subscribers.Add(subscriber);
                            return false;
                        }
                        return true;
                    });


                MessageAttributeFilter filterCollection = new MessageAttributeFilter();
                foreach (var sub in subscribers)
                {
                    filterCollection.Append(sub.GetSubscription());
                }
                foreach (var pub in m_allPublishers)
                {
                    pub.SubscriptionFilterCollection = filterCollection;
                }
                m_filterCollection = filterCollection;
              
                RoutingTablesValid = true;
            }
        }

        private void RoutingTask(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            lock (m_syncRoot)
            {
                if (!RoutingTablesValid)
                {
                    CalculateRoutingTable(null, null);
                }

                LogMessage message;
                while (m_messages.TryDequeue(out message))
                {
                    var filter = m_filterCollection;

                    foreach (var route in m_subscribers)
                    {
                        var subscriber = route.Target as LogSubscriberInternal;

                        if (subscriber == null)
                            RecalculateRoutingTable();
                        else if (filter.IsSubscribedTo(message.LogMessageAttributes))
                            subscriber.RaiseLogMessages(message);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="LogSubscriber"/> that can subscribe to log messages.
        /// </summary>
        /// <returns></returns>
        public LogSubscriberInternal CreateSubscriber()
        {
            lock (m_syncRoot)
            {
                if (m_disposing)
                    return LogSubscriberInternal.DisposedSubscriber;
                var s = new LogSubscriberInternal(RecalculateRoutingTable);
                m_subscribers.Add(s.Reference);
                return s;
            }
        }

        /// <summary>
        /// Invalidates the current routing table.
        /// </summary>
        private void RecalculateRoutingTable()
        {
            RoutingTablesValid = false;
            //Wait some time before recalculating the routing tables. This will be done automatically 
            //if a message is routed before the table is recalculated.
            m_calculateRoutingTable.Start(10);
        }

        /// <summary>
        /// Handles the routing of messages through the logging system.
        /// </summary>
        /// <param name="message">the message to route</param>
        public void OnNewMessage(LogMessage message)
        {
            m_messages.Enqueue(message);
            m_routingTask.Start(50); //Allow a 50ms delay for multiple logs to queue if in a burst period.
        }

        /// <summary>
        /// Creates a type topic on a specified type.
        /// </summary>
        /// <param name="type">the type to create the topic from</param>
        /// <returns></returns>
        public LogPublisherInternal CreateType(Type type)
        {
            if ((object)type == null)
                throw new ArgumentNullException(nameof(type));

            LogPublisherInternal publisher;
            lock (m_syncRoot)
            {
                if (!m_typeIndexCache.TryGetValue(type, out publisher))
                {
                    publisher = new LogPublisherInternal(this, type);
                    m_typeIndexCache.Add(type, publisher);
                    if (!m_disposing)
                    {
                        m_allPublishers.Add(publisher);
                        publisher.SubscriptionFilterCollection = m_filterCollection;
                    }
                }
            }
            return publisher;
        }

        /// <summary>
        /// Gracefully terminate all message routing. Function blocks until all termination is successful.
        /// </summary>
        public void Dispose()
        {
            if (m_disposing)
                return;

            lock (m_syncRoot)
            {
                //Ensure that setting disposing is in a synchronized context.
                m_disposing = true;
            }

            //These scheduled tasks block when dispose is called. Therefore do not put these in a lock on the base class.
            m_calculateRoutingTable.Dispose();
            m_routingTask.Dispose();

            lock (m_syncRoot)
            {
                m_allPublishers.Clear();
                foreach (var sub in m_subscribers)
                {
                    (sub.Target as LogSubscriberInternal)?.Dispose();
                }
                m_subscribers.Clear();
            }
        }


    }
}
