//******************************************************************************************************
//  SyncWorker.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/21/2014 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.Threading;

namespace GSF.Threading
{
    /// <summary>
    /// A ultra lightweight synchronized operation class.
    /// </summary>
    public class SyncWorker
    {
        #region [ Members ]

        // Constants
        private const int NotRunning = 0;
        private const int Running = 1;
        private const int RunAgain = 2;

        // Fields
        private readonly Action m_action;
        private readonly Action<Exception> m_exceptionAction;
        private int m_state;
        private WaitCallback m_workerCallback;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of the <see cref="SyncWorker"/> class.
        /// </summary>
        /// <param name="action">The action to be performed during this operation.</param>
        public SyncWorker(Action action)
            : this(action, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SyncWorker"/> class.
        /// </summary>
        /// <param name="action">The action to be performed during this operation.</param>
        /// <param name="exceptionAction">The action to be performed if an exception is thrown from the action.</param>
        public SyncWorker(Action action, Action<Exception> exceptionAction)
        {
            if ((object)action == null)
                throw new ArgumentNullException(nameof(action));

            m_action = action;
            m_exceptionAction = exceptionAction;
            m_workerCallback = WorkerCallback;
        }

        #endregion

        #region [ Methods ]

        public void Run()
        {
            while (true)
            {
                switch (Thread.VolatileRead(ref m_state))
                {
                    case NotRunning:
                        //If the state was changed from NotRunning To Running
                        if (Interlocked.CompareExchange(ref m_state, Running, NotRunning) == NotRunning)
                        {
                            ThreadPool.QueueUserWorkItem(m_workerCallback);
                            return;
                        }
                        break;
                    case Running:
                        //If the state was changed from [Running] to [RunAgain]
                        if (Interlocked.CompareExchange(ref m_state, RunAgain, Running) == Running)
                        {
                            return;
                        }
                        break;
                    case RunAgain:
                        //Even in a race condition, if this method is called and it was at RunAgain, it's OK to quit.
                        return;
                }
            }
        }

        private void WorkerCallback(object data)
        {
            try
            {
                m_action();
            }
            catch (Exception ex)
            {
                try
                {
                    m_exceptionAction?.Invoke(ex);
                }
                catch
                {

                }
            }

            while (true)
            {
                switch (Thread.VolatileRead(ref m_state))
                {
                    case NotRunning:
                        //If the state was changed from NotRunning To NotRunning
                        if (Interlocked.CompareExchange(ref m_state, NotRunning, NotRunning) == NotRunning)
                        {
                            Debug.Write("This case is invalid and should never occur");
                            return;
                        }
                        break;
                    case Running:
                        //If the state was changed from [Running] to [NotRunning]
                        if (Interlocked.CompareExchange(ref m_state, NotRunning, Running) == Running)
                        {
                            return;
                        }
                        break;
                    case RunAgain:
                        //If the state was changed from [RunAgain] to [Running]
                        if (Interlocked.CompareExchange(ref m_state, Running, RunAgain) == RunAgain)
                        {
                            ThreadPool.QueueUserWorkItem(m_workerCallback);
                            return;
                        }
                        break;
                }
            }
        }

        #endregion
    }
}
