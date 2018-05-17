﻿//******************************************************************************************************
//  LogSubscriberInternal.cs - Gbtc
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
using System.Collections.Generic;

// ReSharper disable InconsistentlySynchronizedField

namespace GSF.Diagnostics
{
    /// <summary>
    /// A <see cref="LogSubscriberInternal"/> that collects logs 
    /// </summary>
    internal class LogSubscriberInternal
    {
        private class SubscriptionInfo
        {
            public PublisherFilter PublisherFilter;
            public MessageAttributeFilter AttributeFilter;
            public bool IsIgnoreSubscription;

            public SubscriptionInfo(PublisherFilter publisherFilter, MessageAttributeFilter attributeFilter, bool isIgnoreSubscription)
            {
                PublisherFilter = publisherFilter;
                AttributeFilter = attributeFilter;
                IsIgnoreSubscription = isIgnoreSubscription;
            }
        }

        /// <summary>
        /// Event handler for the logs that are raised.
        /// </summary>
        /// <remarks>
        /// Any exceptions generated by this callback will be ignored.
        /// </remarks>
        public event NewLogMessageEventHandler NewLogMessage;

        private Action m_recalculateRoutingTable;

        private readonly object m_syncRoot;

        private bool m_disposed;

        private List<SubscriptionInfo> m_allSubscriptions;

        /// <summary>
        /// Since weak references are linked to this class, this is a common one that everyone can use when storing this weak reference.
        /// </summary>
        public readonly NullableWeakReference Reference;

        /// <summary>
        /// Creates a <see cref="LogSubscriberInternal"/>
        /// </summary>
        public LogSubscriberInternal(Action recalculateRoutingTable)
        {
            Reference = new NullableWeakReference(this);
            m_recalculateRoutingTable = recalculateRoutingTable;
            m_syncRoot = new object();
            m_allSubscriptions = null;
        }

        /// <summary>
        /// Clears all subscriptions
        /// </summary>
        public void Clear()
        {
            if (m_disposed)
                return;

            lock (m_syncRoot)
            {
                if (m_disposed)
                    return;

                m_allSubscriptions = null;
            }
            m_recalculateRoutingTable();

        }

        /// <summary>
        /// Adds/Modify/Deletes an existing subscription
        /// </summary>
        public void Subscribe(PublisherFilter publisherFilter, MessageAttributeFilter attributeFilter, bool isIgnoreSubscription)
        {
            if (publisherFilter == null)
                throw new ArgumentNullException(nameof(publisherFilter));

            if (m_disposed)
                return;

            lock (m_syncRoot)
            {
                if (m_disposed)
                    return;

                if (m_allSubscriptions == null)
                {
                    if (attributeFilter == null)
                        return;

                    m_allSubscriptions = new List<SubscriptionInfo>();
                    m_allSubscriptions.Add(new SubscriptionInfo(publisherFilter, attributeFilter, isIgnoreSubscription));
                }
                else
                {
                    for (int x = 0; x < m_allSubscriptions.Count; x++)
                    {
                        if (m_allSubscriptions[x].PublisherFilter.ContainsTheSameLogSearchCriteria(publisherFilter))
                        {
                            if (attributeFilter == null)
                            {
                                m_allSubscriptions.RemoveAt(x);
                                return;
                            }
                            m_allSubscriptions[x].AttributeFilter = attributeFilter;
                            m_allSubscriptions[x].IsIgnoreSubscription = isIgnoreSubscription;
                            return;
                        }
                    }
                    if (attributeFilter == null)
                        return;

                    m_allSubscriptions.Add(new SubscriptionInfo(publisherFilter, attributeFilter, isIgnoreSubscription));
                }
            }
            m_recalculateRoutingTable();
        }

        /// <summary>
        /// Assigns the supplied message to this subscriber.
        /// </summary>
        /// <param name="log">the message</param>
        public void RaiseLogMessages(LogMessage log)
        {
            if (m_disposed)
                return;

            lock (m_syncRoot)
            {
                if (m_disposed)
                    return;

                if (m_allSubscriptions == null || m_allSubscriptions.Count == 0)
                    return;

                OnLog(log);
            }
        }

        public MessageAttributeFilter GetSubscription(LogPublisherInternal publisher)
        {
            lock (m_syncRoot)
            {
                if (m_allSubscriptions == null)
                    m_allSubscriptions = new List<SubscriptionInfo>();

                MessageAttributeFilter filter = new MessageAttributeFilter();
                foreach (var subscription in m_allSubscriptions)
                {
                    if (subscription.PublisherFilter.ContainsPublisher(publisher))
                    {
                        if (subscription.IsIgnoreSubscription)
                        {
                            filter.Remove(subscription.AttributeFilter);
                        }
                        else
                        {
                            filter.Append(subscription.AttributeFilter);
                        }
                    }
                }
                return filter;
            }
        }

        /// <summary>
        /// Raises the <see cref="NewLogMessage"/> event.
        /// </summary>
        /// <param name="logMessage">the message to raise.</param>
        private void OnLog(LogMessage logMessage)
        {
            if (logMessage == null)
                return;
            try
            {
                using (Logger.SuppressFirstChanceExceptionLogMessages())
                {
                    NewLogMessage?.Invoke(logMessage);
                }
            }
            catch (Exception)
            {
                //Swallow this exception
                //This is because if a subscriber throws an exception, creating a new log
                //might cause an infinite loop.
            }
        }

        /// <summary>
        /// Disposes this class so future messages will not route. 
        /// </summary>
        public void Dispose()
        {
            m_disposed = true;
            m_allSubscriptions = null;
            Reference.Clear();
        }

        public static readonly LogSubscriberInternal DisposedSubscriber;

        static LogSubscriberInternal()
        {
            DisposedSubscriber = new LogSubscriberInternal(null);
            DisposedSubscriber.Dispose();
        }


    }
}