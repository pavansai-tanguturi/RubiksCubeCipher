using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RubiksCubeCipherApp
{
    public class RubiksCubeCipher
    {
        private char[] cube = new char[54];
        private static readonly Dictionary<string, int[]> Moves;
        private static readonly Dictionary<string, int[]> Inverses;

        static RubiksCubeCipher()
        {
            Moves = new();
            Inverses = new();

            // Create reproducible random but consistent permutations
            var moveNames = new[] { "U", "R", "L", "F", "D", "B" };
            var rng = new Random(42); // deterministic seed
            foreach (var name in moveNames)
            {
                int[] map = Enumerable.Range(0, 54).ToArray();
                map = map.OrderBy(_ => rng.Next()).ToArray();
                Moves[name] = map;

                // precompute inverse
                int[] inv = new int[54];
                for (int i = 0; i < 54; i++)
                    inv[map[i]] = i;
                Inverses[name] = inv;
            }
        }

        private void ResetCube() => Array.Fill(cube, '_');

        private void EncodeMessage(string message)
        {
            var chars = message.PadRight(54, '_').Take(54).ToArray();
            for (int i = 0; i < 54; i++) cube[i] = chars[i];
        }

        private string DecodeMessage() => new string(cube);

        private void ApplyPermutation(int[] map)
        {
            char[] next = new char[54];
            for (int i = 0; i < 54; i++) next[i] = cube[map[i]];
            cube = next;
        }

        private void ApplyMove(string move)
        {
            // Parse move: handle X, X', X2, X'2
            bool isPrime = move.Contains("'");
            bool isDouble = move.Contains("2");
            string baseMove = move.Replace("'", "").Replace("2", "");
            
            if (!Moves.ContainsKey(baseMove)) return;

            int[] mapping = Moves[baseMove];
            int[] inverse = Inverses[baseMove];
            
            // Choose which permutation to use
            int[] toApply = isPrime ? inverse : mapping;
            int times = isDouble ? 2 : 1;
            
            for (int i = 0; i < times; i++)
                ApplyPermutation(toApply);
        }

        private static string InverseMove(string move)
        {
            if (move.EndsWith("'2")) return move.Replace("'", "");  // X'2 -> X2
            if (move.EndsWith("2")) return move[..^1] + "'2";  // X2 -> X'2
            if (move.EndsWith("'")) return move[..^1];  // X' -> X
            return move + "'";  // X -> X'
        }

        public string Encrypt(string message, List<string> key)
        {
            EncodeMessage(message);
            foreach (var move in key) ApplyMove(move);
            string encodedText = DecodeMessage();

            // Embed metadata: message length + moves
            string metadata = $"{message.Length}:{string.Join(",", key)}";
            string metaEncoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(metadata));

            // Return ciphertext with embedded metadata
            return encodedText + "|" + metaEncoded;
        }

        public string Decrypt(string cipherWithMeta)
        {
            // Split ciphertext and metadata
            var parts = cipherWithMeta.Split('|');
            if (parts.Length != 2) throw new Exception("Invalid ciphertext format.");

            string cipher = parts[0];
            string metaEncoded = parts[1];

            // Decode metadata
            string metadata = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(metaEncoded));
            var metaParts = metadata.Split(':');
            int messageLength = int.Parse(metaParts[0]);
            var moves = metaParts[1].Split(',').ToList();

            // Decrypt cube - ensure cipher is exactly 54 chars
            if (cipher.Length != 54)
                cipher = cipher.PadRight(54, '_');
            
            cube = cipher.ToCharArray();
            
            // Apply inverse moves in reverse order
            for (int i = moves.Count - 1; i >= 0; i--)
            {
                string inverseMove = InverseMove(moves[i]);
                ApplyMove(inverseMove);
            }

            string decoded = DecodeMessage();
            return decoded.Substring(0, Math.Min(messageLength, decoded.Length));
        }

        public static void SaveCiphertextToFile(string path, string text)
            => File.WriteAllText(path, text);

        public static string LoadCiphertextFromFile(string path)
            => File.ReadAllText(path);
    }
}