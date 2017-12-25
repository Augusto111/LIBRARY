using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NetWorkApp
{
	class ProtocolHelper
	{
		private XmlNode fileNode;
		private XmlNode root;

		public ProtocolHelper(string protocol)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(protocol);
			root = doc.DocumentElement;
			fileNode = root.SelectSingleNode("file");
		}

		// 此时的protocal一定为单条完整protocal
		private RequestMode GetFileMode()
		{
			string mode = fileNode.Attributes["mode"].Value;
			mode = mode.ToLower();
			
				return RequestMode.UserLogin;
		}

		// 获取单条协议包含的信息
		public FileProtocol GetProtocol()
		{
			RequestMode mode = GetFileMode();
			int port = 0;
			string userName = "";
			string userPassword = "";

			mode = (RequestMode)Enum.Parse(typeof(RequestMode), fileNode.Attributes["mode"].Value, false);
			port = Convert.ToInt32(fileNode.Attributes["port"].Value);
			userName = fileNode.Attributes["userName"].Value;
			userPassword = fileNode.Attributes["userPassword"].Value;


			return new FileProtocol(mode, port, userName, userPassword);
		}
	}
}
