using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkApp
{
	/// <summary>
	/// 协议类型字段
	/// </summary>
	public enum RequestMode
	{
		UserLogin=0,
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

	}
	class FileProtocol
	{
		private readonly RequestMode mode;
		private readonly int port;
		private readonly string userName;
		private readonly string userPassword;
		private readonly bool loginRes;

		public FileProtocol(RequestMode mode, int port, string userName,string userPassword)
		{
			this.mode = mode;
			this.port = port;
			this.userName = userName;
			this.userPassword = userPassword;
		}

		public RequestMode Mode
		{
			get { return mode; }
		}

		public int Port
		{
			get { return port; }
		}

		public string UserName
		{
			get
			{
				return userName;
			}
		}

		public string UserPassword
		{
			get
			{
				return userPassword;
			}
		}

		public override string ToString()
		{
			return String.Format("<protocol><file mode=\"{0}\" port=\"{1}\" userName=\"{2}\" userPassword=\"{3}\" /></protocol>"
				, mode, port, UserName, UserPassword);
		}
	}
}
