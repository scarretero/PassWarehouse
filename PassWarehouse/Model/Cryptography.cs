using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace PassWarehouse.Model
{
    class Cryptography
    {
        // Esta constante se utiliza para determinar el tamaño de la key del algoritmo de encriptación en bits.
        // La dividimos entre 8 más adelante para obtener el numero de bytes.
        private const int Keysize = 256;

        // Esta constante determina el numero de iteraciones para la función que genera los bytes de la variable a encriptar/desencriptar.
        private const int DerivationIterations = 1000;

        public string Encrypt(string plainText, string passPhrase)
        {
            // El Salt y el IV se generan aleatoriamente cada vez, por ello se concatenarán al incio del texto cifrado, para que este pueda ser descifrado posteriormente.
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);

            var keyBytes = password.GetBytes(Keysize / 8);

            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;

                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            // Creamos el array final de bytes concatenando los bytes aleatorios del Salt, los bytes aleatorios del IV y los bytes del texto cifrado.
                            var cipherTextBytes = saltStringBytes;
                            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                            memoryStream.Close();
                            cryptoStream.Close();
                            password = null;

                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
            }
        }

        public string Decrypt(string cipherText, string passPhrase)
        {
            // Cogemos el flujo completo de bytes que representa:
            // [32 bytes del Salt] + [32 bytes del IV] + [n bytes del texto cifrado]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Cogemos los primeros 32 bytes correspondientes al salt
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Cogemos los siguientes 32 bytes correspondientes al IV
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Cogemos el texto cifrado que nos interesa quitándole los 64 primeros bytes correspondientes al Salt y al IV
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);

            var keyBytes = password.GetBytes(Keysize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[cipherTextBytes.Length];
                            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            memoryStream.Close();
                            cryptoStream.Close();
                            password = null;

                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }

        private byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            var rngCsp = new RNGCryptoServiceProvider();

            // Fill the array with cryptographically secure random bytes.
            rngCsp.GetBytes(randomBytes);
            rngCsp = null;

            return randomBytes;
        }
    }
}
