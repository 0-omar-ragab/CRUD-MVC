using IKEA.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.EmailSetting
{
    public class EmailSettting : IEmailSettting
    {
        public void SendEmail(Email email)
        {

            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            //Sender- Reciver
            //Reciver= oa242895@gmail.com => User Who Try To reset Password
            client.Credentials = new NetworkCredential("oa242895@gmail.com", "osfxvcjexxjyccye");//Generate Password
            client.Send("oa242895@gmail.com", email.To, email.Subject, email.Body);

        }
    }
}
