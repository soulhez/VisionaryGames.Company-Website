using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace VisionaryGames.Website.Models
{
    public class Subscribe
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}