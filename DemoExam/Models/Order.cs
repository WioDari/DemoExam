using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam.Models
{
    public class Order
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string art { get; set; }
        public DateTime order_date { get; set; } = DateTime.UtcNow;
        public DateTime ship_date { get; set; }
        
        [ForeignKey("Address")]
        public int addressid { get; set; }
        public string client { get; set; }
        public int code { get; set; }
        public string status { get; set; }

        public Address address { get; set; }
    }
}
