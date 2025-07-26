using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Entities.SMS
{
    public class SmsMessage
    {
        public string PhoneNumber { get; set; } =null!;
        public string Body { get; set; } = null!;
    }
}
