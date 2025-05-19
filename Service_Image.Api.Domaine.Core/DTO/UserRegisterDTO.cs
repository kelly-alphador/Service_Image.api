using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Image.Api.Domaine.Core.DTO
{
    public class UserRegisterDTO
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public string ConfirmPassword {  get; set; }
    }
}
