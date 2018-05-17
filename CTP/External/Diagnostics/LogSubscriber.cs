﻿//******************************************************************************************************
//  LogSubscriber.cs - Gbtc
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
using System.Reflection;

namespace GSF.Diagnostics
{
    /// <summary>
    /// Subscribes to log events.
    /// </summary>
    public sealed class LogSubscriber : IDisposable
    {
        private readonly LogSubscriberInternal m_subscriber;

        /// <summary>
        /// Event handler for the logs that are raised.
        /// </summary>
        /// <remarks>
        /// Any exceptions generated by this callback will be ignored.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event NewLogMessageEventHandler NewLogMessage
        {
            add
            {
                m_subscriber.NewLogMessage += value;
            }
            remove
            {
                m_subscriber.NewLogMessage -= value;
            }
        }

        /// <summary>
        /// Creates a <see cref="LogSubscriber"/>
        /// </summary>
        internal LogSubscriber(LogSubscriberInternal subscriber)
        {
            m_subscriber = subscriber;
        }

        /// <summary>
        /// Subscribes to all publishers with the specified verbose level.
        /// </summary>
        public void SubscribeToAll(VerboseLevel level)
        {
            m_subscriber.Subscribe(MessageAttributeFilter.Create(level));
        }

        /// <summary>
        /// Clears all subscriptions
        /// </summary>
        public void Clear()
        {
            m_subscriber.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            m_subscriber.Dispose();
        }
    }
}