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
	class ServerMain
	{
		const int bufferSize = 8192;
		static void Main(string[] args)
		{
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
