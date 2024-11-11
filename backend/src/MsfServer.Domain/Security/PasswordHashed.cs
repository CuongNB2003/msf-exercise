using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace MsfServer.Domain.Security
{
    public class PasswordHashed
    {
        public static string HashPassword(string password, int iterations = 4, int memorySize = 1024 * 1024, int degreeOfParallelism = 8)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
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

        public static bool VerifyPassword(string password, string hashedPassword, int iterations = 4, int memorySize = 1024 * 1024, int degreeOfParallelism = 8)
        {
            string hashToVerify = HashPassword(password, iterations, memorySize, degreeOfParallelism);
            return hashToVerify == hashedPassword;
        }
    }
}
