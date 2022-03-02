using System.ComponentModel.DataAnnotations;

namespace StepByStep.Models
{
    public class Contact
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Fax { get; set; }
    }
}