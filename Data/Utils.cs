using System.Security.Cryptography;

namespace BisleriumCafe.Data
{
    public static class Utils
    {
        //Just a variable used as a separator 
        private const char _separator = ':';

        //Hashing
        public static string HashSecretKey(string input)
        {
            var saltSize = 16;
            var iterations = 100_000;
            var keySize = 32;
            HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            return string.Join(
                _separator,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
        }

        //Verifying hash password for authentication
        public static bool HashVerification(string input, string hashString)
        {
            string[] segments = hashString.Split(_separator);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new(segments[3]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        //For getting the folder directory where the app's data will be saved
        public static string GetAppDirectory()
        {
            return Path.Combine(
                FileSystem.AppDataDirectory,
                "BisleriumCafe"
            );
        }

        //Getting users.json file
        public static string GetUsersPath()
        {
            return Path.Combine(GetAppDirectory(), "users.json");
        }

        //Getting items.json file
        public static string GetItemsPath()
        {
            return Path.Combine(GetAppDirectory(), "items.json");
        }

        ////Getting orders.json file
        public static string GetOrdersPath()
        {
            return Path.Combine(GetAppDirectory(), "orders.json");
        }
    }
}