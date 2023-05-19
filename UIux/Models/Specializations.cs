using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UIux.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationID { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Specialization name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization image is mandatory.")]
        public string SpecializationImage { get; set; }

        [Required(ErrorMessage = "Specialization description is mandatory.")]
        [StringLength(256, ErrorMessage = "The description is maximum 256 character long.")]
        public string SpecializationDescription { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }

    }
}