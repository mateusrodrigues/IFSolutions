using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Required]
        public int PetitionID { get; set; }
        public virtual Petition Petition { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string Content { get; set; }

        public virtual ICollection<CommentComplaint> Complaints { get; set; }
    }
}