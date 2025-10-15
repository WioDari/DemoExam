using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam.Models
{
    public class Address
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string address { get; set; }

        public ICollection<Order> orders { get; set; }
    }
}
