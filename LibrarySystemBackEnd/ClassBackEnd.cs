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
			ClassUserBasicInfo user = sql.getUsersBasic(id);
			ClassAdmin admin = sql.getAdmin(id);
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
			bool fl = sql.AddUser(new ClassUserBasicInfo(userid, username, password, school, usertype));
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
			bool fl = sql.AddAdmin(new ClassAdmin(id, name, password, type));
			return fl;
		}
		/// <summary>
		/// 借书
		/// </summary>
		/// <param name="userid">用户id</param>
		/// <param name="password">用户密码</param>
		/// <param name="bookid">书籍id</param>
		/// <returns>返回：0借书成功，1用户借书数量上限，2没有可借书籍，3用户已经借阅过，4</returns>
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
					string sqlstr = "select *from dt_UserBasic with(TABLOCKX) where userId='" + userid + "' and userPassword='" + password + "'";
					DataSet ds = new DataSet();
					SqlDataAdapter adp = new SqlDataAdapter(sqlstr, con);
					adp.SelectCommand.Transaction = tra;

					SqlCommandBuilder cb = new SqlCommandBuilder(adp);
					adp.Fill(ds);

					DataRow dr = ds.Tables[0].Rows[0];
					int tmp1 = Convert.ToInt32(dr["userCurrentBorrowedAmount"]);
					int tmp2 = Convert.ToInt32(dr["userCurrentMaxBorrowableAmount"]);
					if (tmp1 >= tmp2)
					{
						res = 1;//借阅数量上限
						throw new Exception();
					}
					tmp1++;
					dr["userCurrentBorrowedAmount"] = tmp1.ToString();

					adp.Update(ds.GetChanges());
					ds.AcceptChanges();


					sqlstr = "select *from dt_abook with(TABLOCKX) where bookIsbn like '" + bookid + "%'";
					ds = new DataSet();
					adp = new SqlDataAdapter(sqlstr, con);
					adp.SelectCommand.Transaction = tra;

					cb = new SqlCommandBuilder(adp);
					adp.Fill(ds);
					if (ds.Tables[0].Rows.Count == 0)
					{
						res = 2;//没书可借
						throw new Exception();
					}

					bool hasfin = false;
					foreach (DataRow rowtmp in ds.Tables[0].Rows)
					{
						if (rowtmp["borrorUserId"].ToString() == userid)
						{
							BOOKSTATE thisstate = (BOOKSTATE)Enum.ToObject(typeof(BOOKSTATE), rowtmp["bookState"]);
							if (thisstate == BOOKSTATE.Borrowed || thisstate == BOOKSTATE.Unavailable)
							{
								res = 3;
								throw new Exception();
							}
							hasfin = true;
							rowtmp["bookState"] = BOOKSTATE.Borrowed;
							rowtmp["borrorUserId"] = userid;
							rowtmp["borrowTime"] = DateTime.Now;
							rowtmp["returnTime"] = DateTime.Now + ClassUserBasicInfo.DefaultDate;
							break;
						}
					}
					if (!hasfin)
					{
						foreach (DataRow rowtmp in ds.Tables[0].Rows)
						{
							if ((BOOKSTATE)Enum.ToObject(typeof(BOOKSTATE), rowtmp["bookState"]) == BOOKSTATE.Available)
							{
								hasfin = true;
								rowtmp["bookState"] = BOOKSTATE.Borrowed;
								rowtmp["borrorUserId"] = userid;
								rowtmp["borrowTime"] = DateTime.Now;
								rowtmp["returnTime"] = DateTime.Now + ClassUserBasicInfo.DefaultDate;
								break;
							}
						}
					}
					if (!hasfin)
					{
						res = 4;
						throw new Exception();
					}
					adp.Update(ds.GetChanges());
					ds.AcceptChanges();
					tra.Commit();
					res = 0;
				}
				catch (Exception e)
				{
					if (res == 0)
						res = 4;
					tra.Rollback();
				}
			}
			return res;
		}


	}
}
