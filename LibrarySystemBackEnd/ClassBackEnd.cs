using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	public class ClassBackEnd
	{
		private ClassSQL sql;

		#region 私有方法
		/// <summary>
		/// 在数据库中精确查找用户
		/// </summary>
		/// <param name="userid">待查找的用户id</param>
		/// <returns>有满足的用户返回用户实例，没有返回null</returns>
		private ClassUserBasicInfo getUsersBasic(string userid)
		{
			ClassUserBasicInfo users = null;
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
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
		private ClassAdmin getAdmin(string id)
		{
			ClassAdmin admin = null;
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
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
		private bool AddUser(ClassUserBasicInfo user)
		{
			if (getUsersBasic(user.UserId) != null)
				return false;
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
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
		private bool AddAdmin(ClassAdmin admin)
		{
			if (getAdmin(admin.Id) != null)
				return false;
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
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
		#endregion

		#region 公有方法
		/// <summary>
		/// 构造，初始化SQL连接
		/// </summary>
		public ClassBackEnd()
		{
			sql = new ClassSQL();
		}

		/// <summary>
		/// 用户登录函数，加载个人信息；管理员登录函数，加载用户信息
		/// </summary>
		/// <param name="id">用户id</param>
		/// <param name="password">密码</param>
		/// <returns>管理员登录成功返回2，用户登录成功返回1,失败返回0(用户名不存在，密码不正确)</returns>
		public int Login(string id, string password)
		{
			ClassUserBasicInfo user = getUsersBasic(id);
			ClassAdmin admin = getAdmin(id);
			if (user == null && admin == null) return 0;
			else if (user != null)
			{
				if (user.UserPassword == password)
					return 1;
				else return 0;
			}
			else if (admin != null)
			{
				if (admin.Password == password) return 2;
				else return 0;
			}
			return 0;
		}

		/// <summary>
		/// 注册用户函数，会检查id是否与现有用户重复
		/// </summary>
		/// <param name="userid">用户id</param>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="school">学院信息</param>
		/// <param name="usertype">用户种类,0学生,1老师,4访客</param>
		/// <returns>返回值：0id重复，1成功</returns>
		public bool RegisterUser(string userid, string username, string password, string school, USERTYPE usertype)
		{
			bool fl = AddUser(new ClassUserBasicInfo(userid, username, password, school, usertype));
			return fl;
		}

		/// <summary>
		/// 管理员注册
		/// </summary>
		/// <param name="id">管理员id</param>
		/// <param name="name">管理员姓名</param>
		/// <param name="password">管理员密码</param>
		/// <param name="type">2管理员,3书籍管理员</param>
		/// <returns>返回值：0id重复，1成功</returns>
		public bool RegisterAdmin(string id, string name, string password, USERTYPE type)
		{
			bool fl = AddAdmin(new ClassAdmin(id, name, password, type));
			return fl;
		}

		/// <summary>
		/// 借书
		/// </summary>
		/// <param name="userid">用户id</param>
		/// <param name="password">用户密码</param>
		/// <param name="bookid">书籍id</param>
		/// <returns>返回：0借书成功，1用户借书数量上限，2没有可借书籍，3其他错误</returns>
		public int BorrowBook(string userid, string password, string bookid)
		{
			int res = 0;
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
			{
				con.Open();
				SqlTransaction tra = null;
				try
				{
					tra = con.BeginTransaction();
					string sqlstr1 = "update dt_UserBasic set userCurrentBorrowedAmount=userCurrentBorrowedAmount+1 where userId='" + userid + "' and userPassword='" + password + "' and userCurrentBorrowedAmount<userCurrentMaxBorrowableAmount";
					SqlCommand cmd1 = new SqlCommand();
					cmd1.Connection = con;
					cmd1.Transaction = tra;
					cmd1.CommandText = sqlstr1;

					if (cmd1.ExecuteNonQuery() <= 0)
					{
						res = 1;//借阅数量上限
						throw new Exception();
					}

					string sqlstr2 = @"update dt_abook 
									set bookState = 1,
									borrowUserId = '" + userid +
									"', borrowTime = '" + DateTime.Now +
									@"', returnTime = '" + (DateTime.Now + ClassUserBasicInfo.DefaultDate) +
									"'where bookIsbn like '" + bookid +
									"%' and (bookState = 2 and borrowUserId = '" + userid + "')";

					SqlCommand cmd2 = new SqlCommand();
					cmd2.Connection = con;
					cmd2.Transaction = tra;
					cmd2.CommandText = sqlstr2;

					if (cmd2.ExecuteNonQuery() <= 0)
					{
						string sqlstr3 = "SET ROWCOUNT 1 update dt_abook set bookState = 1,borrowUserId = '" + userid + "', borrowTime = '" + DateTime.Now + "', returnTime = '" + (DateTime.Now + "'where bookIsbn like '" + bookid + "%' and bookState = 0 SET ROWCOUNT 0");

						SqlCommand cmd3 = new SqlCommand();
						cmd3.Connection = con;
						cmd3.Transaction = tra;
						cmd3.CommandText = sqlstr3;

						if (cmd3.ExecuteNonQuery() <= 0)
						{
							res = 2;//没书可借
							throw new Exception();
						}
					}


					tra.Commit();
					res = 0;
				}
				catch (Exception e)
				{
					if (res == 0)
						res = 3;
					tra.Rollback();
				}
			}
			return res;
		}

		/// <summary>
		/// 向数据库中添加书籍，采用数据库事物
		/// </summary>
		/// <param name="adminId"></param>
		/// <param name="adminPassword"></param>
		/// <param name="bk"></param>
		/// <returns></returns>
		public bool AddBook(string adminId, string adminPassword, ClassBook bk)
		{
			bool res = false;
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
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
		/// <summary>
		/// 书籍条件检索方法,将符合条件的书籍载入到book list中
		/// </summary>
		/// <param name="type">检索条件种类，1 全部条件，2 isbn，3 书名，4 作者，5 出版社，6标签</param>
		/// <param name="searchInfo">检索关键词</param>
		/// <param name="curnum">目前的访问序号</param>
		/// <returns></returns>
		public ClassBook[] SearchBook(int type, string searchInfo, int curnum, ref int linenum)
		{
			List<ClassBook> bk = new List<ClassBook>();

			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
			{
				con.Open();
				SqlCommand cmd = con.CreateCommand();
				switch (type)
				{
					case 1:
						{
							cmd.CommandText = "select count(*) from dt_book where (bookIsbn LIKE '%" + searchInfo + "%' or bookPublisher like '%" + searchInfo + "%' or bookAuthor like '%" + searchInfo + "%' or bookName like '%" + searchInfo + "%' or bookLable1 like '%" + searchInfo + "%' or bookLable2 like '%" + searchInfo + "%' or bookLable3 like '%" + searchInfo + "%')";
							linenum = Convert.ToInt32(cmd.ExecuteScalar());

							cmd.CommandText = "select * from  (select row_number() over(order by getdate()) 'rn',* from dt_book where (bookIsbn LIKE '%" + searchInfo + "%' or bookPublisher like '%" + searchInfo + "%' or bookAuthor like '%" + searchInfo + "%' or bookName like '%" + searchInfo + "%' or bookLable1 like '%" + searchInfo + "%' or bookLable2 like '%" + searchInfo + "%' or bookLable3 like '%" + searchInfo + "%'))t where rn between " + curnum + " and " + (curnum + 9);

							break;
						}
					case 2:
						{
							cmd.CommandText = "select count(*) from dt_book where (bookIsbn LIKE '%" + searchInfo + "%')";
							linenum = Convert.ToInt32(cmd.ExecuteScalar());

							cmd.CommandText = "select * from  (select row_number() over(order by getdate()) 'rn',* from dt_book where (bookIsbn LIKE '%" + searchInfo + "%'))t where rn between " + curnum + " and " + (curnum + 9);

							break;
						}
					case 3:
						{
							cmd.CommandText = "select count(*) from dt_book where (bookName like '%" + searchInfo + "%')";
							linenum = Convert.ToInt32(cmd.ExecuteScalar());

							cmd.CommandText = "select * from  (select row_number() over(order by getdate()) 'rn',* from dt_book where (bookName like '%" + searchInfo + "%'))t where rn between " + curnum + " and " + (curnum + 9);

							break;
						}
					case 4:
						{
							cmd.CommandText = "select count(*) from dt_book where (bookAuthor like '%" + searchInfo + "%')";
							linenum = Convert.ToInt32(cmd.ExecuteScalar());

							cmd.CommandText = "select * from  (select row_number() over(order by getdate()) 'rn',* from dt_book where (bookAuthor like '%" + searchInfo + "%'))t where rn between " + curnum + " and " + (curnum + 9);

							break;
						}
					case 5:
						{
							cmd.CommandText = "select count(*) from dt_book where (bookPublisher like '%" + searchInfo + "%')";
							linenum = Convert.ToInt32(cmd.ExecuteScalar());

							cmd.CommandText = "select * from  (select row_number() over(order by getdate()) 'rn',* from dt_book where (bookPublisher like '%" + searchInfo + "%'))t where rn between " + curnum + " and " + (curnum + 9);

							break;
						}
					case 6:
						{
							cmd.CommandText = "select count(*) from dt_book where (bookLable1 like '%" + searchInfo + "%'" + " or bookLable2 like '%" + searchInfo + "%'" + " or bookLable3 like '%" + searchInfo + "%')";
							linenum = Convert.ToInt32(cmd.ExecuteScalar());

							cmd.CommandText = "select * from  (select row_number() over(order by getdate()) 'rn',* from dt_book where (bookLable1 like '%" + searchInfo + "%'" + " or bookLable2 like '%" + searchInfo + "%'" + " or bookLable3 like '%" + searchInfo + "%'))t where rn between " + curnum + " and " + (curnum + 9);

							break;
						}
					default:
						break;
				}

				SqlDataReader rd = cmd.ExecuteReader();
				while (rd.HasRows && rd.Read())
				{
					bk.Add(new ClassBook(rd));
				}
			}
			return bk.ToArray();
		}

		public ClassBook GetBookDetail(string bookIsbn)
		{
			using (SqlConnection con = new SqlConnection(sql.Builder.ConnectionString))
			{
				SqlCommand cmd = con.CreateCommand();
				cmd.CommandText = "select * from dt_book where bookIsbn=@a";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@a", bookIsbn);

				con.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					while (dr.Read())
					{
						return new ClassBook(dr);
					}
				}
				else throw new Exception("请求的书号不存在！");
			}
			return null;
		}
		#endregion
	}
}
