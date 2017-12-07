using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LibrarySystemBackEnd
{
	static class SQL
	{
		private static string configFile = @"config.xml";
		private static XmlNode sqlNode;
		private static XmlNode root;
		private static string sqlName;
		private static string loginName;
		private static string loginPassword;
		private static Sq
		
		static SQL()
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(configFile);
			root = doc.DocumentElement;
			sqlNode = root.SelectSingleNode("sqlconfig");
			sqlName = sqlNode.SelectSingleNode("sqlname").Value;
			loginName = sqlNode.SelectSingleNode("loginname").Value;
			loginPassword = sqlNode.SelectSingleNode("loginpassword").Value;

		}
		public static void Print()
		{
			Console.WriteLine(sqlName + "\n" + loginName + "\n" + loginPassword);
		}

		static DataSet Query
	}
}
