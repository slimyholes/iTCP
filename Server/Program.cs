using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTCP.Core;
using System.IO;
using System.Windows.Threading;

namespace Server
{
    class Program
    {
        public static List<int> ID = new List<int>();
        public static List<string> IP = new List<string>();
        public static List<string> Pass = new List<string>();
        public static List<string> Name = new List<string>();

        public static NetConnection ServerHandler = new NetConnection();

        static void Main(string[] args)
        {

            ServerHandler.OnConnect += OnConnect;
            ServerHandler.OnDataReceived += OnDataReceived;
            ServerHandler.OnDisconnect += OnDisconnect;

            Console.Write("PORT: ");
            string sPort = Console.ReadLine();
            int port = Int32.Parse(sPort);

            ServerHandler.Start(port);

            DispatcherTimer dT = new DispatcherTimer();
            dT.Tick += DT_Tick;
            dT.Interval = new TimeSpan(0, 0, 0, 20);
            dT.Start();

            while (true);
        }

        private static void DT_Tick(object sender, EventArgs e)
        {
            Data.Store();
        }

        static void OnConnect(object sender, NetConnection connection)
        {
            string folder = string.Format("{0}", connection.RemoteEndPoint.ToString().Replace(".", ""));
            string file = string.Format("{0}/user.dat", connection.RemoteEndPoint.ToString().Replace(".", ""));
            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Connection from " + connection.RemoteEndPoint);
            if(Directory.Exists(folder) && File.Exists(file))
            {
                using (StreamReader sr = new StreamReader(string.Format("{0}/user.dat", connection.RemoteEndPoint.ToString().Replace(".", ""))));
                {

                }
            }
            else
            {
                ServerHandler.Send(Encoding.UTF8.GetBytes("sReg<>"));                 
            }
        }

        private static void OnDataReceived(object sender, NetConnection connection, byte[] e)
        {
            if (Encoding.UTF8.GetString(e).Substring(0, 6) == "cMsg<>")
            { Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Message from " + connection.RemoteEndPoint + ": " + Encoding.UTF8.GetString(e)); Console.ForegroundColor = ConsoleColor.Gray; }
            if (Encoding.UTF8.GetString(e).Substring(0, 6) == "cCmd<>")
            { Console.ForegroundColor = ConsoleColor.Gray; Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] @ " + connection.RemoteEndPoint + " issued a command: " + Encoding.UTF8.GetString(e)); Console.ForegroundColor = ConsoleColor.Gray; }
            if (Encoding.UTF8.GetString(e).Substring(0, 6) == "cNam<>")
            { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] @ " + connection.RemoteEndPoint + " name was set to " + Encoding.UTF8.GetString(e)); Console.ForegroundColor = ConsoleColor.Gray; }
            if (Encoding.UTF8.GetString(e).Substring(0, 6) == "cPwd<>")
            { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] @ " + connection.RemoteEndPoint + " password set to [private]"); Console.ForegroundColor = ConsoleColor.Gray; }
            if (Encoding.UTF8.GetString(e).Substring(0, 6) == "cReg<>")
            { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] @ " + connection.RemoteEndPoint + " has registered!"); Console.ForegroundColor = ConsoleColor.Gray; }
            {
                using (StreamWriter sw = new StreamWriter(string.Format("file")))
                {
                    sw.WriteLine(connection.RemoteEndPoint);
                    string ip = connection.RemoteEndPoint.ToString();
                    int i = 0;
                    foreach (string ip2 in IP)
                    {
                        i++;
                        if (ip2 == ip)
                            break;
                    }
                    string[] eSplit = Encoding.UTF8.GetString(e).Split();
                    sw.WriteLine(eSplit[0]); //Name
                    sw.WriteLine(eSplit[1]); //Pass
                }
            }
        }

        private static void OnDisconnect(object sender, NetConnection connection)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Disconnection from " + connection.RemoteEndPoint);
        }
    }
}
