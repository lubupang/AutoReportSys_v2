using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using MySql.Data;
using System.IO;
using System.Threading;

namespace Autoreport_v2
{
    public class CreateCsvClient
    {
        public static Stack<Item> items = new Stack<Item>();
        private string ip;
        private int port = 0;
        private string user = "";
        private string password = "";
        private string mysqlip;
        private uint mysqlport;
        private string mysqlpassword;
        private string mysqluser;
        private string mysqlconnectstr;
        private static string myoutpath;
        private static Queue<Thread> threads = new Queue<Thread>();
        private static Queue<Item> subitems = new Queue<Item>();
        public CreateCsvClient(string ip, int port, string user, string password, string mysqlip, uint mysqlport, string mysqluser, string mysqlpassword, string path)
        {
            this.ip = ip;
            this.port = port;
            this.user = user;
            this.password = password;
            this.mysqlip = mysqlip;
            this.mysqlport = mysqlport;
            this.mysqluser = mysqluser;
            this.mysqlpassword = mysqlpassword;
            CreateCsvClient.myoutpath = path;
        }
        public CreateCsvClient(string mysqlip, uint mysqlport, string mysqluser, string mysqlpassword, string path)
        {
            this.mysqlip = mysqlip;
            this.mysqlport = mysqlport;
            this.mysqluser = mysqluser;
            this.mysqlpassword = mysqlpassword;
            CreateCsvClient.myoutpath = path;
        }
        public void SshConnect()
        {
            if (ip != "" && port != 0 && user != "" && password != "")
            {
                PasswordConnectionInfo sshinfo = new PasswordConnectionInfo(ip, port, user, password);
                sshinfo.Timeout = TimeSpan.FromSeconds(30);
                SshClient client = new SshClient(sshinfo);
                client.Connect();
                Console.WriteLine(sshinfo.ClientVersion);
                if (!client.IsConnected)
                {
                    Console.WriteLine("SSH connect failed");
                }
                else
                {
                    var portfwd = new ForwardedPortLocal("127.0.0.1", 51741, mysqlip, mysqlport);
                    client.AddForwardedPort(portfwd);
                    portfwd.Start();
                    if (!client.IsConnected)
                    {
                        Console.WriteLine("port mapping failed");
                    }
                    else { mysqlconnectstr = "server=127.0.0.1;port=51741;user=" + mysqluser + ";password=" + mysqlpassword + ";Sslmode=none;"; }
                }
            }
            else { throw new Exception("Without Ssh Connect Info"); }
        }
        public void ConnectWithoutSsh()
        {
            mysqlconnectstr = "server=" + mysqlip + ";port=" + mysqlport + ";user=" + mysqluser + ";password=" + mysqlpassword + ";Sslmode=none;";
        }
        public void Start()
        {

            int threadsalive = threads.Count;
            while (true)
            {

                if (items.Count > 0 && threadsalive <= 20)
                {
                    Item item = items.Pop();
                    subitems.Enqueue(item.Copy());
                    Thread td = new Thread(new ParameterizedThreadStart(Getdataforthread));
                    td.Start(item);
                    threads.Enqueue(td);
                }
                RemoveDeadThreads();
                threadsalive = threads.Count;
                Thread.Sleep(1);
            }



        }
        private void Getdataforthread(object item)
        {
            Item myitem = (Item)item;
            Getdata(myitem);

        }

        private void Getdata(Item item)
        {
            for (int i = 0; i < item.ranges.Length; i++)
            {
                StringWriter sw = new StringWriter();
                MySql.Data.MySqlClient.MySqlConnection cnn = new MySql.Data.MySqlClient.MySqlConnection(mysqlconnectstr);
                while (!item.ranges[i].successconnect)
                {
                    try
                    {
                        cnn.Open();
                        item.ranges[i].successconnect = true;
                        Console.WriteLine(DateTime.Now + ":连接数据库成功 ");
                        Client.sw.WriteLine(DateTime.Now + ":连接数据库成功");

                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        Console.WriteLine(DateTime.Now + ":连接数据库,失败,失败内容:" + e.ToString());
                        Client.sw.WriteLine(DateTime.Now + ":连接数据库,失败,失败内容:" + e.ToString());
                        item.ranges[i].successconnect = false;
                    }
                }

                try
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(item.ranges[i].sqlstring, cnn);
                    Console.WriteLine(DateTime.Now + ":项目" + item.name + "区域" + item.ranges[i].rangename + "查询开始,查询内容:" + item.ranges[i].sqlstring);
                    Client.sw.WriteLine(DateTime.Now + ":项目" + item.name + "区域" + item.ranges[i].rangename + "查询开始,查询内容" + item.ranges[i].sqlstring);

                    MySql.Data.MySqlClient.MySqlDataReader reader = cmd.ExecuteReader();
                    int fieldscont = reader.FieldCount;
                    while (reader.Read())
                    {
                        string str = "";
                        for (int ii = 0; ii < fieldscont; ii++) { str=str.Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace("\r\n", ""); str = str + reader[ii].ToString().Replace(",", "，") + ","; }
                        str = str.Substring(0, str.Length - 1);
                        lock (sw)
                        {
                            sw.WriteLine(str);
                        }
                    }

                    item.report.csvfullnames[i] = myoutpath + @"\" + item.report._ranges[i].csvname + ".csv";
                    int kk = 1;
                    while (File.Exists(item.report.csvfullnames[i]))
                    {
                        item.report.csvfullnames[i] = item.report.csvfullnames[i] + "(" + kk + ")" + ".csv";
                        kk++;
                    }
                    File.WriteAllText(item.report.csvfullnames[i], sw.ToString(), Encoding.UTF8);
                    Console.WriteLine(DateTime.Now + ":项目" + item.name + "区域" + item.ranges[i].rangename + "查询成功CSV已生成");
                    Client.sw.WriteLine(DateTime.Now + ":项目" + item.name + "区域" + item.ranges[i].rangename + "查询成功CSV已生成");
                    try
                    {
                        cnn.Close();
                        Console.WriteLine(DateTime.Now + ":连接成功关闭 ");
                        Client.sw.WriteLine(DateTime.Now + ":连接成功关闭");
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        Console.WriteLine(DateTime.Now + ":连接关闭失败,失败内容:" + e.ToString());
                        Client.sw.WriteLine(DateTime.Now + ":连接关闭失败,失败内容:" + e.ToString());

                    }

                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    Console.WriteLine(DateTime.Now + ":查询失败,失败内容:" + e.ToString() + "查询内容:" + item.ranges[i].sqlstring);

                    Client.sw.WriteLine(DateTime.Now + ":查询失败,失败内容:" + e.ToString() + "查询内容:" + item.ranges[i].sqlstring);
                }


            }
        }
        private void RemoveDeadThreads()
        {
            lock (threads)
            {
                while (threads.Count > 0)
                {
                    Thread td = threads.Dequeue();
                    Item item = subitems.Dequeue();
                    Boolean isalive = td.IsAlive;
                    if (isalive) { threads.Enqueue(td); subitems.Enqueue(item); }
                    else
                    {
                        Report report = item.report;
                        lock (ReportClient.reports)
                        {
                            ReportClient.reports.Push(item.report);
                        }
                        Console.WriteLine(DateTime.Now + ":项目" + item.name + "csv已全部生成等待合成报表");
                        Client.sw.WriteLine(DateTime.Now + ":项目" + item.name + "csv已全部生成等待合成报表");


                    }
                }
            }
        }



    }
}
