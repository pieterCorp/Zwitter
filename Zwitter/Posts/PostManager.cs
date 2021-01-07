using ConsoleTables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zwitter
{
    internal class PostManager
    {
        public string FolderPath { get; set; } = "../../../Db/";
        public string FilePathPosts { get; set; } = "../../../Db/Posts.txt";
        private UserIO userIO;

        public PostManager()
        {
            userIO = new UserIO();
        }

        public void CreateNewPost(User user)
        {
            Post post = new Post
            {
                PostId = GetNewId(),
                PostFromUserId = user.Id,
                PostContent = GetPostContent(),
                PostedAt = DateTime.Now
            };
            user.allPosts.Add(post);
            StorePost(post);
        }

        public void DeletePost(User user)
        {
            List<Post> posts = LoadPosts();

            int selection = ShowPostsSelection(user.allPosts, "Delete Post");

            if (selection == -1)
            {
                userIO.ShowTitle();
                userIO.PrintColor(ConsoleColor.Yellow, "No posts to be deleted, press enter to continue", true);
                Console.ReadLine();
            }
            else if (selection == user.allPosts.Count)
            {
                return; //user pressed back
            }
            else
            {
                userIO.PrintColor(ConsoleColor.DarkRed, "Are you sure you want to delete this post? y/n", true);

                if (userIO.AskYesNoQ())
                {
                    int postId = user.allPosts[selection].PostId;
                    Post postToDelete = posts.Find(x => x.PostId == postId);
                    posts.Remove(postToDelete); //remove from all posts

                    user.allPosts.RemoveAt(selection); //remove from userlist

                    UpdateDb(posts);

                    userIO.ShowTitle();

                    userIO.PrintColor(ConsoleColor.Green, "Your post was succesfully deleted!, press enter to continue", true);
                    Console.ReadLine();
                }
            }
        }

        public void UpdatePost(User user)
        {
            List<Post> posts = LoadPosts();
            int selection = ShowPostsSelection(user.allPosts, "Update Post");

            if (selection == -1)
            {
                userIO.ShowTitle();
                userIO.PrintColor(ConsoleColor.Yellow, "No posts to be updated, press enter to continue", true);
                Console.ReadLine();
            }
            else if (selection == user.allPosts.Count)
            {
                return; //user pressed back
            }
            else
            {
                string newContent = GetPostContent();
                userIO.PrintColor(ConsoleColor.DarkRed, "Are you sure you want to update this post? y/n", true);

                if (userIO.AskYesNoQ())
                {
                    user.allPosts[selection].PostContent = newContent; //update userlist

                    int postId = user.allPosts[selection].PostId;
                    Post postToUpdate = posts.Find(x => x.PostId == postId);
                    int index = posts.IndexOf(postToUpdate);
                    posts[index].PostContent = newContent; //Update in posts

                    UpdateDb(posts);

                    userIO.ShowTitle();

                    userIO.PrintColor(ConsoleColor.Green, "Your post was succesfully updated!, press enter to continue", true);
                    Console.ReadLine();
                }
            }
        }

        public void DisplayUserPosts(User user)
        {
            UserManager userManager = new UserManager();
            userManager.LoadUserPosts(user);

            userIO.ShowTitle();

            if (!user.allPosts.Any())
            {
                userIO.PrintColor(ConsoleColor.Yellow, "No posts to show", true);
            }
            else
            {
                var table = new ConsoleTable("Author", "Posted at", "Zweet Body", "Likes");
                foreach (var post in user.allPosts)
                {
                    User postAuthor = userManager.GetUserByid(post.PostFromUserId);
                    int likes = 0;
                    if (post.LikedBy != null) likes = post.LikedBy.Length;
                    table.AddRow($"{postAuthor.FirstName} {postAuthor.LastName}", post.PostedAt.ToString("dd/MM H:mm"), post.PostContent, likes);
                }
                table.Write();
            }
        }

        public void DisplayAllPosts()
        {
            UserManager userManager = new UserManager();

            userIO.ShowTitle();

            List<Post> posts = LoadPosts();

            if (!posts.Any())
            {
                userIO.PrintColor(ConsoleColor.Yellow, "No posts to show", true);
            }
            else
            {
                var table = new ConsoleTable("Author", "Posted at", "Zweet Body", "Likes");
                foreach (var post in posts)
                {
                    User postAuthor = userManager.GetUserByid(post.PostFromUserId);
                    int likes = 0;
                    if (post.LikedBy != null) likes = post.LikedBy.Length;
                    table.AddRow($"{postAuthor.FirstName} {postAuthor.LastName}", post.PostedAt.ToString("dd/MM H:mm"), post.PostContent, likes);
                }
                table.Write();
            }
        }

        public void LikePost(User user)
        {
            List<Post> posts = LoadPosts();
            List<int> usersWhoLiked = new List<int>();
            int selection = ShowPostsSelection(posts, "Like a zweet");

            if (selection != posts.Count) // user pressed back
            {
                if (posts.Count != 0) // post selection is not null
                {
                    if (posts[selection].LikedBy != null) // likedby list is not empty
                    {
                        usersWhoLiked = posts[selection].LikedBy.ToList();
                    }
                }
            }

            if (selection == -1)
            {
                userIO.ShowTitle();
                userIO.PrintColor(ConsoleColor.Yellow, "No zwwets to like, press enter to continue", true);
                Console.ReadLine();
            }
            else if (selection == posts.Count)
            {
                return; //user pressed back
            }
            else
            {
                bool alreadyLiked = false;

                foreach (var userWhoLiked in usersWhoLiked)
                {
                    if (userWhoLiked == user.Id)
                    {
                        alreadyLiked = true;
                    }
                }

                if (alreadyLiked)
                {
                    Console.WriteLine("You already liked this zweet, can't like twice");
                    Console.ReadLine();
                }
                else
                {
                    usersWhoLiked.Add(user.Id); //like as current user
                    posts[selection].LikedBy = usersWhoLiked.ToArray();
                    UpdateDb(posts);

                    userIO.PrintColor(ConsoleColor.Green, "You liked this zweet!", true);
                    Console.ReadLine();
                }
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
            string[] postPreview = new string[posts.Count + 1];

            if (!posts.Any())
            {
                return -1; //return -1 if list is empty
            }
            else
            {
                for (int i = 0; i < postPreview.Length - 1; i++)
                {
                    if (posts[i].PostContent.Length < 30)
                    {
                        postPreview[i] = posts[i].PostContent;
                    }
                    else
                    {
                        postPreview[i] = posts[i].PostContent.Substring(0, 30) + "...";
                    }
                }
            }

            postPreview[posts.Count] = "Back"; // add back as last option

            int selection = userIO.Menu(postPreview, menuTitle);

            return selection;
        }

        private int GetNewId()
        {
            List<Post> allPosts = LoadPosts();

            if (allPosts.Count < 1) return 0;

            int lastId = allPosts[allPosts.Count - 1].PostId;
            int newId = lastId + 1;
            return newId;
        }

        private string GetPostContent()
        {
            userIO.ShowTitle();

            userIO.PrintColor(ConsoleColor.DarkCyan, "What's happening?", true);
            string content = userIO.GetUserString();
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

        public List<Post> LoadPosts()
        {
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