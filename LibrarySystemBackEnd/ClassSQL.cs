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
	public class ClassSQL
	{
		private string configFile = @"config.xml";
		private XmlNode sqlNode;
		private XmlNode root;
		private string sqlName;
		private string loginName;
		private string loginPassword;
		private string initialCatalog;
		private SqlConnectionStringBuilder builder;

		public SqlConnectionStringBuilder Builder
		{
			get
			{
				return builder;
			}

			set
			{
				builder = value;
			}
		}

		public ClassSQL()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(configFile);
			root = doc.DocumentElement;
			sqlNode = root.SelectSingleNode("sqlconfig");
			sqlName = sqlNode.SelectSingleNode("sqlname").InnerText;
			loginName = sqlNode.SelectSingleNode("loginname").InnerText;
			loginPassword = sqlNode.SelectSingleNode("loginpassword").InnerText;
			initialCatalog = sqlNode.SelectSingleNode("initialcatalog").InnerText;
			Builder = new SqlConnectionStringBuilder();
			Builder.DataSource = sqlName;
			Builder.UserID = loginName;
			Builder.Password = loginPassword;
			Builder.InitialCatalog = initialCatalog;
		}
		public void Print()
		{
			Console.WriteLine(sqlName + "\n" + loginName + "\n" + loginPassword);
		}

		public DataSet Query(string SQLstr, string tableName)
		{
			DataSet ds = new DataSet();
			using (SqlConnection con = new SqlConnection(Builder.ConnectionString))
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
		/// <returns>有满足的用户返回用户实例，没有返回null</returns>
		public ClassUserBasicInfo getUsersBasic(string userid)
		{
			ClassUserBasicInfo users = null;
			using (SqlConnection con = new SqlConnection(Builder.ConnectionString))
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
						users = new ClassUserBasicInfo(dr);
					}
				}
			}
			return users;
		}
		/// <summary>
		/// 精确查找管理员
		/// </summary>
		/// <param name="id">管理员id</param>
		/// <returns>管理员实例</returns>
		public ClassAdmin getAdmin(string id)
		{
			ClassAdmin admin = null;
			using (SqlConnection con = new SqlConnection(Builder.ConnectionString))
			{
				SqlCommand cmd = con.CreateCommand();
				cmd.CommandText = "select *from dt_Admin where id=@a";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@a", id);

				con.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					while (dr.Read())
					{
						admin = new ClassAdmin(dr);
					}
				}
			}
			return admin;
		}
		/// <summary>
		/// 向数据库添加用户
		/// </summary>
		/// <param name="user">待添加的用户</param>
		/// <returns>成功返回true，失败(学号已存在)返回false</returns>
		public bool AddUser(ClassUserBasicInfo user)
		{
			if (getUsersBasic(user.UserId) != null)
				return false;
			using (SqlConnection con = new SqlConnection(Builder.ConnectionString))
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
		/// <summary>
		/// 向数据库添加用户
		/// </summary>
		/// <param name="admin">待添加的管理员</param>
		/// <returns>成功返回true，失败(学号已存在)返回false</returns>
		public bool AddAdmin(ClassAdmin admin)
		{
			if (getAdmin(admin.Id) != null)
				return false;
			using (SqlConnection con = new SqlConnection(Builder.ConnectionString))
			{
				SqlCommand cmd = con.CreateCommand();
				cmd.CommandText = "insert into dt_Admin values(@a,@b,@c,@d,@e)";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@a", admin.Id);
				cmd.Parameters.AddWithValue("@b", admin.Name);
				cmd.Parameters.AddWithValue("@c", admin.Password);
				cmd.Parameters.AddWithValue("@d", admin.Type);
				cmd.Parameters.AddWithValue("@e", admin.RegisterDate);

				con.Open();
				int count = cmd.ExecuteNonQuery();
				if (count > 0)
					return true;
				else
					return false;
			}
		}
		/// <summary>
		/// 向数据库中添加书籍，采用数据库事物
		/// </summary>
		/// <param name="bk">书籍类</param>
		/// <returns>成功/失败</returns>
		public bool AddBook(ClassBook bk)
		{
			bool res = false;
			using (SqlConnection con = new SqlConnection(Builder.ConnectionString))
			{
				con.Open();
				SqlTransaction tra = null;
				try
				{
					tra = con.BeginTransaction();
					SqlCommand cmd = new SqlCommand();
					cmd.Transaction = tra;
					cmd.Connection = con;

					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "insert into dt_book values(@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k)";
					cmd.Parameters.Clear();
					cmd.Parameters.AddWithValue("@a", bk.BookName);
					cmd.Parameters.AddWithValue("@b", bk.BookIsbn);
					cmd.Parameters.AddWithValue("@c", bk.BookPublisher);
					cmd.Parameters.AddWithValue("@d", bk.BookAuthor);
					cmd.Parameters.AddWithValue("@e", bk.BookImage);
					cmd.Parameters.AddWithValue("@f", bk.BookIntroduction);
					cmd.Parameters.AddWithValue("@g", bk.BookPublishTime);
					cmd.Parameters.AddWithValue("@h", bk.BookAmount);
					cmd.Parameters.AddWithValue("@i", bk.BookLable1);
					cmd.Parameters.AddWithValue("@j", bk.BookLable2);
					cmd.Parameters.AddWithValue("@k", bk.BookLable3);

					if (cmd.ExecuteNonQuery() < 0)
						throw new Exception();
					foreach (ClassABook abk in bk.Book)
					{
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = "insert into dt_abook values(@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m)";
						cmd.Parameters.Clear();

						cmd.Parameters.AddWithValue("@a", abk.BookName);
						cmd.Parameters.AddWithValue("@b", abk.BookIsbn);
						cmd.Parameters.AddWithValue("@c", abk.BookPublisher);
						cmd.Parameters.AddWithValue("@d", abk.BookAuthor);
						cmd.Parameters.AddWithValue("@e", abk.BookImage);
						cmd.Parameters.AddWithValue("@f", abk.BookPublishDate);
						cmd.Parameters.AddWithValue("@g", abk.BookBroughtTime);
						cmd.Parameters.AddWithValue("@h", abk.BookState);
						cmd.Parameters.AddWithValue("@i", abk.BorrowUserId);
						cmd.Parameters.AddWithValue("@j", abk.BorrowTime);
						cmd.Parameters.AddWithValue("@k", abk.ReturnTime);
						cmd.Parameters.AddWithValue("@l", abk.Delayed);
						cmd.Parameters.AddWithValue("@m", abk.Deleted);

						if (cmd.ExecuteNonQuery() < 0)
							throw new Exception();
					}
					tra.Commit();
					res = true;
				}
				catch (Exception e)
				{
					res = false;
					tra.Rollback();
				}
			}
			return res;
		}
	}
}
