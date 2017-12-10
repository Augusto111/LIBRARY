using System;
using System.Collections.Generic;
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
	}
}
