using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Company_Website.Models
{
    public class Subscribe
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}