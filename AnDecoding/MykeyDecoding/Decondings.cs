using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MykeyDecoding
{
    public class Decondings
    {
        public static string Decongding(string str1)
        {
            string res="";
            Queue<string> answer_bin = new Queue<string>();
            Queue<byte> answer_byt = new Queue<byte>();
            for(int i = 0; i < (int)str1.Length / 2; i++)
            {
                answer_bin.Enqueue(Convert.ToString(Convert.ToUInt32(str1.Substring(i * 2, 2), 16), 2).PadLeft(8,(char)0x30));
            }
            string[] tempstr = new string[8];

            while (answer_bin.Count > 0)
            {
                string _data = answer_bin.Dequeue();
                for (int i = 0; i < 8; i++)
                {
                    tempstr[i] = tempstr[i] + _data.Substring(i, 1);
                }
                if (tempstr[0].Length == 8)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        answer_byt.Enqueue((byte) Convert.ToUInt32( tempstr[i],2));
                    }
                    tempstr = new string[8];
                }
            }
            res = Encoding.Default.GetString(answer_byt.ToArray());
            return res;
        }
    }
}
