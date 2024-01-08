using System.Text.Json;

namespace BisleriumCafe.Data
{
    public class UserService
    {
        public const string seedUsername = "admin";
        public const string seedPassword = "admin";

        private static void SaveUserInfo(List<User> users)
        {
            string appDataPath = Utils.GetAppDirectory();
            string userFilePath = Utils.GetUsersPath();

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(userFilePath, json);
        }

        public static List<User> GetUserInfo()
        {
            string userFilePath = Utils.GetUsersPath();

            if (!File.Exists(userFilePath))
            {
                return new List<User>();
            }
            var json = File.ReadAllText(userFilePath);
            var users = JsonSerializer.Deserialize<List<User>>(json);
            return users;
        }

        public static List<User> CreateUser(Guid userId, string username, string password, Role role)
        {
            List<User> users = GetUserInfo();
            bool usernameExist = users.Any(x => x.Username == username);

            if (usernameExist)
            {
                throw new Exception("This user name is used!");
            }
            users.Add(
                new User
                {
                    Username = username,
                    PasswordHash = Utils.HashSecretKey(password),
                    Role = role
                }
                );
            SaveUserInfo(users);
            return users;
        }

        public static void SeedUsers()
        {
            var users = GetUserInfo().FirstOrDefault(x => x.Role == Role.Admin);

            if (users == null)
            {
                CreateUser(Guid.Empty, seedUsername, seedPassword, Role.Admin);
            }
        }

        public static List<User> DeleteUser(Guid ID)
        {
            List<User> users = GetUserInfo();
            User user = users.FirstOrDefault(x => x.UserID == ID);
            if (user == null)
            {
                throw new Exception("Null Users!");
            }
            users.Remove(user);
            SaveUserInfo(users);
            return users;

        }

        public static User Login(string username, string password)
        {
            List<User> users = GetUserInfo();
            User user = users.FirstOrDefault(x => x.Username == username);
            var errorMessage = "Invalid Credentials!";
            if (user == null)
            {
                throw new Exception(errorMessage);
            }
            bool pwMatched = Utils.HashVerification(password, user.PasswordHash);
            if (!pwMatched)
            {
                throw new Exception(errorMessage);
            }
            return user;

        }
    }
}