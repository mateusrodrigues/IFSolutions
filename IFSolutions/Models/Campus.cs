using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class Campus
    {
        public int CampusID { get; set; }

        [Required]
        [Display(Name = "Campus")]
        public string Description { get; set; }

        public virtual ICollection<Petition> Petitions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}