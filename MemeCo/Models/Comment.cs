using MemeCo.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemeCo.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string MemeCoUserID { get; set; }
        [Required]
        public Guid PostID { get; set; }
        public DateTime TimeCommented { get; set; }
        public Post Post { get; set; }
        public MemeCoUser User { get; set; }
    }
}
