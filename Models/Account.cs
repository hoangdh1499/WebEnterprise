using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebEnterprise.Models
{

    public class Account
    {
      
        [Required(ErrorMessage = "Field can't be empty")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}