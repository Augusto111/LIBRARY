using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystemBackEnd;

namespace LibrarySystemBackEnd
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
		private ClassUserBasicInfo userinfo;
		private string searchWords;
		private int searchCat;
		private int curNum, endNum;
		private int returnVal;
		private ClassBook[] resBook;

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
				return returnVal;
			}

			set
			{
				returnVal = value;
			}
		}

		public string SearchWords
		{
			get
			{
				return searchWords;
			}

			set
			{
				searchWords = value;
			}
		}

		public int SearchCat
		{
			get
			{
				return searchCat;
			}

			set
			{
				searchCat = value;
			}
		}

		public int CurNum
		{
			get
			{
				return curNum;
			}

			set
			{
				curNum = value;
			}
		}

		public int EndNum
		{
			get
			{
				return endNum;
			}

			set
			{
				endNum = value;
			}
		}

		public ClassBook[] Resbook
		{
			get
			{
				return resBook;
			}

			set
			{
				resBook = value;
			}
		}

		public override string ToString()
		{
			switch (mode)
			{
				case RequestMode.UserLogin:
					{
						return String.Format("<protocol><file mode=\"{0}\" port=\"{1}\" retval=\"{2}\" /></protocol>", mode, port, returnVal);
					}
				case RequestMode.UserRegist:
					{
						return String.Format("<protocol><file mode=\"{0}\" port=\"{1}\" retval=\"{2}\" /></protocol>", mode, port, returnVal);
					}
				case RequestMode.UserSearchBook:
					{
						string ret = "<protocol>";
						ret += String.Format("<file mode=\"{0}\" port=\"{1}\" />", mode, port);
						ret += String.Format("<usersearchbook curnum=\"{0}\" endnum=\"{1}\" thisnum=\"{2}\" />", curNum, endNum, resBook.Length);
						for(int i=0;i<resBook.Length;i++)
						{
							ret += String.Format("<book bookisbn=\"{0}\" bookname=\"{1}\" bookauthor=\"{2}\" bookpublisher=\"{3}\" >");
						}
						ret += "</protocol>";
						return ret;
					}
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
			return "";
		}
	}
}
