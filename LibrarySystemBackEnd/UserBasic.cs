using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	/// <summary>
	/// 用户种类
	/// </summary>
	public enum USERTYPE
	{
		/// <summary>
		/// 学生
		/// </summary>
		Student,
		/// <summary>
		/// 老师
		/// </summary>
		Lecturer,
		/// <summary>
		/// 访客
		/// </summary>
		Guest
	};

	public class UserBasicInfo
	{
		public static readonly TimeSpan DefaultDate = new TimeSpan(30, 0, 0, 0, 0);
		public static readonly TimeSpan DefaultDelay = new TimeSpan(15, 0, 0, 0, 0);
		public static readonly int MaxScheduleAmount = 5;
		public static readonly int MaxCredit = 100;

		#region 私有属性
		private string username;
		private string userid;
		private string password;
		private string school;
		private USERTYPE usertype;
		private int currentscheduleamount;//当前已预约书数量
		private int maxborrowableamount;//最大借书数量
		private int currentborrowedamount;//当前借书数量
		private int currentmaxborrowableamount;//当前最大可借数量
		private int credit;///信用
						   ///满分100
						   ///每逾期3天还书信用减1
						   ///借书数量是信用百分比乘最大借书数量向上取整
						   ///交钱恢复信用一元一点信用
		private DateTime registerDate;
		#endregion

		#region 访问器
		/// <summary>
		/// 用户名
		/// </summary>
		public string Username
		{
			get
			{
				return username;
			}

			internal set
			{
				username = value;
			}
		}
		/// <summary>
		/// 用户id
		/// </summary>
		public string Userid
		{
			get
			{
				return userid;
			}

			internal set
			{
				userid = value;
			}
		}
		/// <summary>
		/// 用户密码
		/// </summary>
		public string Password
		{
			get
			{
				return password;
			}

			internal set
			{
				password = value;
			}
		}
		/// <summary>
		/// 学院
		/// </summary>
		public string School
		{
			get
			{
				return school;
			}

			internal set
			{
				school = value;
			}
		}
		/// <summary>
		/// 用户种类 学生 老师 访客
		/// </summary>
		public USERTYPE Usertype
		{
			get
			{
				return usertype;
			}

			internal set
			{
				usertype = value;
			}
		}
		/// <summary>
		/// 当前预约书籍数量
		/// </summary>
		public int Currentscheduleamount
		{
			get
			{
				return currentscheduleamount;
			}

			internal set
			{
				currentscheduleamount = value;
			}
		}
		/// <summary>
		/// 最大可借数量，依据用户种类而定
		/// </summary>
		public int Maxborrowableamount
		{
			get
			{
				return maxborrowableamount;
			}

			internal set
			{
				maxborrowableamount = value;
			}
		}
		/// <summary>
		/// 当前借书数量
		/// </summary>
		public int Currentborrowedamount
		{
			get
			{
				return currentborrowedamount;
			}

			internal set
			{
				currentborrowedamount = value;
			}
		}
		/// <summary>
		/// 当前最大借书数量，最大借书数量乘以信用百分比
		/// </summary>
		public int Currentmaxborrowableamount
		{
			get
			{
				return currentmaxborrowableamount;
			}

			internal set
			{
				currentmaxborrowableamount = value;
			}
		}
		/// <summary>
		/// 信用
		/// </summary>
		public int Credit
		{
			get
			{
				return credit;
			}

			internal set
			{
				if (value > 100) credit = 100;
				else credit = value;
			}
		}
		/// <summary>
		/// 注册日期
		/// </summary>
		public string RegisterDateToString
		{
			get
			{
				var a = RegisterDate.Year.ToString("D4");
				var b = RegisterDate.Month.ToString("D2");
				var c = RegisterDate.Day.ToString("D2");
				return a + "-" + b + "-" + c;
			}
		}

		public DateTime RegisterDate
		{
			get
			{
				return registerDate;
			}

			set
			{
				registerDate = value;
			}
		}


		#endregion

		public UserBasicInfo(string _username, string _userid, string _password, string _school, USERTYPE _usertype)
		{
			Username = _username;
			Userid = _userid;
			Password = _password;
			School = _school;
			Usertype = _usertype;
			Credit = 100;
			Currentborrowedamount = 0;
			Currentscheduleamount = 0;


			RegisterDate = ClassTime.systemTime;
			if (usertype == USERTYPE.Guest) Currentmaxborrowableamount = Maxborrowableamount = 0;
			else if (usertype == USERTYPE.Student) Currentmaxborrowableamount = Maxborrowableamount = 10;
			else if (usertype == USERTYPE.Lecturer) Currentmaxborrowableamount = Maxborrowableamount = 20;
		}

	}
}
