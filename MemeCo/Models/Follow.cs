using MemeCo.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeCo.Models
{
    public class Follow
    {
        public string UserID { get; set; }
        public MemeCoUser User { get; set; }
        public string FollowerID { get; set; }
        public MemeCoUser Follower { get; set; }

        public override bool Equals(object obj)
        {
            // Syntax for as borrowed from Logan Franken's blog
            // https://www.loganfranken.com/blog/687/overriding-equals-in-c-part-1/
            Follow other = obj as Follow;
            return this.UserID == other.UserID && this.FollowerID == other.FollowerID;
        }
    }
}
