using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FanOutputModelLib;
using Newtonsoft.Json;

namespace FanOutputTCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start();
        }
    }

    class Server
    {
        static List<FanOutputModel> fanOutputReadings = new List<FanOutputModel>()
        {
            new FanOutputModel(1, "First Output", 15, 30),
            new FanOutputModel(2, "Second Output", 18, 41),
            new FanOutputModel(3, "Thrid Output", 24, 69),
            new FanOutputModel(4, "Fourth Output", 22, 49),
            new FanOutputModel(5, "Sixth Output", 20, 75),
        };

        public static void Start()
        {
            TcpListener server = null;
            int clientsConnected = 0;
            try
            {
                Int32 port = 4646;
                IPAddress localAddress = IPAddress.Loopback;

                server = new TcpListener(localAddress, port);

                server.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for a connection");

                    TcpClient socket = server.AcceptTcpClient();
                    clientsConnected++;
                    Console.WriteLine($"Client {clientsConnected} has connected to the server");
                    Task.Run(() => { DoClient(socket, $"Client {clientsConnected}"); });
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        private static void DoClient(TcpClient socket, string client)
        {
            try
            {
                while (socket.Connected)
                {
                    string method = string.Empty;
                    string data = string.Empty;

                    NetworkStream ns = socket.GetStream();

                    StreamReader sr = new StreamReader(ns);
                    method = sr.ReadLine();
                    data = sr.ReadLine();
                    Console.WriteLine($"Received from {client}: {method}");
                    Console.WriteLine($"Received from {client}: {data}");

                    data = CompleteUsersAction(method, data);

                    if (method.ToLower() != "post")
                    {
                        StreamWriter sw = new StreamWriter(ns);
                        sw.AutoFlush = true;
                        sw.WriteLine(data);
                        Console.WriteLine($"Sent to {client}: {data}");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"{client} has disconnected from the server");
                socket.Dispose();
                socket.Close();
            }
        }

        static string CompleteUsersAction(string method, string data)
        {
            if (method == null || data == null) {return "";}

            switch (method.ToLower())
            {
                case "get":
                    return ConvertObjectToJson(ref fanOutputReadings);
                case "getbyid":
                    int n;
                    if (int.TryParse(data, out n))
                    {
                        FanOutputModel fanOutputModel = fanOutputReadings.Find(i => i.Id == n);
                        return ConvertObjectToJson(ref fanOutputModel);
                    }

                    return "'" + data + "' is not a number!";
                case "post":
                    fanOutputReadings.Add(ConvertStringToObject(data));
                    break;
            }
            return "";
        }

        static FanOutputModel ConvertStringToObject(string data)
        {
            string[] inputStrings = data.Split(" ");
            return new FanOutputModel(Int32.Parse(inputStrings[0]), inputStrings[1], Int32.Parse(inputStrings[2]), Int32.Parse(inputStrings[3]));
        }

        static string ConvertObjectToJson<T>(ref T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
