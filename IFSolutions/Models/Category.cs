using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual ICollection<Petition> Petitions { get; set; }
    }
}