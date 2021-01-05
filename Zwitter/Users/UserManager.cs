using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Zwitter
{
    internal class UserManager
    {
        public string FolderPath { get; set; } = "../../../Db/";
        public string FilePathUsers { get; set; } = "../../../Db/Users.txt";

        public string[] EmailRegisteredUsers { get; set; }

        public void Login()
        {
            //ask for email and check if in system
            //check if email and pass are correct
            //check if not logged in already
            //set logged in to true
        }

        public void Logout()
        {
            //set logged in to false
        }

        public void Register()
        {
            Console.WriteLine("Enter Email");
            string eMail = UserIO.GetUserString();

            if (CheckEmailInSystem(eMail))
            {
                Console.WriteLine("email already exists, do you want to login?");
                //redirect to login
                //or return
            }
            else
            {
                User newUser = MakeNewAccount(eMail);

                StoreUser(newUser);

                UserIO.PrintColor(ConsoleColor.Green, "Your account has been created. Press enter to continue", true);
                Console.ReadLine();

                //update emailsRegistered array
                //UpdateRegisteredEmail();
            }
        }

        private bool CheckEmailInSystem(string eMail)
        {
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
            for (int i = 0; i < users.Count - 1; i++)
            {
                EmailRegisteredUsers[i] = users[i].Email;
            }
        }
    }
}