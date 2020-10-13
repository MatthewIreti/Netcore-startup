using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.DTO
{
    public class Common
    {
    }

    public class Organization
    {
        public string OrganizationName { get; set; }
    }

    public class UserEmail
    {
        public string Value { get; set; }
    }
    public class PasswordReset
    {
        public string Email { get; set; }
        public string ActivationCode { get; set; }
    }

    public class EmailPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
