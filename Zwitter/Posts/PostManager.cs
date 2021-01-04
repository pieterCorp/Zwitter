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

            post.postId = GetPostId();

            post.postContent = GetPostContent();

            DateTime date = DateTime.Now;

            //strore
            StorePost(post);
        }

        public void DeletePost()
        {
            //delete post
        }

        public void UpdatePost()
        {
        }

        public void DisplayPosts()
        {
            List<Post> posts = LoadPosts();
            foreach (var post in posts)
            {
                Console.WriteLine(post.postContent);
                Console.WriteLine(post.postId);
            }
        }

        private int GetPostId()
        {
            int postId = 0;

            //open filereader and get latest id, add 1
            return postId;
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
            List<Post> GlobalPosts = new List<Post>();

            fileManager.CreateFolder(FolderPath);
            fileManager.CreateFile(FilePathPosts);
            postsJson = fileManager.LoadAllFiles(FilePathPosts);

            for (int i = 0; i < postsJson.Count; i++)
            {
                string json = postsJson[i];
                Post result = JsonConvert.DeserializeObject<Post>(json);
                GlobalPosts.Add(result);
            }
            return GlobalPosts;
        }
    }
}