using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;
using MykeyDecoding;
using System.Diagnostics;
namespace Autoreport_v2
{
    public class Client
    {
        public static JObject modules;
        public static JObject reports;
        public static JObject items;
        public static Stack<Item> items_finished = new Stack<Item>();
        public static StringWriter sw = new StringWriter();
        private string sship = ""; int sshport = 0; string sshuser = ""; string sshpassword = ""; string mysqlip; uint mysqlport; string mysqluser; string mysqlpassword; string csvtemppath;
        private string frommail; string fromname; Encoding code; string smtphost; int smtpport; string frompassword;
        public static string outreportpath;
        private ReportClient reportClient;
        private CreateCsvClient createCsvClient;
        private EmailClient emailClient;
        private string dailyreportconfigfile;
        private string tempreportpath;
        private string reportdetialfile;
        private string modulesfile;
        public static string webanswerpath;
        public static string webconfigpath;
        private string key;
        public Client(string sship, int sshport, string sshuser, string sshpassword, string mysqlip, uint mysqlport, string mysqluser, string mysqlpassword, string csvtemppath, string frommail, string fromname, Encoding code, string smtphost, int smtpport, string frompassword, string reportpath, string dailyreportconfigfile, string tempreportpath, string reportdetialfile,string modulesfile, string webanswerpath, string webconfigpath,string keypath)
        {
            FileStream fs = new FileStream(keypath, FileMode.Open);
            byte[] datas = new byte[fs.Length];
            fs.Read(datas, 0, datas.Length);
            key =Decondings.Decongding(Encoding.Default.GetString(datas));
            this.sship = sship;
            this.sshport = sshport;
            this.sshuser = sshuser;
            this.sshpassword = sshpassword;
            this.mysqlip = mysqlip;
            this.mysqlport = mysqlport;
            this.mysqluser = mysqluser;
            this.mysqlpassword = mysqlpassword;
            this.csvtemppath = csvtemppath;
            this.frommail = frommail;
            this.fromname = fromname;
            this.code = code;
            this.smtphost = smtphost;
            this.smtpport = smtpport;
            this.frompassword = frompassword;
            Client.outreportpath = reportpath;
            reportClient = new ReportClient(Client.outreportpath);
            createCsvClient = new CreateCsvClient(this.sship, this.sshport, this.sshuser, this.sshpassword, this.mysqlip, this.mysqlport, this.mysqluser, this.mysqlpassword, this.csvtemppath);
            emailClient = new EmailClient(this.frommail, this.fromname, this.code, this.smtphost, this.smtpport, this.frompassword);
            this.dailyreportconfigfile = dailyreportconfigfile;
            this.tempreportpath = tempreportpath;
            this.reportdetialfile = reportdetialfile;
            this.modulesfile = modulesfile;
            reports = GetjobjectByfile(reportdetialfile, this.key);
            modules = GetjobjectByfile(modulesfile, this.key);
            items = GetjobjectByfile(dailyreportconfigfile, this.key);

            Client.webconfigpath = webconfigpath;
            Client.webanswerpath = webanswerpath;
        }
        public Client(string mysqlip, uint mysqlport, string mysqluser, string mysqlpassword, string csvtemppath, string frommail, string fromname, Encoding code, string smtphost, int smtpport, string frompassword, string reportpath, string dailyreportconfigfile, string tempreportpath, string reportdetialfile, string modulesfile, string webanswerpath, string webconfigpath, string keypath)
        {
            MykeyDecoding.Decondings decoding = new Decondings();
            FileStream fs = new FileStream(keypath, FileMode.Open);
            byte[] datas = new byte[fs.Length];
            fs.Read(datas, 0, datas.Length);
            key = Decondings.Decongding(Encoding.Default.GetString(datas));

            this.mysqlip = mysqlip;
            this.mysqlport = mysqlport;
            this.mysqluser = mysqluser;
            this.mysqlpassword = mysqlpassword;
            this.csvtemppath = csvtemppath;
            this.frommail = frommail;
            this.fromname = fromname;
            this.code = code;
            this.smtphost = smtphost;
            this.smtpport = smtpport;
            this.frompassword = frompassword;
            Client.outreportpath = reportpath;

            reportClient = new ReportClient(Client.outreportpath);
            createCsvClient = new CreateCsvClient(this.mysqlip, this.mysqlport, this.mysqluser, this.mysqlpassword, this.csvtemppath);
            emailClient = new EmailClient(this.frommail, this.fromname, this.code, this.smtphost, this.smtpport, this.frompassword);
            this.dailyreportconfigfile = dailyreportconfigfile;
            this.tempreportpath = tempreportpath;
            this.reportdetialfile = reportdetialfile;
            this.modulesfile = modulesfile;
            reports = GetjobjectByfile(reportdetialfile, this.key);
            modules = GetjobjectByfile(modulesfile, this.key);
            items = GetjobjectByfile(dailyreportconfigfile, this.key);
            Client.webconfigpath = webconfigpath;
            Client.webanswerpath = webanswerpath;
        }
        public void Ini()
        {
            if (sship == "")
            {
                createCsvClient.ConnectWithoutSsh();
            }
            else
            {
                createCsvClient.SshConnect();

            }

        }

        public void Start()
        {
            Ini();
            Thread[] ts = new Thread[7];
            ts[0] = new Thread(new ThreadStart(reportClient.Start));
            ts[1] = new Thread(new ThreadStart(createCsvClient.Start));
            ts[2] = new Thread(new ThreadStart(emailClient.Start));
            for (int i = 0; i < 3; i++) { ts[i].Start(); }
            ts[3] = new Thread(new ThreadStart(DailyRun));
            ts[4] = new Thread(new ThreadStart(TempRun));
            ts[5] = new Thread(new ThreadStart(Cleartemp));
            ts[6] = new Thread(new ThreadStart(TransToANSI));
            ts[3].Start();
            ts[4].Start();
            ts[5].Start();
            ts[6].Start();




        }
        private void DailyRun()
        {
            Console.WriteLine("请输入日报延迟的天数");
            string offsetdaysstr = Console.ReadLine();
            int offsetdays = 0;
            try
            {
                offsetdays = Convert.ToInt32(offsetdaysstr);
            }
            catch
            {

            }
            DateTime daybefor = DateTime.Today.AddDays(-1);
            DateTime today = DateTime.Today;
            DateTime datestart = today.AddDays(offsetdays);
            Console.WriteLine(datestart.ToString());
            Console.WriteLine(today != daybefor && today >= datestart);
            while (true)
            {
                today = DateTime.Today;
                
                if (today != daybefor&&today>=datestart)
                {

                    Queue<Item> items = GetItemsByfile(dailyreportconfigfile, key);
                    while (items.Count > 0)
                    {
                        Item tempitem = items.Dequeue();
                        if (DateTime.Now.CompareTo(Convert.ToDateTime(tempitem.itemstarttime)) >= 0)
                        {
                            CreateCsvClient.items.Push(tempitem);
                            Console.WriteLine(DateTime.Now + ":日报" + tempitem.name + "已推送到客户端处理");
                            Client.sw.WriteLine(DateTime.Now + ":日报" + tempitem.name + "已推送到客户端处理");
                        }
                        else
                        {
                            items.Enqueue(tempitem);
                        }

                        Thread.Sleep(1);

                    }


                    daybefor = today;
                }
               Thread.Sleep(1);
            }

        }
        private void TempRun()
        {
            while (true)
            {
                FileInfo[] files = new DirectoryInfo(tempreportpath).GetFiles("*.json");
                DateTime dt1 = DateTime.Now;
                while (DateTime.Now <= dt1.AddSeconds(5))
                {
                    Thread.Sleep(1);
                }
                for (int i = 0; i < files.Length; i++)
                {
                    Queue<Item> items = GetItemsByfile(tempreportpath + @"\" + files[i].ToString());
                    File.Copy(tempreportpath + @"\" + files[i].ToString(), Client.webanswerpath + @"\" + files[i].ToString());
                    File.Delete(tempreportpath + @"\" + files[i].ToString());

                    while (items.Count>0)
                    {
                        Item tempitem = items.Dequeue();
                        CreateCsvClient.items.Push(tempitem);
                        Console.WriteLine(DateTime.Now + ":临时报" + tempitem.name + "已推送到客户端处理");
                        Client.sw.WriteLine(DateTime.Now + ":临时报" + tempitem.name + "已推送到客户端处理");

                    }
                }
                Thread.Sleep(1);
            }
        }
        private void TransToANSI()
        {
            while (true)
            {
                FileInfo[] files = new DirectoryInfo(Client.webconfigpath).GetFiles("*.json");
                DateTime dt1 = DateTime.Now;
                while (DateTime.Now <= dt1.AddSeconds(5))
                {
                    Thread.Sleep(1);
                }


                for (int i = 0; i < files.Length; i++)
                {
                    StreamReader sr = new StreamReader(Client.webconfigpath + @"\" + files[i], Encoding.UTF8, false);
                    byte[] byteArray = Encoding.UTF8.GetBytes(sr.ReadToEnd());
                    byte[] NEWARR = Encoding.Convert(Encoding.UTF8, Encoding.Default, byteArray);
                    string finalString = Encoding.Default.GetString(NEWARR);
                    File.WriteAllText(tempreportpath + @"\" + files[i], finalString, Encoding.Default);
                    sr.Dispose();
                    File.Delete(Client.webconfigpath + @"\" + files[i]);
                }
                Thread.Sleep(1);
            }
        }

        private Queue<Item> GetItemsByfile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            string rss;
            JObject _items;
            Queue<Item> _items_l=new Queue<Item>();
            rss= Encoding.Default.GetString(byData);
            fs.Close();

            _items = GetjobjectByfile(file);

            foreach (var x in _items)
            {
                Item item = new Item(JObject.Parse(x.Value.ToString()));
                _items_l.Enqueue(item);
            }





            return _items_l;
        }
        private Queue<Item> GetItemsByfile(string file,string key)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            string rss;
            JObject _items;
            Queue<Item> _items_l = new Queue<Item>();
            rss = Encoding.Default.GetString(byData);
            fs.Close();
            _items = GetjobjectByfile(file, key);


            foreach (var x in _items)
            {
                Item item = new Item(JObject.Parse(x.Value.ToString()));
                _items_l.Enqueue(item);
            }





            return _items_l;
        }

        private void Cleartemp()
        {
            while (true)
            {
                while (items_finished.Count > 0)
                {
                    Item item = items_finished.Pop();
                    for (int i = 0; i < item.ranges.Length; i++)
                    {
                        File.Delete(item.report.csvfullnames[i]);
                    }
                }
                Thread.Sleep(1);
            }

        }
        private JObject GetjobjectByfile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);

            string json;
            JObject rss = new JObject();
            json = Encoding.Default.GetString(byData);
            fs.Close();
            try
            {
                rss = JObject.Parse(json);
            }
            catch (Exception e)
            {
                string hh = e.ToString();
                rss = JObject.Parse(Encoding.Default.GetString(Convert.FromBase64String(json)));
            }
            return rss;

        }
        private JObject GetjobjectByfile(string file,string key)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);

            string json;
            JObject rss = new JObject();
            json = Encoding.Default.GetString(byData);
            json = Encoding.Default.GetString(Convert.FromBase64String(json));

            fs.Close();
            List<byte> outdatasl = new List<byte>();
            string codebook = key;
            for (int i = 0; i < json.Length / 2; i++)
            {
                string tempstr = "";
                tempstr = tempstr + Convert.ToString(codebook.IndexOf(json.Substring(i * 2, 1)), 2).PadLeft(4, (char)0x30) + Convert.ToString(codebook.IndexOf(json.Substring(i * 2 + 1, 1)), 2).PadLeft(4, (char)0x30);
                outdatasl.Add((byte)Convert.ToUInt32(tempstr, 2));
            }
            byte[] outdatasb = outdatasl.ToArray();

            rss = JObject.Parse(Encoding.Default.GetString(outdatasb));
            
            return rss;

        }
        public void ShowItems()
        {
            if (items == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(items.ToString());
            }
        }
        public void ShowReports()
        {
            if (reports == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(reports.ToString());
            }
        }
        public void ShowModules()
        {
            if (modules == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(modules.ToString());
            }
        }
        public void ShowALLDailyItemsSQLInfo()
        {
            Queue<Item> dailyitems = GetItemsByfile(dailyreportconfigfile, this.key);
            int num = 1;
            while (dailyitems.Count > 0)
            {
                Item item = dailyitems.Dequeue();
                Console.WriteLine("".PadLeft(10, (char)0x2d)+num+ "".PadLeft(10, (char)0x2d)+"项目名:"+item.name+"项目信息如下:");
                for(int i = 0; i < item.ranges.Length; i++)
                {
                    Console.WriteLine("range" + (i + 1).ToString() + "sql:" + item.ranges[i].sqlstring);
                }
                num++;
            }



        }
        public void WriteReportidANDNameToWeb()
        {
            JObject job = new JObject();
            foreach(var x in reports)
            {
                job.Add(new JProperty(x.Value["name"].ToString(), x.Key.ToString()));
            }
            StreamWriter  sw = new StreamWriter(webconfigpath + @"\reportsinfo\infos",false,Encoding.UTF8);
            sw.Write(job.ToString());
            sw.Close();

        }
        public void ShowTempItems()
        {
            FileInfo[] files;
            files = new DirectoryInfo(Client.webconfigpath).GetFiles("*.json");
            for (int i = 0; i < files.Length; i++)
            {
                StreamReader sr = new StreamReader(Client.webconfigpath + @"\" + files[i], Encoding.UTF8, false);
                byte[] byteArray = Encoding.UTF8.GetBytes(sr.ReadToEnd());
                byte[] NEWARR = Encoding.Convert(Encoding.UTF8, Encoding.Default, byteArray);
                string finalString = Encoding.Default.GetString(NEWARR);
                File.WriteAllText(tempreportpath + @"\" + files[i], finalString, Encoding.Default);
                sr.Dispose();
                File.Delete(Client.webconfigpath + @"\" + files[i]);
            }

            files = new DirectoryInfo(tempreportpath).GetFiles("*.json");
            for (int i = 0; i < files.Length; i++)
            {
                Queue<Item> items = GetItemsByfile(tempreportpath + @"\" + files[i].ToString());
                while (items.Count > 0)
                {
                    Item tempitem = items.Dequeue();
                    Console.WriteLine(tempitem.name);                    

                }
            }

        }
        public  void ShowALLDailyItemsSQLInfo(string itemid)
        {
            Item item = new Item(itemid);
            Console.WriteLine("".PadLeft(10, (char)0x2d) + "当前项目" + "".PadLeft(10, (char)0x2d) + "项目名:" + item.name + "项目信息如下:");
            for (int i = 0; i < item.ranges.Length; i++)
            {
                Console.WriteLine("range" + (i + 1).ToString() + "sql:" + item.ranges[i].sqlstring);
            }

        }

    }
}
