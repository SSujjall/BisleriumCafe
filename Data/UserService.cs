using System.Text.Json;

namespace BisleriumCafe.Data
{
    public class UserService
    {
        public const string seedUsername = "admin";
        public const string seedPassword = "admin";

        private static void SaveUser(List<User> users)
        {
            string appDataDirectory = Utils.GetAppDirectoryPath();
            string userFilePath = Utils.GetUsersFilePath();

            if (!Directory.Exists(appDataDirectory))
            {
                Directory.CreateDirectory(appDataDirectory);
            }
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(userFilePath, json);
        }

        public static List<User> GetUser()
        {
            string userFilePath = Utils.GetUsersFilePath();

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
            List<User> users = GetUser();
            bool usernameExists = users.Any(x => x.Username == username);

            if (usernameExists)
            {
                throw new Exception("This username already exists!");
            }
            users.Add(
                new User
                {
                    Username = username,
                    PasswordHash = Utils.HashSecret(password),
                    Role = role
                }
                );
            SaveUser(users);
            return users;
        }

        public static void SeedUsers()
        {
            var users = GetUser().FirstOrDefault(x => x.Role == Role.Admin);

            if (users == null)
            {
                CreateUser(Guid.Empty, seedUsername, seedPassword, Role.Admin);
            }
        }

        public static List<User> DeleteUser(Guid ID)
        {
            List<User> users = GetUser();
            User user = users.FirstOrDefault(x => x.Id == ID);
            if (user == null)
            {
                throw new Exception("No users to delete!");
            }
            users.Remove(user);
            SaveUser(users);
            return users;

        }

        public static User LogIn(string username, string password)
        {
            List<User> users = GetUser();
            User user = users.FirstOrDefault(x => x.Username == username);
            var errorDialog = "Invalid username or password";
            if (user == null)
            {
                throw new Exception(errorDialog);
            }
            bool pwMatched = Utils.VerifyHash(password, user.PasswordHash);
            if (!pwMatched)
            {
                throw new Exception(errorDialog);
            }
            return user;

        }
    }
}
