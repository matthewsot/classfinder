using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classfinder
{
    /// <summary>
    /// Provides functions that you can call to use your own email services.
    /// </summary>
    public class Email
    {
        //Pipes messages to the right place
        public void Receive(string From, string To, string Subject, string Contents)
        {

        }

        public void Send(string From, string FromName, string To, string ToName, string Subject, string Text)
        {
            //Pre-written code for SMTP support.
            //If you're not using SMTP, replace this with your own code.
            var settings = Config.GetValues(new string[] { "SMTP Server", "SMTP Port", "SMTP User", "SMTP Pass" });
            SmtpMessage msg = new SmtpMessage();
            msg.To = new AddressCollection();
            msg.To.Add(new Address(To, ToName));
            msg.From = new Address(From, FromName);
            msg.BodyText.Text = Text;
            msg.BuildMimePartTree();
            msg.Send(settings["SMTP Server"], Int32.Parse(settings["SMTP Port"]), settings["SMTP User"], settings["SMTP Pass"], SaslMechanism.Login);
        }
    }
}