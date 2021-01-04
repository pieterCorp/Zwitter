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

        public void CreateNewPost()
        {
            Post post = new Post();

            post.postId = GetNewId();
            post.postFromUserId = 0;
            post.postContent = GetPostContent();
            post.postedAt = DateTime.Now;

            StorePost(post);
        }

        public void DeletePost()
        {
            List<Post> posts = LoadPosts();
            int selection = ShowPostsSelection(posts, "Delete Post");

            if (!posts.Any())
            {
                Console.WriteLine("No posts to be deleted, press enter to continue");
                Console.ReadLine();
            }
            else
            {
                posts.RemoveAt(selection);
                UpdateDb(posts);
            }
        }

        public void UpdatePost()
        {
            List<Post> posts = LoadPosts();
            int selection = ShowPostsSelection(posts, "Update Post");

            if (!posts.Any())
            {
                Console.WriteLine("No posts to be updated, press enter to continue");
                Console.ReadLine();
            }
            else
            {
                posts[selection].postContent = "new content";  //update post here...
                UpdateDb(posts);
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

            string[] postPreview = new string[posts.Count];

            for (int i = 0; i < postPreview.Length; i++)
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

            int selection = UserIO.Menu(postPreview, menuTitle);

            return selection;
        }

        public void DisplayPosts()
        {
            List<Post> posts = LoadPosts();
            foreach (var post in posts)
            {
                Console.WriteLine(post.postContent);
                Console.WriteLine(post.postId);
                Console.WriteLine(post.postedAt);
            }
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
            string content = "test";
            //ask user for content
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