using IKEA.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.EmailSetting
{
    public interface IEmailSettting
    {
        public void SendEmail(Email email);
    }
}
