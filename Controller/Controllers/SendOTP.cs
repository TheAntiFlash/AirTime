using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendOTP : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail(string clientEmail, string OTP)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nolan22@ethereal.email"));//sender email
            email.To.Add(MailboxAddress.Parse(clientEmail));

            email.Subject = "OTP for Airtime.pk for Registration";

            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = "Your OTP is " + OTP
            };

            using var smtpClient = new SmtpClient();

            smtpClient.Connect("smtp.gmail.email", 587 , SecureSocketOptions.StartTls);

            smtpClient.Authenticate("nolan22@ethereal.email", "8gJHu7PFWWUEudefcY");

            smtpClient.Send(email);

            smtpClient.Disconnect(true);



            return Ok();
        }
    }
}
