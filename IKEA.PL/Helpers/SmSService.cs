using IKEA.DAL.Entities.SMS;
using IKEA.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace IKEA.PL.Helpers
{
    public class SmSService(IOptions<TwilioSetting> _options): ISMSService
    {
        public MessageResource SendSms(SmsMessage smsMessage)
        {
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);
            //var message = MessageResource.Create(
            //    body: smsMessage.Body,
            //    from: new Twilio.Types.PhoneNumber(_options.Value.TwilioPhoneNumber),
            //    to: new Twilio.Types.PhoneNumber(smsMessage.PhoneNumber)
            //)

            string phoneNumber = smsMessage.PhoneNumber;

            // لو الرقم مصري وبيبدأ بـ 010 أو 011 أو 012 أو 015
            if (phoneNumber.StartsWith("01"))
            {
                phoneNumber = "+2" + phoneNumber;
            }

            var message = MessageResource.Create(
                body: smsMessage.Body,
                from: new Twilio.Types.PhoneNumber(_options.Value.TwilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );

            return message;
        }
    }
}
