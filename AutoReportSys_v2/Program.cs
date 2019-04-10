using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autoreport_v2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AutoReportSys_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonfile =File.ReadAllText(args[0],Encoding.UTF8);
            JObject jobject = JObject.Parse(jsonfile);
            string mysqlip = jobject["mysqlip"].ToString();
            uint mysqlport = Convert.ToUInt32(jobject["mysqlport"].ToString());
            string mysqluser = jobject["mysqluser"].ToString();
            string mysqlpassword = jobject["mysqlpassword"].ToString();
            string tempcsvpath = jobject["tempcsvpath"].ToString();
            string frommail = jobject["frommail"].ToString();
            string fromname = jobject["fromname"].ToString();
            string smtpip = jobject["smtpip"].ToString();
            int smtpport = Convert.ToInt32(jobject["smtpport"].ToString());
            string mailpassword = jobject["mailpassword"].ToString();
            string outpath = jobject["outpath"].ToString();
            string itemsfile = jobject["itemsfile"].ToString();
            string tempitempath = jobject["tempitempath"].ToString();
            string reportsfile = jobject["reportsfile"].ToString();
            string modulesfile = jobject["modulesfile"].ToString();
            string towebfile = jobject["towebfile"].ToString();
            string fromwebfile = jobject["fromwebfile"].ToString();
            string keyfile = jobject["keyfile"].ToString();

            Autoreport_v2.Client client = new Autoreport_v2.Client(mysqlip, mysqlport, mysqluser, mysqlpassword, tempcsvpath, frommail, fromname, Encoding.UTF8, smtpip, smtpport, mailpassword, outpath, itemsfile, tempitempath, reportsfile, modulesfile, towebfile, fromwebfile, keyfile);
            client.Start();
            //Console.WriteLine(jobject.ToString());
        }
    }
}
