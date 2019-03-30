//******************************************************************************************************
//  StateMachine.cs - Gbtc
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
using System.Linq.Expressions;
using System.Threading;
using GSF.Reflection;

#pragma warning disable 420

namespace GSF.Threading
{
    /// <summary>
    /// Helps facilitate a multithreaded state machine.
    /// </summary>
    /// <remarks>
    /// State machine variables must be integers since <see cref="Interlocked"/> methods require premitive types.
    /// </remarks>
    public class StateMachine<T>
        where T : struct
    {
        private readonly Func<T, int> m_toInt;
        private readonly Func<int, T> m_fromInt;
        private volatile int m_state;

        /// <summary>
        /// Creates a new <see cref="StateMachine"/>
        /// </summary>
        /// <param name="initialState">the state to initially set to</param>
        public StateMachine(T initialState)
        {
            m_toInt = EnumCasting<T>.ToInt;
            m_fromInt = EnumCasting<T>.FromInt;
            m_state = m_toInt(initialState);
        }

        /// <summary>
        /// Attempts to change the state of this machine from <see param="prevState"/> to <see param="nextState"/>.
        /// </summary>
        /// <param name="prevState">The state to change from</param>
        /// <param name="nextState">The state to change to</param>
        /// <returns>True if the state changed from the previous state to the next state. 
        /// False if unsuccessful.</returns>
        public bool TryChangeState(T prevState, T nextState)
        {
            int prev = m_toInt(prevState);
            return (m_state == prev && Interlocked.CompareExchange(ref m_state, m_toInt(nextState), prev) == prev);
        }

        /// <summary>
        /// Gets the current state of the State Machine. 
        /// </summary>
        public T State
        {
            get
            {
                return m_fromInt(m_state);
            }
        }

        /// <summary>
        /// Sets the value of the state. Note. This is not concurrently thread safe. 
        /// </summary>
        /// <param name="state"></param>
        public void SetState(T state)
        {
            m_state = m_toInt(state);
        }

        /// <summary>
        /// Implicitly conversion of the state to the integer value of the state.
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public static implicit operator T(StateMachine<T> machine)
        {
            return machine.State;
        }
    }
}