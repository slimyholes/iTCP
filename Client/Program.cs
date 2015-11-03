using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTCP.Core;
using System.IO;

namespace Client
{
    class Program
    {
        public static NetConnection ClientHandler = new NetConnection();

        public static bool messageMode = false;

        static void Main(string[] args)
        {
            ClientHandler.OnConnect += OnConnect;
            ClientHandler.OnDataReceived += OnDataReceived;
            ClientHandler.OnDisconnect += OnDisconnect;

            Console.WriteLine("p.stH iTCP [Version 3.10a]\n(c) 2015 p.stH. All rights reserved.\n\nType 'help' for a list of possible commands.\n");

            while (ClientHandler.client == null)
            {
                cmdMode();
            }
        }

        static void msgMode()
        {
            Console.Write("MSG >> ");
            string input = Console.ReadLine();
            string msg = "cMsg<>" + input;
            if (input == "cmd")
            {
                messageMode = false;
            }
            else
            {
                ClientHandler.Send(Encoding.UTF8.GetBytes(msg));
            }
        }

        static void cmdMode()
        {
            Console.Write("CMD >> ");
            string input = Console.ReadLine();
            string[] cArgs = input.Split();
            switch (cArgs[0])
            {
                case "msg":
                    if (ClientHandler.client != null)
                    {
                        string inputS1 = cArgs[0];
                        string msg1 = "cCmd<>" + inputS1;
                        //ClientHandler.Send(Encoding.UTF8.GetBytes(msg1));
                        messageMode = true;
                    }
                    else
                    {
                        Console.WriteLine("You aren't connected. Can't switch to message mode.\n");
                    }
                    break;
                case "help":
                    string inputS2 = cArgs[0];
                    string msg2 = "cCmd<>" + inputS2;
                    //ClientHandler.Send(Encoding.UTF8.GetBytes(msg2));
                    Console.WriteLine("regu (ip) (port) (name) (pass)     :     Registers you at the given address.");
                    Console.WriteLine("conn (ip) (port) (name) (pass)     :     Connects to the given address");
                    Console.WriteLine("dcon                               :     Disconnects the current connection\n");
                    break;
                case "conn":
                    Connect(input);
                    break;
                default:
                    Console.WriteLine("Invalid command. Type 'help' for a list of commands.\n");
                    break;
            }
        }

        static void Connect(string input)
        {
            try
            {
                string pInput = input.Split()[1];

                int IPStart = 0;
                int IPEnd = pInput.IndexOf(':');
                int PortStart = pInput.IndexOf(':') + 1;
                int PortEnd = pInput.Length - PortStart;

                string IP = pInput.Substring(IPStart, IPEnd);
                string Port = pInput.Substring(PortStart, PortEnd);

                try
                {

                    ClientHandler.Connect(IP, Int32.Parse(Port));
                    if (ClientHandler.client != null)
                        Console.WriteLine("Connected to " + IP + ":" + Port + "!\n");
                    if (ClientHandler.client == null)
                        Console.WriteLine("Connection to " + IP + ":" + Port + "has failed!\n");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("The correct syntax is 'conn (ip):(port)'.\n");
                    ClientHandler.client = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occurred: " + ex.Message + "\n");
            }
        }

        static void OnConnect(object sender, NetConnection connection)
        {
            Console.WriteLine("Succesfully connected to " + connection.RemoteEndPoint.ToString());
            while (true)
            {
                if (messageMode)
                {
                    msgMode();
                }
                else if(!messageMode)
                {
                    cmdMode();
                }
            }
        }

        private static void OnDataReceived(object sender, NetConnection connection, byte[] e)
        {
            switch(Encoding.UTF8.GetString(e))
            {
                case "sReg<>":
                    Console.Write("Username: ");
                    string i = Console.ReadLine();
                    Console.Write("Password: ");
                    string j = Console.ReadLine();
                    ClientHandler.Send(Encoding.UTF8.GetBytes("cReg<>" + i + " " + j));
                    break;
            }
        }

        private static void OnDisconnect(object sender, NetConnection connection)
        {
            ClientHandler.client = null;
            string[] i = { "notNull" };
            Main(i);
        }
    }
}