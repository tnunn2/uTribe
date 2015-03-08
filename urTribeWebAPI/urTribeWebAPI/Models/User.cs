using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urTribeWebAPI.Models
{
    public class User
    {
        public string Name { get; set; }
        private List<Channel> conversations;
        public Guid Id { get; set; }
 
        public User(string name)
        {
            conversations = new List<Channel>();
            Name = name;
            Id = new Guid();
        }
    }
}