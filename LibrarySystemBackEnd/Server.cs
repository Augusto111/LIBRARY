using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LibrarySystemBackEnd
{
	class Server
	{
		public delegate void ParameterizedThreadStart(object obj);
		const int bufferSize = 8192;
		static void Main(string[] args)
		{
			//ClassBackEnd nk = new ClassBackEnd();
			//Console.WriteLine(nk.AddComment("2332332332100", "2015211254", "Test!"));
			//do
			//{
			//	Console.ReadKey();
			//} while (true);
			TcpListener server = new TcpListener(IPAddress.Parse("0.0.0.0"), 6000);
			server.Start();
			while (true)
			{
				Console.WriteLine("Waiting for client...");
				TcpClient remoteClient = server.AcceptTcpClient();

				RemoteClient wapper = new RemoteClient(remoteClient);

				wapper.BeginRead();
			}
		}
	}
}
