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
	public static class SQL
	{
		private static string configFile = @"config.xml";
		private static XmlNode sqlNode;
		private static XmlNode root;
		private static string sqlName;
		private static string loginName;
		private static string loginPassword;
		private static string initialCatalog;
		private static SqlConnectionStringBuilder builder;
		private static SqlConnection con;

		static SQL()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(configFile);
			root = doc.DocumentElement;
			sqlNode = root.SelectSingleNode("sqlconfig");
			sqlName = sqlNode.SelectSingleNode("sqlname").InnerText;
			loginName = sqlNode.SelectSingleNode("loginname").InnerText;
			loginPassword = sqlNode.SelectSingleNode("loginpassword").InnerText;
			initialCatalog = sqlNode.SelectSingleNode("initialcatalog").InnerText;
			builder = new SqlConnectionStringBuilder();
			builder.DataSource = sqlName;
			builder.UserID = loginName;
			builder.Password = loginPassword;
			builder.InitialCatalog = initialCatalog;
		}
		public static void Print()
		{
			Console.WriteLine(sqlName + "\n" + loginName + "\n" + loginPassword);
		}

		public static DataSet Query(string SQLstr, string tableName)
		{
			DataSet ds = new DataSet();
			using (SqlConnection con = new SqlConnection(builder.ConnectionString))
			{
				con.Open();
				SqlDataAdapter SQLda = new SqlDataAdapter(SQLstr, con);
				SQLda.Fill(ds, tableName);
			}
			return ds;
		}
		/// <summary>
		/// 在数据库中精确查找用户
		/// </summary>
		/// <param name="userid">待查找的用户id</param>
		/// <returns>有满足的用户返回list，没有返回null</returns>
		public static UserBasicInfo getUsersBasic(string userid)
		{
			UserBasicInfo users = null;
			using (SqlConnection con = new SqlConnection(builder.ConnectionString))
			{
				SqlCommand cmd = con.CreateCommand();
				cmd.CommandText = "select *from dt_UserBasic where userId=@a";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@a", userid);

				con.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					while (dr.Read())
					{
						UserBasicInfo user = new UserBasicInfo(dr);
					}
				}
			}
			return users;
		}
		/// <summary>
		/// 向数据库添加用户
		/// </summary>
		/// <param name="user">待添加的用户</param>
		/// <returns>成功返回true，失败(学号已存在)返回false</returns>
		public static bool AddUser(UserBasicInfo user)
		{
			if (getUsersBasic(user.UserId) == null)
				return false;
			using (SqlConnection con = new SqlConnection(builder.ConnectionString))
			{
				SqlCommand cmd = con.CreateCommand();
				cmd.CommandText = "insert into dt_UserBasic values(@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@a", user.UserId);
				cmd.Parameters.AddWithValue("@b", user.UserName);
				cmd.Parameters.AddWithValue("@c", user.UserPassword);
				cmd.Parameters.AddWithValue("@d", user.UserSchool);
				cmd.Parameters.AddWithValue("@e", user.UserType);
				cmd.Parameters.AddWithValue("@f", user.UserCurrentScheduleAmount);
				cmd.Parameters.AddWithValue("@g", user.UserMaxBorrowableAmount);
				cmd.Parameters.AddWithValue("@h", user.UserCurrentBorrowedAmount);
				cmd.Parameters.AddWithValue("@i", user.UserCurrentMaxBorrowableAmount);
				cmd.Parameters.AddWithValue("@j", user.UserCredit);
				cmd.Parameters.AddWithValue("@k", user.UserRegisterDate);

				con.Open();
				int count = cmd.ExecuteNonQuery();
				if (count > 0)
					return true;
				else
					return false;
			}
		}
	}
}
