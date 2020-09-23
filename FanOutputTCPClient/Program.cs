using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using FanOutputModelLib;
using Newtonsoft.Json;

namespace FanOutputTCPClient
{
    class Program
    {
        static void Main()
        {
            string userData = string.Empty;

            Console.WriteLine("Options: get, getbyid, post");
            Console.WriteLine("Input action:");
            string method = Console.ReadLine();

            switch (method.ToLower())
            {
                case "get":
                    break;
                case "getbyid":
                    Console.WriteLine("Input id:");
                    userData = Console.ReadLine();
                    break;
                case "post":
                    Console.WriteLine("Input data (Id Name Temp Humidity):");
                    userData = Console.ReadLine();
                    break;
                default:
                    Main();
                    break;
            }

            Client.Start(method, userData);
        }
    }

    class Client
    {
        public static void Start(string method, string userData)
        {
            try
            {
                Int32 port = 4646;
                TcpClient socket = new TcpClient("localhost", port);
                NetworkStream ns = socket.GetStream();

                StreamWriter sw = new StreamWriter(ns);
                sw.AutoFlush = true;
                sw.WriteLine(method);
                sw.WriteLine(userData);
                Console.WriteLine("Sent: {0}, {1}", method, userData);


                String responseData = String.Empty;
                StreamReader sr = new StreamReader(ns);
                responseData = sr.ReadLine();
                Console.WriteLine("Received: {0}", responseData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
