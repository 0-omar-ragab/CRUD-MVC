using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Entities.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public bool IsAgree { get; set; }
    }
}
