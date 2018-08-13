using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace CTP.SRP
{
    public class SrpCredential
    {
        /// <summary>
        /// Gets the ID associated with this name.
        /// </summary>
        public uint RuntimeCredentialNameID;

        /// <summary>
        /// The name of the credential record used to authenticate a client.
        /// </summary>
        public readonly string CredentialName;
        /// <summary>
        /// The verifier that can verify a user's secret. This should be kept confidential 
        /// otherwise, an attacker can attempt a brute force attack on the password, 
        /// plus anyone who possesses this verifier can impersonate the server.
        /// </summary>
        public readonly byte[] Verifier;

        /// <summary>
        /// A salt value to make verifiers distinct with the same password. For sessions that are paired, 
        /// the salt is unnecessary since the password itself is random.
        /// </summary>
        public readonly byte[] Salt;

        /// <summary>
        /// The bit strength of the SRP authentication. The security strength should be considered equivalent strength of RSA.
        /// </summary>
        public readonly SrpStrength SrpStrength;

        /// <summary>
        /// The name information to associate with the login.
        /// </summary>
        public readonly string LoginName;

        /// <summary>
        /// The roles granted to this credential.
        /// </summary>
        public readonly string[] Roles;

        /// <summary>
        /// Gets if the user is allowed to change their password.
        /// </summary>
        public bool CannotChangePassword { get; private set; }

        /// <summary>
        /// Gets if the user can rotate a key.
        /// </summary>
        public bool CannotChangeKey { get; private set; }

        /// <summary>
        /// Indicates that the user must change their credentials on the next login attempt. 
        /// </summary>
        public bool MustChangeCredentials { get; private set; }

        /// <summary>
        /// Gets if the server will require that the password be provided so it can validate that it meets
        /// a specific password policy.
        /// </summary>
        public bool MustValidatesPasswordComplexity { get; private set; }

        /// <summary>
        /// Indicates if the stored credentials for this user are a specified password or a randomly generated key.
        /// </summary>
        public bool IsPassword { get; private set; }

        /// <summary>
        /// Gets the date that the password was last modified.
        /// </summary>
        public DateTime PasswordLastChangedDate { get; private set; }

        /// <summary>
        /// Gets if this account is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets if this account is locked out.
        /// </summary>
        public bool IsLockedOut { get; private set; }

        /// <summary>
        /// Gets the number of failed attempts before an account is locked out.
        /// </summary>
        public int MaxInvalidAttempts { get; private set; }

        /// <summary>
        /// Gets the time before resetting the failed attempts.
        /// </summary>
        public int LockoutTimerSeconds { get; private set; }

        /// <summary>
        /// Gets the time delay before an account is automatically unlocked.
        /// </summary>
        public int ResetDurationSeconds { get; private set; }


        public SrpCredential Clone(uint id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="salt"></param>
        /// <param name="verifierCode"></param>
        /// <param name="srpStrength"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        public SrpCredential(string credentialName, byte[] verifierCode, byte[] salt, SrpStrength srpStrength, string loginName, string[] roles)
        {
            CredentialName = credentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            Verifier = (byte[])verifierCode.Clone();
            Salt = (byte[])salt.Clone();
            SrpStrength = srpStrength;
            LoginName = loginName;
            Roles = roles;
        }

        /// <summary>
        /// Creates a credential from the provided data.
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="secret"></param>
        /// <param name="strength"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public SrpCredential(string credentialName, string secret, SrpStrength strength, string loginName, string[] roles)
        {
            credentialName = credentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            CredentialName = credentialName;
            Salt = Security.CreateSalt(64);
            byte[] x = SrpMethods.ComputeX(Salt, credentialName, secret);
            Verifier = SrpMethods.ComputeV(strength, x);
            LoginName = loginName;
            Roles = roles;
        }


    }
}