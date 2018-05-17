//******************************************************************************************************
//  MemberInfoExtensions.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
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
//  01/30/2009 - J. Ritchie Carroll
//       Generated original version of source code.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System.Reflection;

namespace GSF.Reflection
{
    /// <summary>
    /// Defines extensions methods related to <see cref="MemberInfo"/> objects and derived types (e.g., <see cref="FieldInfo"/>,
    /// <see cref="PropertyInfo"/>, <see cref="MethodInfo"/>, etc.).
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets the friendly class name of the provided <see cref="MemberInfo"/> object, trimming generic parameters.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> object over which to get friendly class name.</param>
        /// <returns>Friendly class name of the provided member, or <see cref="string.Empty"/> if <paramref name="member"/> is <c>null</c>.</returns>
        public static string GetFriendlyClassName<TMemberInfo>(this TMemberInfo member) where TMemberInfo : MemberInfo
        {
            // Compiler may get confused about which extension function to use, so we specify explicitly to avoid potential recursive call
            return (object)member != null ? TypeExtensions.GetFriendlyClassName(member.DeclaringType) : string.Empty;
        }

       
    }
}
