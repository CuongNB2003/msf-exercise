
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace MsfServer.Domain.Security
{
    public class PasswordHasher
    {
        public static string HashPassword(string password, byte[] salt, int iterations = 4, int memorySize = 1024 * 1024, int degreeOfParallelism = 8)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };
            byte[] hash = argon2.GetBytes(16); // Độ dài của hash là 16 byte
            return Convert.ToBase64String(hash);
        }

        public static byte[] GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static bool VerifyPassword(string password, string hashedPassword, byte[] salt, int iterations = 4, int memorySize = 1024 * 1024, int degreeOfParallelism = 8)
        {
            string hashToVerify = HashPassword(password, salt, iterations, memorySize, degreeOfParallelism);
            return hashToVerify == hashedPassword;
        }
    }
}
