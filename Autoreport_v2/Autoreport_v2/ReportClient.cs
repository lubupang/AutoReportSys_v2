using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;


namespace Autoreport_v2
{
    public class ReportClient
    {
        public static Stack<Report> reports = new Stack<Report>();
        private static string myoutpath;
        private static readonly Excel.Application excelapp = new Excel.Application();
        public ReportClient(string path)
        {
            ReportClient.myoutpath = path;
        }
        public void Start()
        {
            excelapp.Visible = false;
            excelapp.DisplayAlerts = false;

            while (true)
            {
                if (reports.Count > 0)
                {
                    Report report = reports.Pop();
                    Console.WriteLine(DateTime.Now + ":项目" + report.item.name + "的报表开始生成");
                    Client.sw.WriteLine(DateTime.Now + ":项目" + report.item.name + "的报表开始生成");

                    Excel.Workbook excelreport = excelapp.Workbooks.Open(report.module.file);
                    int k = report.ranges_count;
                    for (int i = 0; i < k; i++)
                    {
                        Excel.Workbook csv;
                        Boolean csvfinished = false;
                        while (!File.Exists(report.csvfullnames[i])) { Thread.Sleep(1); }
                        while (!csvfinished)
                        {
                            int times = 1;
                            try
                            {


                                csv = excelapp.Workbooks.Open(report.csvfullnames[i]);
                                csv.Sheets[1].UsedRange.Copy(excelreport.Sheets[report.sheets[i]].Cells[report.startrows[i], report.startcols[i]]);
                                csv.Close(false);
                                csvfinished = true;
                                Console.WriteLine(DateTime.Now + ":项目 " + report.item.name + " 的报表:" + report.file + " 的CSV" + report.csvfullnames[i] + "第" + times + "次打开成功");
                                Client.sw.WriteLine(DateTime.Now + ":项目 " + report.item.name + " 的报表:" + report.file + " 的CSV" + report.csvfullnames[i] + "第" + times + "次打开成功");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(DateTime.Now + ":项目 " + report.item.name + " 的报表:" + report.file + " 的CSV" + report.csvfullnames[i] + "第" + times + "次打开失败;稍后进行" + (times + 1) + "次打开");

                                Console.WriteLine("失败原因：" + e.ToString());
                                Client.sw.WriteLine(DateTime.Now + ":项目 " + report.item.name + " 的报表:" + report.file + " 的CSV" + report.csvfullnames[i] + "第" + times + "次打开失败;稍后进行" + (times + 1) + "次打开");
                                Client.sw.WriteLine("失败原因：" + e.ToString());
                                times++;

                            }
                        }
                    }
                    report.file = myoutpath + @"\" + report.item.name + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsm";
                    int kk = 1;
                    while (File.Exists(report.file))
                    {
                        report.file = report.file.Split('.')[0] + "(" + kk + ").xlsm";
                    }
                    excelapp.Calculate();
                    excelreport.SaveAs(report.file);
                    excelreport.Close(true);
                    string emailtxt = "Dear " + report.item.branding + ":\r\n你的报表：" + report.item.name + "在附件请查阅";
                    Email email = new Email(report, emailtxt);
                    EmailClient.emails.Push(email);
                    Client.items_finished.Push(report.item);
                    Console.WriteLine(DateTime.Now + ":项目" + report.item.name + "的报表:" + report.file + "已经生成等待发送邮件");
                    Client.sw.WriteLine(DateTime.Now + ":项目" + report.item.name + "的报表:" + report.file + "已经生成等待发送邮件");
                }
                Thread.Sleep(1);
            }
        }



    }
}
