using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;

namespace Zwitter
{
    internal class PostManager
    {
        public string FolderPath { get; set; } = "../../../Db/";
        public string FilePathPosts { get; set; } = "../../../Db/Posts.txt";

        public void CreateNewPost()
        {
            Post post = new Post
            {
                postId = GetNewId(),
                postFromUserId = 0,
                postContent = GetPostContent(),
                postedAt = DateTime.Now
            };

            StorePost(post);
        }

        public void DeletePost()
        {
            List<Post> posts = LoadPosts();
            int selection = ShowPostsSelection(posts, "Delete Post");

            if (selection == -1)
            {
                UserIO.PrintColor(ConsoleColor.Yellow, "No posts to be deleted, press enter to continue", true);
                Console.ReadLine();
            }
            else if (selection == posts.Count)
            {
                return; //user pressed back
            }
            else
            {
                posts.RemoveAt(selection);

                UserIO.PrintColor(ConsoleColor.DarkRed, "Are you sure you want to delete this post? y/n", true);

                if (UserIO.AskYesNoQ())
                {
                    UpdateDb(posts);
                    UserIO.PrintColor(ConsoleColor.Green, "Your post was succesfully deleted!, press enter to continue", true);
                    Console.ReadLine();
                }
            }
        }

        public void UpdatePost()
        {
            List<Post> posts = LoadPosts();
            int selection = ShowPostsSelection(posts, "Update Post");

            if (selection == -1)
            {
                UserIO.PrintColor(ConsoleColor.Yellow, "No posts to be updated, press enter to continue", true);
                Console.ReadLine();
            }
            else if (selection == posts.Count)
            {
                return; //user pressed back
            }
            else
            {
                posts[selection].postContent = GetPostContent();

                UserIO.PrintColor(ConsoleColor.DarkRed, "Are you sure you want to update this post? y/n", true);

                if (UserIO.AskYesNoQ())
                {
                    UpdateDb(posts);
                    UserIO.PrintColor(ConsoleColor.Green, "Your post was succesfully updated!, press enter to continue", true);
                    Console.ReadLine();
                }
            }
        }

        public void DisplayPosts()
        {
            Console.Clear();
            List<Post> posts = LoadPosts();

            if (!posts.Any())
            {
                UserIO.PrintColor(ConsoleColor.Yellow, "No posts to show", true);
            }
            else
            {
                var table = new ConsoleTable("id", "Posted at", "Zweet Body");
                foreach (var post in posts)
                {
                    table.AddRow(post.postId, post.postedAt, post.postContent);
                }
                table.Write();
            }
        }

        private void UpdateDb(List<Post> posts)
        {
            Filemanager fileManager = new Filemanager();
            fileManager.DeleteFile(FilePathPosts);
            foreach (var post in posts)
            {
                StorePost(post);
            }
        }

        private int ShowPostsSelection(List<Post> posts, string menuTitle)
        {
            Console.Clear();

            string[] postPreview = new string[posts.Count + 1];

            if (!posts.Any())
            {
                return -1; //return -1 if list is empty
            }
            else
            {
                for (int i = 0; i < postPreview.Length - 1; i++)
                {
                    if (posts[i].postContent.Length < 30)
                    {
                        postPreview[i] = posts[i].postContent;
                    }
                    else
                    {
                        postPreview[i] = posts[i].postContent.Substring(0, 30);
                    }
                }
            }

            postPreview[posts.Count] = "Back"; // add back as last option

            int selection = UserIO.Menu(postPreview, "Posts");

            return selection;
        }

        private int GetNewId()
        {
            List<Post> allPosts = LoadPosts();

            if (allPosts.Count < 1) return 0;

            int lastId = allPosts[allPosts.Count - 1].postId;
            int newId = lastId + 1;
            return newId;
        }

        private string GetPostContent()
        {
            //ask user for content
            Console.Clear();
            UserIO.PrintColor(ConsoleColor.Cyan, UserIO.zwitterAscii, true);
            UserIO.PrintColor(ConsoleColor.DarkCyan, "What's happening?", true);
            string content = UserIO.GetUserString();
            return content;
        }

        private void StorePost(Post post)
        {
            Filemanager fileManager = new Filemanager();
            string json = System.Text.Json.JsonSerializer.Serialize(post);

            fileManager.CreateFolder(FolderPath);
            fileManager.CreateFile(FilePathPosts);
            fileManager.WriteDataToFile(json, FilePathPosts);
        }

        private List<Post> LoadPosts()
        {
            UserIO.PrintColor(ConsoleColor.Cyan, UserIO.zwitterAscii, true);
            Filemanager fileManager = new Filemanager();
            List<string> postsJson = new List<string>();
            List<Post> Posts = new List<Post>();

            fileManager.CreateFolder(FolderPath);
            fileManager.CreateFile(FilePathPosts);
            postsJson = fileManager.LoadAllFiles(FilePathPosts);

            for (int i = 0; i < postsJson.Count; i++)
            {
                string json = postsJson[i];
                Post result = JsonConvert.DeserializeObject<Post>(json);
                Posts.Add(result);
            }
            return Posts;
        }
    }
}