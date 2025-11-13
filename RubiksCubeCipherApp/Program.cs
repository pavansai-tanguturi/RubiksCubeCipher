using System;
using System.Collections.Generic;

namespace RubiksCubeCipherApp
{
    class Program
    {
        static void Main()
        {
            var cipher = new RubiksCubeCipher();

            Console.Write("Enter message: ");
            string plaintext = Console.ReadLine() ?? "";

            var allMoves = new[] { "U", "R", "L", "F", "D", "B" };
            var rng = new Random();
            var key = Enumerable.Range(0, 6)
                    .Select(_ => allMoves[rng.Next(allMoves.Length)] + (rng.Next(3) == 0 ? "'" : rng.Next(3) == 1 ? "2" : ""))
                    .ToList();

            string encrypted = cipher.Encrypt(plaintext, key);
            RubiksCubeCipher.SaveCiphertextToFile("cipher.txt", encrypted);

            string loaded = RubiksCubeCipher.LoadCiphertextFromFile("cipher.txt");
            
            // Create a new cipher instance for decryption
            var decryptCipher = new RubiksCubeCipher();
            string decrypted = decryptCipher.Decrypt(loaded);

            Console.WriteLine("Key: " + string.Join(" ", key) + "\n");
            Console.WriteLine("Encrypted: " + encrypted + "\n");
            Console.WriteLine("Decrypted: " + decrypted + "\n");
        }
    }
}