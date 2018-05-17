﻿//******************************************************************************************************
//  LogPublisherInternal.cs - Gbtc
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
    /// This publisher is shared by all other instances of the same <see cref="Type"/>.
    /// </summary>
    internal class LogPublisherInternal
    {
        #region [ Members ]

        /// <summary>
        /// Occurs when a new <see cref="LogMessage"/> is ready to be published.
        /// </summary>
        private LoggerInternal m_logger;

        public readonly PublisherTypeDefinition TypeData;

        public readonly Type Type;

        public readonly Assembly Assembly;

        internal MessageAttributeFilter SubscriptionFilterCollection;

        #endregion

        #region [ Constructors ]

        public LogPublisherInternal(LoggerInternal logger, Type type)
        {
            if ((object)type == null)
                throw new ArgumentNullException(nameof(type));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            Type = type;
            Assembly = type.Assembly;
            TypeData = new PublisherTypeDefinition(type);
            m_logger = logger;
            SubscriptionFilterCollection = new MessageAttributeFilter();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Checks messages generated by this publisher and the provided attributes will be received by a subscriber.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public bool HasSubscribers(LogMessageAttributes attributes)
        {
            return !m_logger.RoutingTablesValid || SubscriptionFilterCollection.IsSubscribedTo(attributes);
        }

        #endregion
     
    }
}
