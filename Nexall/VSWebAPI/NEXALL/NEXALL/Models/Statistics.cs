using System.ComponentModel.DataAnnotations;

namespace NEXALL.Models
{
    public class Statistics
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public double Speed { get; set; }

        [Required]
        [StringLength(10)]
        public string RegistrationNumber { get; set; }
    }
}