namespace Assets.GameEngine.Providers
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class CryptoProvider
    {
#if !UNITY_METRO
        public byte[] Encrypt(string dataToEncrypt, byte[] password)
        {
            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream();
                Encrypt(Encoding.UTF8.GetBytes(dataToEncrypt), password, memoryStream);

                // Return Base 64 String
                return memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }
        }

        public byte[] CreateHmac(byte[] secret, byte[] message)
        {
            var hmac = new HMACSHA256();
            hmac.Key = secret;
            return hmac.ComputeHash(message);
        }

        /// <summary>
        /// Encrypt string with AES.
        /// </summary>
        /// <param name="dataToEncrypt">Unsafe data</param>
        /// <param name="password">Password used for encription</param>
        /// <param name="stream">Destination stream like memory or file</param>
        public static void Encrypt(byte[] dataToEncrypt, byte[] password, Stream stream)
        {
            AesManaged aes = null;
            CryptoStream cryptoStream = null;

            try
            {
                // Generate a Key based on a Password and HMACSHA1 pseudo-random number generator
                // Salt must be at least 8 bytes long
                // Use an iteration count of at least 1000
                //Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10);

                // Create AES algorithm
                aes = new AesManaged();

                // Key derived from byte array with 32 pseudo-random key bytes
                aes.Key = password;

                // IV derived from byte array with 16 pseudo-random key bytes
                aes.IV = new byte[16];

                // Create Crypto Streams
                cryptoStream = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                // Encrypt Data
                cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cryptoStream.FlushFinalBlock();
            }
            finally
            {
                if (cryptoStream != null)
                    cryptoStream.Close();

                if (aes != null)
                    aes.Clear();
            }
        }
#endif
    }
}
