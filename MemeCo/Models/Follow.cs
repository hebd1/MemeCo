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
    }
}
