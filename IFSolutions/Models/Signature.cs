using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IFSolutions.Models
{
    public class Signature
    {
        public int SignatureID { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int PetitionID { get; set; }
        public virtual Petition Petition { get; set; }
    }
}