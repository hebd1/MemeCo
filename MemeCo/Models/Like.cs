/**
 * Authors:    Jose Monterroso, Jasen Lassig, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, Jasen, Eli, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Like table for database
 */

using MemeCo.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemeCo.Models
{
    /// <summary>
    /// Like table for database
    /// </summary>
    public class Like
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string MemeCoUserID { get; set; }

        [Required]
        public bool Liked { get; set; }
        public DateTime TimeLiked { get; set; }
        public Post Post { get; set; }
        public MemeCoUser User { get; set; }

        public Like()
        {
            TimeLiked = DateTime.UtcNow;
        }
    }
}