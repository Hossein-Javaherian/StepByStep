using System.ComponentModel.DataAnnotations;

namespace StepByStep.Models
{
    public class Bio
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Family { get; set; }
    }
}