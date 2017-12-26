using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystemBackEnd;

namespace NetWorkApp
{
	/// <summary>
	/// 协议类型字段
	/// </summary>
	public enum RequestMode
	{
		UserLogin = 0,
		UserRegist,
		UserSearchBook,

		UserBookLoad,
		UserBookStateLoad,
		UserBookCommentLoad,
		UserBorrowBook,
		UserCommentBook,
		UserDelComment,
		UserOrderBook,

		UserInfoLoad,
		UserInfoChange,
		UserNotificationLoad,
		UserBorrowedBook,
		UserBorrowHis,
		UserBadRecord,

		UserAbookLoad,
		UserReturnBook,
		UserDelayBook,
	}
	class FileProtocol
	{
		private RequestMode mode;
		private int port;
		private LibrarySystemBackEnd.ClassUserBasicInfo userinfo;
		private int retval;

		public FileProtocol(RequestMode mode, int port)
		{
			this.mode = mode;
			this.port = port;
		}

		public RequestMode Mode
		{
			get { return mode; }
		}

		public int Port
		{
			get { return port; }
		}

		public ClassUserBasicInfo Userinfo
		{
			get
			{
				return userinfo;
			}

			set
			{
				userinfo = value;
			}
		}

		public int Retval
		{
			get
			{
				return retval;
			}

			set
			{
				retval = value;
			}
		}

		public override string ToString()
		{
			switch (mode)
			{
				case RequestMode.UserLogin:
					{
						return String.Format("<protocol><file mode=\"{0}\" port=\"{1}\" retval=\"{2}\" /></protocol>", mode, port, retval);
					}
				case RequestMode.UserRegist:
					{
						return String.Format("<protocol><file mode=\"{0}\" port=\"{1}\" retval=\"{2}\" /></protocol>", mode, port, retval);
					}
				case RequestMode.UserSearchBook:
					break;
				case RequestMode.UserBookLoad:
					break;
				case RequestMode.UserBookStateLoad:
					break;
				case RequestMode.UserBookCommentLoad:
					break;
				case RequestMode.UserBorrowBook:
					break;
				case RequestMode.UserCommentBook:
					break;
				case RequestMode.UserDelComment:
					break;
				case RequestMode.UserOrderBook:
					break;
				case RequestMode.UserInfoLoad:
					break;
				case RequestMode.UserInfoChange:
					break;
				case RequestMode.UserNotificationLoad:
					break;
				case RequestMode.UserBorrowedBook:
					break;
				case RequestMode.UserBorrowHis:
					break;
				case RequestMode.UserBadRecord:
					break;
				case RequestMode.UserAbookLoad:
					break;
				case RequestMode.UserReturnBook:
					break;
				case RequestMode.UserDelayBook:
					break;
				default:
					break;
			}

		}
	}
}
