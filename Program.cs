using System;
using System.Security.Cryptography;

namespace TestAES_GCM_256
{
    class Program
    {
        static void Main(string[] args)
        {

            //Generate and dump KEY so we could use again
            //Console.WriteLine(AesGcm256.toHex(AesGcm256.NewKey()));

            //Generate and dump IV so we could use again
            //Console.WriteLine(AesGcm256.toHex(AesGcm256.NewIv()));

            //Console.ReadKey();


            //using above code these key and iv was generated
            string hexKey = "2192B39425BBD08B6E8E61C5D1F1BC9F428FC569FBC6F78C0BC48FCCDB0F42AE";
            string hexIV = "E1E592E87225847C11D948684F3B070D";

            string plainText = "Test encryption and decryption";
            Console.WriteLine("Plain Text: " + plainText);

            //encrypt - result base64 encoded string
            string encryptedText = AesGcm256.encrypt(plainText, AesGcm256.HexToByte(hexKey), AesGcm256.HexToByte(hexIV));
            Console.WriteLine("Encrypted base64 encoded: " + encryptedText);

            //decrypt - result plain string
            string decryptedText = AesGcm256.decrypt(encryptedText, AesGcm256.HexToByte(hexKey), AesGcm256.HexToByte(hexIV));
            Console.WriteLine("Decrypted Text: " + decryptedText);

            if (plainText.Equals(decryptedText))
            {
                Console.WriteLine("Test Passed");
            }
            else
            {
                Console.WriteLine("Test Failed");
            }

            DeriveKey();

            /* Console Output
            Plain Text: Test encryption and decryption
            Encrypted base64 encoded: A/boAixWJKflKviHp2cfDl6l/xn1qw2MsHcKFkrOfm2XOVmawIFct4fS1w7wKw==
            Decrypted Text: Test encryption and decryption
            Test Passed
            Press any key to continue . . .
            */
        }
        /// <summary>
        /// Test to demonstrate that key and iv can be derived again using password if salt stays the same
        /// </summary>
        private static void DeriveKey()
        {

            String password = "Test@Password!";
            byte[] saltBytes = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(saltBytes);
            }

            Rfc2898DeriveBytes encryptionKey = new Rfc2898DeriveBytes(password, saltBytes, 10);
            byte[] key1 = encryptionKey.GetBytes(32);
            byte[] iv1 = encryptionKey.GetBytes(16);

            encryptionKey = new Rfc2898DeriveBytes(password, saltBytes, 10);
            byte[] key2 = encryptionKey.GetBytes(32);
            byte[] iv2 = encryptionKey.GetBytes(16);


            string plainText = "Test encryption and decryption";
            Console.WriteLine("Plain Text: " + plainText);

            //encrypt - result base64 encoded string
            string encryptedText = AesGcm256.encrypt(plainText, key1, iv1);
            Console.WriteLine("Encrypted base64 encoded: " + encryptedText);

            //decrypt - result plain string
            string decryptedText = AesGcm256.decrypt(encryptedText, key2, iv2);
            Console.WriteLine("Decrypted Text: " + decryptedText);
        }
    }
}
