using MemeCo.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemeCo.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        [Required]
        public string MemeCoUserID { get; set; }
        [Required]
        public byte[] Meme { get; set; }
        public string Description { get; set; }
        public DateTime TimePosted { get; set; }
        public IEnumerable<Like> Likes { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public MemeCoUser User { get; set; }
        
        public int? TempleteID { get; set; }

        public Post()
        {
            TimePosted = DateTime.UtcNow;
        }
    }
}
