using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class CommentComplaint
    {
        public int CommentComplaintID { get; set; }

        [Required]
        public int CommentID { get; set; }
        public virtual Comment Comment { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}