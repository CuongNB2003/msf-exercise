using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace MsfServer.Domain.Security
{
    public class PasswordHashed
    {
        public static string HashPassword(string password, string key, int iterations = 4, int memorySize = 1024 * 1024, int degreeOfParallelism = 8)
        {
            byte[] salt = GenerateBase64Key(key);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };
            byte[] hash = argon2.GetBytes(64);
            return Convert.ToBase64String(hash);
        }

        public static byte[] GenerateBase64Key(string key)
        {
            return Encoding.UTF8.GetBytes(key);
        }
    }
}
