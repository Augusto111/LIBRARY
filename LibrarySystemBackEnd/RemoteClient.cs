using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	class RemoteClient
	{
		private TcpClient client;
		private NetworkStream streamToClient;
		private const int BufferSize = 8192;
		private byte[] buffer;
		private ProtocolHandler handler;
		private int port = 6000;

		public RemoteClient(TcpClient client)
		{
			this.client = client;

			Console.WriteLine("\nClient Connected ! {0} <-- {1}",
				client.Client.LocalEndPoint, client.Client.RemoteEndPoint);

			streamToClient = client.GetStream();
			buffer = new byte[BufferSize];

			handler = new ProtocolHandler();
		}

		public void BeginRead()
		{
			AsyncCallback callBack = new AsyncCallback(OnReadComplete);
			streamToClient.BeginRead(buffer, 0, BufferSize, callBack, null);

		}

		private void OnReadComplete(IAsyncResult ar)
		{
			int bytesRead = 0;
			try
			{
				lock (streamToClient)
				{
					bytesRead = streamToClient.EndRead(ar);
					Console.WriteLine("Reading Data, {0} bytes", bytesRead);
				}
				if (bytesRead == 0) return;
				string msg = Encoding.Unicode.GetString(buffer, 0, bytesRead);
				Array.Clear(buffer, 0, buffer.Length);

				string[] protocolArray = handler.GetProtocol(msg);

				foreach (string pro in protocolArray)
				{
					Thread thr = new Thread(handleProtocol);
					thr.Start(pro);
				}

				lock (streamToClient)
				{
					AsyncCallback callback = new AsyncCallback(OnReadComplete);
					streamToClient.BeginRead(buffer, 0, BufferSize, callback, null);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				if (streamToClient != null) streamToClient.Dispose();
				client.Close();
			}
		}

		private void handleProtocol(object obj)
		{
			string pro = obj as string;
			ProtocolHelper helper = new ProtocolHelper(pro);
			FileProtocol protocol = helper.GetProtocol();

			if (protocol.Mode == RequestMode.UserLogin)
			{
				ClassBackEnd bk = new ClassBackEnd();

				int res = bk.Login(protocol.Userinfo.UserId, protocol.Userinfo.UserPassword);

				protocol.Retval = res;
				Thread.Sleep(3000);
			}
			else if (protocol.Mode == RequestMode.UserRegist)
			{
				ClassBackEnd bk = new ClassBackEnd();

				bool res = bk.RegisterUser(protocol.Userinfo.UserId, protocol.Userinfo.UserName, protocol.Userinfo.UserPassword, protocol.Userinfo.UserSchool, protocol.Userinfo.UserType);

				protocol.Retval = Convert.ToInt32(res);

				Thread.Sleep(3000);
			}
			else if (protocol.Mode == RequestMode.UserSearchBook)
			{
				ClassBackEnd bk = new ClassBackEnd();
				int k = 0;
				protocol.Resbook = bk.SearchBook(protocol.SearchCat, protocol.SearchWords, protocol.CurNum, ref k);
				protocol.EndNum = k;

				Thread.Sleep(3000);
			}
			else if (protocol.Mode == RequestMode.UserBookDetailLoad)
			{
				ClassBackEnd bk = new ClassBackEnd();
				protocol.NowBook = bk.GetBookDetail(protocol.NowBook.BookIsbn);
				Thread.Sleep(3000);
			}
			else if (protocol.Mode == RequestMode.UserBookStateLoad)
			{
				int retval = 0;
				ClassBackEnd bk = new ClassBackEnd();
				protocol.Bks = bk.GetBookState(protocol.NowBook.BookIsbn, protocol.Userinfo.UserId, ref retval);
				protocol.Retval = retval;
				Thread.Sleep(3000);
			}
			else if (protocol.Mode == RequestMode.UserBookCommentLoad)
			{
				ClassBackEnd bk = new ClassBackEnd();
				int linenum = 0;
				protocol.Comments = bk.GetComment(protocol.NowBook.BookIsbn, protocol.CurNum, ref linenum);
				protocol.EndNum = linenum;
				Thread.Sleep(500);
			}
			else if (protocol.Mode == RequestMode.UserBookLoad)
			{
				ClassBackEnd bk = new ClassBackEnd();
				protocol.NowBook = bk.GetBookDetail(protocol.NowBook.BookIsbn);

				int retval = 0;
				protocol.Bks = bk.GetBookState(protocol.NowBook.BookIsbn, protocol.Userinfo.UserId, ref retval);
				protocol.Retval = retval;
				//Thread.Sleep(3000);
			}
			else if (protocol.Mode == RequestMode.PicSend)
			{
				ClassBackEnd bk = new ClassBackEnd();
				string bookIsbn = protocol.NowBook.BookIsbn;
				byte[] pic = bk.GetBookPic(bookIsbn);
				try
				{
					lock (streamToClient)
					{
						streamToClient.Write(pic, 0, pic.Length);
					}
					Console.WriteLine("Sent: lenth{0}", pic.Length);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					return;
				}
				finally
				{
					streamToClient.Dispose();
					client.Close();
				}
				return;
			}
			else if (protocol.Mode == RequestMode.UserCommentBook)
			{
				ClassBackEnd bk = new ClassBackEnd();
				protocol.Retval = Convert.ToInt32(bk.AddComment(protocol.NowComment.CommentIsbn, protocol.NowComment.UserId, protocol.NowComment.Text));
			}
			else if (protocol.Mode == RequestMode.UserDelComment)
			{
				ClassBackEnd bk = new ClassBackEnd();
				protocol.Retval = Convert.ToInt32(bk.DelComment(protocol.NowComment.CommentIsbn));
			}
			else if (protocol.Mode == RequestMode.UserBorrowBook)
			{
				ClassBackEnd bk = new ClassBackEnd();
				protocol.Retval = Convert.ToInt32(bk.BorrowBook(protocol.Userinfo.UserId, protocol.Userinfo.UserPassword, protocol.NowBook.BookIsbn));
			}
			else if (protocol.Mode == RequestMode.UserOrderBook)
			{

			}

			SendMessage(protocol.ToString());
		}

		public void SendMessage(string msg)
		{
			try
			{
				//msg = String.Format("[length={0}]{1}", msg.Length, msg);

				byte[] tmp = Encoding.Unicode.GetBytes(msg);

				lock (streamToClient)
				{
					streamToClient.Write(tmp, 0, tmp.Length);
				}
				Console.WriteLine("Sent: {0}", msg);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if(client.Client.Connected)
					Console.WriteLine("Closed {0}<--{1}", client.Client.LocalEndPoint, client.Client.RemoteEndPoint);
				streamToClient.Close();
				client.Close();
			}
		}

		//private void SendFile(byte[] file)
		//{
		//	TcpListener listener = new TcpListener(IPAddress.Parse("0.0.0.0"), port + 1);
		//	listener.Start();

		//	IPEndPoint endpoint = listener.LocalEndpoint as IPEndPoint;
		//	int listeningPort = endpoint.Port;

		//	MD5 md5 = MD5.Create();
		//	byte[] data = md5.ComputeHash(file);

		//	// 创建一个 Stringbuilder 来收集字节并创建字符串  
		//	StringBuilder sBuilder = new StringBuilder();

		//	// 循环遍历哈希数据的每一个字节并格式化为十六进制字符串  
		//	for (int i = 0; i < data.Length; i++)
		//	{
		//		sBuilder.Append(data[i].ToString("x2"));
		//	}
		//	// 返回十六进制字符串
		//	string fileName = sBuilder.ToString();

		//	FileProtocol protocol = new FileProtocol(RequestMode.PicReceive, listeningPort);
		//	protocol.FileName = fileName;

		//	string pro = protocol.ToString();

		//	SendMessage(pro);

		//	TcpClient localClient = listener.AcceptTcpClient();
		//	Console.WriteLine("Start sending file...");
		//	NetworkStream stream = localClient.GetStream();

		//	FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
		//	byte[] fileBuffer = new byte[1024];
		//	int bytesRead;
		//	int totalBytes = 0;

		//	SendStatus status = new SendStatus(fileName);
		//	try
		//	{
		//		do
		//		{
		//			Thread.Sleep(10);
		//			bytesRead = fs.Read(fileBuffer, 0, fileBuffer.Length);
		//			stream.Write(fileBuffer, 0, bytesRead);
		//			totalBytes += bytesRead;
		//			status.PrintStatus(totalBytes);
		//		} while (bytesRead > 0);
		//		Console.WriteLine("Total {0} bytes sent, Done!", totalBytes);
		//	}
		//	catch (Exception)
		//	{
		//		Console.WriteLine("Server has lost...");
		//	}
		//	finally
		//	{
		//		stream.Dispose();
		//		fs.Dispose();
		//		localClient.Close();
		//		listener.Stop();
		//	}
		//}

		//private void BeginSendFile(object obj)
		//{
		//	byte[] file = obj as byte[];
		//	SendFile(file);
		//}
		//public void BeginSendFile(byte[] file)
		//{
		//	Thread thr = new Thread(BeginSendFile);
		//	thr.Start(file);
		//}
	}
}
