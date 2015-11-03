using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Data
    {
        public static List<int> uID = new List<int>();
        public static List<string> uIP = new List<string>();
        public static List<string> uName = new List<string>();
        public static List<string> uPass = new List<string>();
        public static List<int> uRank = new List<int>();

        public static void Store()
        {
            using (StreamWriter sw = new StreamWriter("users.dat"))
            {
                foreach (byte u in uID)
                {
                    sw.Write(uID[u].ToString() + ":" + uIP[u] + ":" + uName[u] + ":" + uPass[u] + ":" + uRank[u].ToString() + ";");
                }
            }
        }
    }
}