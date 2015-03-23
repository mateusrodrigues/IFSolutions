﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class Petition
    {
        public int PetitionID { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = "Campus")]
        public int CampusID { get; set; }
        public virtual Campus Campus { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Data de criação")]
        public DateTime DateCreated { get; set; }

        [Required]
        public bool Solved { get; set; }

        [Display(Name = "Data de resolução")]
        public DateTime? DateSolved { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PetitionComplaint> Complaints { get; set; }
        public virtual ICollection<Signature> Signatures { get; set; }
    }
}