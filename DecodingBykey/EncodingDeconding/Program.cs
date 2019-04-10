using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MykeyDecoding;

namespace EncodingDeconding
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string keyfile = args[0];
            string inputfile = args[1];
            MykeyDecoding.Decondings decoding = new Decondings();
            FileStream fs0 = new FileStream(keyfile, FileMode.Open);
            byte[] datas0 = new byte[fs0.Length];
            fs0.Read(datas0, 0, datas0.Length);
            


            string codebook =  Decondings.Decongding(Encoding.Default.GetString(datas0));
            FileStream fs = new FileStream(inputfile, FileMode.Open);
            byte[] datas = new byte[fs.Length];
            fs.Read(datas, 0, datas.Length);
            List<int> newdatas = new List<int>();
            for (int i = 0; i < datas.Length; i++)
            {
                string tempstr = "";
                tempstr = Convert.ToString(datas[i], 2).PadLeft(8, (char)0x30);
                for (int j = 0; j < 2; j++) { newdatas.Add(Convert.ToInt32(tempstr.Substring(j * 4, 4), 2)); }

            }
            string res = "";
            for (int i = 0; i < newdatas.Count(); i++)
            {
                res = res + codebook.Substring(newdatas[i], 1);
            }
            res = Convert.ToBase64String(Encoding.Default.GetBytes(res));
            Console.WriteLine(res);
            Console.ReadKey();
            res = Encoding.Default.GetString(Convert.FromBase64String(res));
            List<byte> outdatasl = new List<byte>();
            for (int i = 0; i < res.Length / 2; i++)
            {
                string tempstr = "";
                tempstr = tempstr + Convert.ToString(codebook.IndexOf(res.Substring(i * 2, 1)), 2).PadLeft(4, (char)0x30) + Convert.ToString(codebook.IndexOf(res.Substring(i * 2 + 1, 1)), 2).PadLeft(4, (char)0x30);
                outdatasl.Add((byte)Convert.ToUInt32(tempstr, 2));
            }
            byte[] outdatasb = outdatasl.ToArray();
            Console.WriteLine(Encoding.Default.GetString(outdatasb));
            Console.ReadKey();

        }
    }
}
