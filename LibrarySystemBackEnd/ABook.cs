using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	/// <summary>
	/// 书籍状态 可借，已借，预约，不可用
	/// </summary>
	public enum BOOKSTATE
	{
		/// <summary>
		/// 可用，可借
		/// </summary>
		Available,
		/// <summary>
		/// 已被借阅
		/// </summary>
		Borrowed,
		/// <summary>
		/// 已被预约，等待取书
		/// </summary>
		Scheduled,
		/// <summary>
		/// 不可用
		/// </summary>
		Unavailable
	};

	/// <summary>
	/// 单一的一本书
	/// </summary>
	public class ABook
	{
		#region PrivateProperty

		/// <summary>
		/// 书籍状态，BOOKSTATE类型
		/// </summary>
		private BOOKSTATE bookState;

		/// <summary>
		/// 完整的isbn
		/// </summary>
		private string extIsbn;

		private string bookName;

		/// <summary>
		/// 借阅的用户id，没人借就是空
		/// </summary>
		private string borrowUserId;

		private DateTime borrowTime;

		private DateTime returnTime;

		/// <summary>
		/// 购买时间
		/// </summary>
		private DateTime broughtTime;

		#endregion

		#region 访问器
		/// <summary>
		/// 书籍状态，BOOKSTATE类型
		/// </summary>
		public BOOKSTATE Bookstate
		{
			get
			{
				return bookState;
			}

			internal set
			{
				bookState = value;
			}
		}
		/// <summary>
		/// 借阅的用户id，没人借就是空
		/// </summary>
		public string Borrowuserid
		{
			get
			{
				return borrowUserId;
			}

			internal set
			{
				borrowUserId = value;
			}
		}
		/// <summary>
		/// 借阅时间，没人借就是空
		/// </summary>
		public string Broughttime
		{
			get
			{
				var a = broughtTime.Year.ToString("D4");
				var b = broughtTime.Month.ToString("D2");
				var c = broughtTime.Day.ToString("D2");
				return a + "-" + b + "-" + c;
			}
		}
		/// <summary>
		/// 完整的isbn
		/// </summary>
		public string Extisbn
		{
			get
			{
				return extIsbn;
			}

			internal set
			{
				extIsbn = value;
			}
		}



		#endregion

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="_bookstate">书籍状态，枚举类型</param>
		/// <param name="_borrowuserid">借书用户id，没借为空</param>
		/// <param name="_broughttime">结借书时间</param>
		/// <param name="_extisbn">完整的isbn</param>
		internal ABook(BOOKSTATE _bookstate, string _borrowuserid, DateTime _broughttime, string _extisbn)
		{
			Bookstate = _bookstate;
			Borrowuserid = _borrowuserid;
			broughtTime = _broughttime;
			Extisbn = _extisbn;
		}
		/// <summary>
		/// 写入文件函数
		/// </summary>
		/// <param name="sw">streamwriter类型</param>
		internal void SaveToFile(StreamWriter sw)
		{
			sw.WriteLine(Convert.ToInt32(Bookstate));
			sw.WriteLine(Borrowuserid);
			sw.WriteLine(broughtTime.ToString());
			sw.WriteLine(Extisbn);
		}
		/// <summary>
		/// 从文件的构造函数
		/// </summary>
		/// <param name="sr">streamreader类型</param>
		internal ABook(StreamReader sr)
		{
			Bookstate = (BOOKSTATE)(System.Enum.Parse(typeof(BOOKSTATE), Convert.ToInt16(sr.ReadLine()).ToString()));
			Borrowuserid = sr.ReadLine();
			broughtTime = Convert.ToDateTime(sr.ReadLine());
			Extisbn = sr.ReadLine();
		}
	}
}
