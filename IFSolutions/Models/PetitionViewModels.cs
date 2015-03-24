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
        [Display(Name = "Categoria")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Campus")]
        public int CampusID { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
    }

    public class EditPetitionViewModel
    {
        [Required]
        public int PetitionID { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Campus")]
        public int CampusID { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
    }

    public class CommentPetitionViewModel
    {
        [Required]
        public int PetitionID { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MinLength(1)]
        public string Comment { get; set; }
    }
}