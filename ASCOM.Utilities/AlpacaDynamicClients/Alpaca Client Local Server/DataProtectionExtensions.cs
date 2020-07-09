using System;
using System.Text;
using System.Security.Cryptography;
using ASCOM.Utilities;

namespace ASCOM.DynamicRemoteClients
{
    /// <summary>
    /// Extensions to the String data type to allow the string to be encrypted and decrypted.
    /// 
    /// </summary>
    public static class DataProtectionExtensions
    {
        // Set the scope of encryption - either to just the current user or to any user on the local machine
        // Local machine is used because the ASCOM registry is machine wide and is not tied to a specific user logon.
        // This is the weaker of the two options, but the alternative is to have the call fail if configuration is made from one user account
        // while use of the driver is undertaken from another account.
        // Encryption uses the Microsoft Data Protection API (DPAPI) for simplicity and avoidance of the need to manage keys. 
        const DataProtectionScope dataProtectionScope = DataProtectionScope.LocalMachine;

        public static string Encrypt(this string clearText, TraceLogger TL)
        {
            try
            {
                byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
                byte[] entropyBytes = Encoding.UTF8.GetBytes(GenerateEntropy());
                byte[] encryptedBytes = ProtectedData.Protect(clearBytes, entropyBytes, dataProtectionScope);
                string encryptedText = Convert.ToBase64String(encryptedBytes);
                TL.LogMessage("Encrypt", encryptedText);
                return encryptedText;
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Encrypt", ex.ToString());
                return "Unable to encrypt this value";
            }
        }

        public static string Unencrypt(this string encryptedText, TraceLogger TL)
        {
            try
            {
                if (encryptedText == null) return ""; // Deal with initial case where text has not yet been encrypted
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] entropyBytes = Encoding.UTF8.GetBytes(GenerateEntropy());
                byte[] clearBytes = ProtectedData.Unprotect(encryptedBytes, entropyBytes, dataProtectionScope);
                string clearText = Encoding.UTF8.GetString(clearBytes);
                TL.LogMessage("Unencrypt", encryptedText);
                return clearText;
            }
            catch (Exception ex)
            {
                TL.LogMessageCrLf("Unencrypt", ex.ToString());
                return "Unable to decrypt this value";
            }
        }


        public static string GenerateEntropy()
        {
            //const string optionalEntropy = "ASCOM!*&";

            string optionalEntropy = Environment.MachineName;
            return optionalEntropy;
        }
    }
}