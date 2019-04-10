using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Threading;
using System.Security.Cryptography.X509Certificates;


namespace Autoreport_v2
{
    class EmailClient
    {
        public static Stack<Email> emails = new Stack<Email>();
        private static string fromemail;
        private static string fromname;
        private static Encoding code;
        private static string host;
        private static int port;
        private static string password;
        private static SmtpClient client = new SmtpClient();
        public EmailClient(string fromemail, string fromname, Encoding code, string host, int port, string password)
        {
            EmailClient.fromemail = fromemail;
            EmailClient.fromname = fromname;
            EmailClient.code = code;
            EmailClient.host = host;
            EmailClient.port = port;
            EmailClient.password = password;
            client.Credentials = new System.Net.NetworkCredential(fromemail, password);
            client.Port = port;
            client.Host = host;
            client.EnableSsl = true;
        }
        public void Start()
        {
            while (true)
            {
                if (emails.Count > 0)
                {
                    Email email = emails.Pop();
                    MailMessage msg = new MailMessage();
                    msg.From = new MailAddress(fromemail, fromname, code);
                    msg.BodyEncoding = code;
                    msg.SubjectEncoding = code;
                    msg.Body = email.txt;
                    msg.Subject = email.title;
                    for (int i = 0; i < email.emails.Length; i++) { msg.To.Add(email.emails[i]); }
                    Attachment data = new Attachment(email.file);
                    msg.Attachments.Add(data);
                    client.Timeout = 600000;
                    try
                    {
                        ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
                        client.Send(msg);
                        Console.WriteLine(DateTime.Now + ":项目" + email.itemname + "的报表:" + email.file + "邮件已发送至指定邮箱");
                        Client.sw.WriteLine(DateTime.Now + ":项目" + email.itemname + "的报表:" + email.file + "邮件已发送至指定邮箱");
                        msg.Dispose();
                        if (File.Exists(Client.webanswerpath + @"\" + email.itemname + ".json")) { File.Delete(Client.webanswerpath + @"\" + email.itemname + ".json"); }
                    }
                    catch (SmtpException e)
                    {
                        Console.WriteLine(DateTime.Now + ":项目" + email.itemname + "的报表:" + email.file + "邮件发送失败失败原因:" + e.ToString() + ":邮件推送至栈顶稍后为您重试");
                        Client.sw.WriteLine(DateTime.Now + ":项目" + email.itemname + "的报表:" + email.file + "邮件发送失败失败原因:" + e.ToString() + ":邮件推送至栈顶稍后为您重试");
                        Thread.Sleep(60);
                        emails.Push(email);

                    }


                }
                Thread.Sleep(1);
            }
        }

    }
}
