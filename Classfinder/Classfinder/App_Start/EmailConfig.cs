using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Classfinder
{
    public class EmailConfig
    {
        //pre-written code for IMAP. If you're not using IMAP, BYOC
        public static void BeginReceiving()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(StartIdleProcess);

            if (worker.IsBusy)
                worker.CancelAsync();

            worker.RunWorkerAsync();
        }

        public static Imap4Client imap;
        public static Mailbox inbox;
        private static async void StartIdleProcess(object sender, DoWorkEventArgs e)
        {
            if (imap != null && imap.IsConnected)
            {
                imap.StopIdle();
                imap.Disconnect();
            }

            imap = new Imap4Client();

            var settings = Config.GetValues(new string[] { "IMAP Server", "IMAP Port", "IMAP User", "IMAP Pass" });

            await imap.ConnectAsync(settings["IMAP Server"], int.Parse(settings["IMAP Port"]));
            imap.Login(settings["IMAP User"], settings["IMAP Pass"]);

            var inbox = imap.SelectMailbox("INBOX");
            imap.NewMessageReceived += new NewMessageReceivedEventHandler(NewMessageReceived);

            inbox.Subscribe();

            imap.StartIdle();
        }

        public static void NewMessageReceived(object source, NewMessageReceivedEventArgs e)
        {
            var msg = inbox;
        }
    }
}