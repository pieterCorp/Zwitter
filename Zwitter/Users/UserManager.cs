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
            userIO.PrintColor(ConsoleColor.Cyan, "  Login\n");

            UpdateSearchLists();

            Console.WriteLine("  Enter Email");
            string eMail = userIO.GetUserString();

            User user = GetUserByEmail(eMail);

            if (!CheckEmailInSystem(eMail))
            {
                Console.WriteLine("Email not found. Press enter to continue.");
                Console.ReadLine();
                return user;
            }

            Console.WriteLine("  Enter Password");
            string password = GetPassword();

            //check if email and pass are correct
            if (password != user.Password)
            {
                userIO.PrintColor(ConsoleColor.Red, "Password incorrect, press enter to continue");
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
            userIO.PrintColor(ConsoleColor.Cyan, "  Register\n");

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

                userIO.PrintColor(ConsoleColor.Green, "Your account has been created. Press enter to continue");
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
            Console.WriteLine("  Enter firstname");
            string firstName = userIO.GetUserString();

            Console.WriteLine("  Enter lastname");
            string lastName = userIO.GetUserString();

            Console.WriteLine("  Enter password");
            string password = GetPassword();
            Console.WriteLine("  Confirm password");
            string confirmPassword = GetPassword();

            if (password != confirmPassword)
            {
                userIO.PrintColor(ConsoleColor.Red, "Passwords did not match. Press enter and try again");
                Console.ReadLine();
                bool validPassword = false;

                while (!validPassword)
                {
                    Console.WriteLine("  Enter password");
                    password = GetPassword();

                    Console.WriteLine("  Confirm password");
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