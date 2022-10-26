using SocialMediaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.ViewModels
{
    internal class FeedViewModel
    {
        public ObservableCollection<Post> Posts { get; set; }

        public FeedViewModel()
        {
            // TODO: Replace all this with actual data from an actual API connection
            Posts = new ObservableCollection<Post>();
            User user1 = new User {
                Id = 1,
                Username = "User1",
                Email = "test@mail.com"
            };
            Posts.Add(new Post
            {
                Id = 1, 
                Title = "Hello World", 
                Body = "This is such a cheap way of doing it", 
                Created = DateTime.Now, 
                Poster = user1, 
                LikedBy = new List<User>()
            });
        }
    }
}
