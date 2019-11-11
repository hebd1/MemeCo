using MemeCo.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemeCo.Models
{
    public class Like
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string MemeCoUserID { get; set; }
        //[Required]
        //public Guid PostID { get; set; }
        [Required]
        public bool Liked { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimeLiked { get; set; }
        public Post Post { get; set; }
        public MemeCoUser User { get; set; }
    }
}
