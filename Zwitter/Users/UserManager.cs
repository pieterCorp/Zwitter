using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Zwitter
{
    internal class UserManager
    {
        public string FolderPath { get; set; } = "../../../Db/";
        public string FilePathUsers { get; set; } = "../../../Db/Users.txt";

        private List<string> EmailRegisteredUsers = new List<string>();
        private List<int> IdRegisteredUsers = new List<int>();
        private UserIO userIO;

        public UserManager()
        {
            userIO = new UserIO();
        }

        public User Login()
        {
            userIO.CenterText(userIO.zwitterAscii);
            userIO.PadLeft("Login\n", 2, ConsoleColor.Cyan);
            UpdateSearchLists();

            userIO.PadLeft("Enter Email", 2);
            string eMail = userIO.GetUserString();
            User user = GetUserByEmail(eMail);

            if (!CheckEmailInSystem(eMail))
            {
                userIO.PadLeft("Email not found. Press enter to continue.", 2, ConsoleColor.DarkYellow);
                Console.ReadLine();
                return user;
            }

            userIO.PadLeft("Enter Password", 2);
            string password = GetPassword();

            //check if email and pass are correct
            if (password != user.Password)
            {
                userIO.PadLeft("Password incorrect, press enter to continue", 2, ConsoleColor.Red);
                Console.ReadLine();
                return user;
            }

            user.LoggedIn = true;
            LoadUserPosts(user);
            return user;
        }

        public void Logout(User user)
        {
            user.LoggedIn = false;
        }

        public void Register()
        {
            userIO.CenterText(userIO.zwitterAscii);
            userIO.PadLeft("Register\n", 2, ConsoleColor.Cyan);

            UpdateSearchLists();

            Console.WriteLine("  Enter Email");
            string eMail = userIO.GetUserString();

            if (CheckEmailInSystem(eMail))
            {
                Console.WriteLine("email already exists, press enter to continue");
                Console.ReadLine();
                return;
            }
            else
            {
                User newUser = MakeNewAccount(eMail);

                StoreUser(newUser);
                UpdateSearchLists();

                userIO.PadLeft("Your account has been created. Press enter to continue", 2, ConsoleColor.Green);
                Console.ReadLine();
            }
        }

        private bool CheckEmailInSystem(string eMail)
        {
            if (EmailRegisteredUsers.Contains(eMail))
            {
                return true;
            }

            return false;
        }

        private User MakeNewAccount(string eMail)
        {
            userIO.PadLeft("Enter firstname", 2);
            string firstName = userIO.GetUserString();

            userIO.PadLeft("Enter lastname", 2);
            string lastName = userIO.GetUserString();

            userIO.PadLeft("Enter password", 2);
            string password = GetPassword();

            userIO.PadLeft("Confirm password", 2);
            string confirmPassword = GetPassword();

            if (password != confirmPassword)
            {
                userIO.PadLeft("Passwords did not match. Press enter and try again", 2, ConsoleColor.Red);
                Console.ReadLine();
                bool validPassword = false;

                while (!validPassword)
                {
                    userIO.PadLeft("Enter password", 2);
                    password = GetPassword();

                    userIO.PadLeft("Confirm password", 2);
                    confirmPassword = GetPassword();

                    if (password == confirmPassword)
                    {
                        validPassword = true;
                    }
                }
            }

            // make unique id;
            Filemanager fileManager = new Filemanager();
            int newId = fileManager.CountLinesFile(FilePathUsers) + 1;

            User newUser = new User();

            newUser.FirstName = firstName;
            newUser.LastName = lastName;
            newUser.Email = eMail;
            newUser.Password = password;
            newUser.dateRegistered = DateTime.Now;
            newUser.Id = newId;

            return newUser;
        }

        private string GetPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }

        private void StoreUser(User user)
        {
            Filemanager fileManager = new Filemanager();
            string json = System.Text.Json.JsonSerializer.Serialize(user);

            fileManager.CreateFolder(FolderPath);
            fileManager.CreateFile(FilePathUsers);
            fileManager.WriteDataToFile(json, FilePathUsers);
        }

        private List<User> LoadUsers()
        {
            Filemanager fileManager = new Filemanager();
            List<string> usersJson = new List<string>();
            List<User> users = new List<User>();

            fileManager.CreateFolder(FolderPath);
            fileManager.CreateFile(FilePathUsers);
            usersJson = fileManager.LoadAllFiles(FilePathUsers);

            for (int i = 0; i < usersJson.Count; i++)
            {
                string json = usersJson[i];
                User result = JsonConvert.DeserializeObject<User>(json);
                users.Add(result);
            }
            return users;
        }

        private void UpdateSearchLists()
        {
            List<User> users = LoadUsers();
            EmailRegisteredUsers.Clear();
            IdRegisteredUsers.Clear();

            foreach (var user in users)
            {
                EmailRegisteredUsers.Add(user.Email);
                IdRegisteredUsers.Add(user.Id);
            }
        }

        private User GetUserByEmail(string eMail)
        {
            int index = EmailRegisteredUsers.IndexOf(eMail);

            if (index < 0)
            {
                User defaultUser = new User();
                return defaultUser;
            }
            else
            {
                List<User> users = LoadUsers();
                return users[index];
            }
        }

        public User GetUserByid(int id)
        {
            UpdateSearchLists();
            int index = IdRegisteredUsers.IndexOf(id);
            List<User> users = LoadUsers();
            return users[index];
        }

        public void LoadUserPosts(User user)
        {
            PostManager postManager = new PostManager();
            List<Post> allPosts = postManager.LoadPosts();
            user.allPosts.Clear();

            foreach (var post in allPosts)
            {
                if (post.PostFromUserId == user.Id)
                {
                    user.allPosts.Add(post);
                }
            }
        }
    }
}