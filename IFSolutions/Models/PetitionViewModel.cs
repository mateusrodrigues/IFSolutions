using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class CreatePetitionViewModel
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int CampusID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class EditPetitionViewModel
    {
        [Required]
        public int PetitionID { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int CampusID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}