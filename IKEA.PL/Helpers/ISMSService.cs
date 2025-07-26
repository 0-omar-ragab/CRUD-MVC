using IKEA.DAL.Entities.SMS;
using Twilio.Rest.Api.V2010.Account;

namespace IKEA.PL.Helpers
{
    public interface ISMSService
    {
        public MessageResource SendSms(SmsMessage smsMessage);
    }
}
