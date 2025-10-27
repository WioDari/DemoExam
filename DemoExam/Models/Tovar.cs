using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam.Models
{
    public class Tovar
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string art { get; set; }
        public string name { get; set; }
        public string dim { get; set; }
        public double price { get; set; }
        public string supplier { get; set; }
        public string creator { get; set; }
        public string category { get; set; }
        public int discount { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public string photo { get; set; }
    }
}
