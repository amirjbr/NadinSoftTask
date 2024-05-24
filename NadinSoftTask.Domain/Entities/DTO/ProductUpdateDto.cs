using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadinSoftTask.Domain.Entities.DTO
{
    public class ProductUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(11)]
        public string ManufacturePhone { get; set; }
        [Required]
        [EmailAddress]
        public string ManufactureEmail { get; set; }
    }
}
