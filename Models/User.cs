using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoExam.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string role { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}
