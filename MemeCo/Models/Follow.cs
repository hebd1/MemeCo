/**
 * Authors:    Jose Monterroso, Jasen Lassig, Eli Hebdon
 * Date:      December 6, 2019
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jose, Jasen, Eli - This work may not be copied for use in Academic Coursework.
 *
 * I, Jose, Eli, Jasen, certify that I wrote this code from scratch and did not copy it in part or whole from 
 * another source.  Any references used in the completion of the assignment are cited in my README file.
 *
 * File Contents
 *
 *    Follow table for database
 */

using MemeCo.Areas.Identity.Data;

namespace MemeCo.Models
{
    /// <summary>
    /// Follow table for database
    /// </summary>
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