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

        public User Login()
        {
            User defaultUser = new User(); //return default user as not logged in

            UpdateRegisteredEmail();
            //ask for email and check if in system

            Console.WriteLine("Enter Email");
            string eMail = UserIO.GetUserString();

            if (!CheckEmailInSystem(eMail))
            {
                Console.WriteLine("email not found. Press enter to continue");
                Console.ReadLine();
                return defaultUser;
            }
            //ask for password
            Console.WriteLine("Enter Password");
            string password = UserIO.GetUserString();

            User user = GetUserByEmail(eMail);

            //check if email and pass are correct
            if (password != user.Password)
            {
                Console.WriteLine("Password incorrect, press enter to continue");
                Console.ReadLine();
                return defaultUser;
            }

            user.LoggedIn = true;
            UserIO.PrintColor(ConsoleColor.Green, $"You succesfully logged in as {user.FirstName} {user.LastName}", true);
            Console.ReadLine();
            return user;
        }

        public void Logout(User user)
        {
            //set logged in to false
            user.LoggedIn = false;
            UserIO.PrintColor(ConsoleColor.Green, "You succesfully logged out", true);
            Console.ReadLine();
        }

        public void Register()
        {
            UpdateRegisteredEmail();

            Console.WriteLine("Enter Email");
            string eMail = UserIO.GetUserString();

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
                UpdateRegisteredEmail();

                UserIO.PrintColor(ConsoleColor.Green, "Your account has been created. Press enter to continue", true);
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
            User newUser = new User();

            Console.WriteLine("Enter firstname");
            string firstName = UserIO.GetUserString();

            Console.WriteLine("Enter lastname");
            string lastName = UserIO.GetUserString();

            Console.WriteLine("Enter password");
            string password = UserIO.GetUserString();

            // make unique id;
            Filemanager fileManager = new Filemanager();
            int newId = fileManager.CountLinesFile(FilePathUsers) + 1;

            newUser.FirstName = firstName;
            newUser.LastName = lastName;
            newUser.Email = eMail;
            newUser.Password = password;
            newUser.dateRegistered = DateTime.Now;
            newUser.Id = newId;

            return newUser;
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

        private void UpdateRegisteredEmail()
        {
            List<User> users = LoadUsers();
            EmailRegisteredUsers.Clear();

            foreach (var user in users)
            {
                EmailRegisteredUsers.Add(user.Email);
            }
        }

        private User GetUserByEmail(string eMail)
        {
            int index = EmailRegisteredUsers.IndexOf(eMail);
            List<User> users = LoadUsers();
            return users[index];
        }

        public void test()
        {
            for (int i = 0; i < EmailRegisteredUsers.Count; i++)
            {
                Console.WriteLine(EmailRegisteredUsers[i]);
            }
        }
    }
}