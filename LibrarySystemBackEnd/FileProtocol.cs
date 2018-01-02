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

		UserBookDetailLoad,
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

		UserBookLoad,
		PicReceive,
		PicSend
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
		private string fileName;
		private ClassBook[] resBook;
		private ClassBook nowBook;
		private ClassABook[] eachBookState;

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

		public ClassBook NowBook
		{
			get
			{
				return nowBook;
			}

			set
			{
				nowBook = value;
			}
		}

		public ClassABook[] Bks
		{
			get
			{
				return eachBookState;
			}

			set
			{
				eachBookState = value;
			}
		}

		public string FileName
		{
			get
			{
				return fileName;
			}

			set
			{
				fileName = value;
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
						ret += String.Format("<usersearchbook curnum=\"{0}\" endnum=\"{1}\" amo=\"{2}\" />", curNum, endNum, resBook.Length);
						for(int i=0;i<resBook.Length;i++)
						{
							ret += String.Format("<book bookisbn=\"{0}\" bookname=\"{1}\" bookauthor=\"{2}\" bookpublisher=\"{3}\" />", resBook[i].BookIsbn, resBook[i].BookName, resBook[i].BookAuthor, resBook[i].BookPublisher);
						}
						ret += "</protocol>";
						return ret;
					}
				case RequestMode.UserBookDetailLoad:
					{
						string ret = "<protocol>";
						ret += String.Format("<file mode=\"{0}\" port=\"{1}\" />", mode, port);
						ret += String.Format("<book bookname=\"{0}\" bookauthor=\"{1}\" bookpublisher=\"{2}\" bookisbn=\"{3}\" bookamo=\"{4}\" booklable1=\"{5}\" booklable2=\"{6}\" booklable3=\"{7}\" bookintroduction=\"{8}\" bookpic=\"{9}\" bookpublishtime=\"{10}\" />", nowBook.BookName, nowBook.BookAuthor, nowBook.BookPublisher, nowBook.BookIsbn, nowBook.BookAmount, nowBook.BookLable1, nowBook.BookLable2, nowBook.BookLable3, nowBook.BookIntroduction, nowBook.BookImage, nowBook.BookPublishTime);
						ret += "</protocol>";
						return ret;
					}
				case RequestMode.UserBookStateLoad:
					{
						string ret = "<protocol>";
						ret += String.Format("<file mode=\"{0}\" port=\"{1}\" retval={2} />", mode, port, Retval);
						ret += String.Format("<book bookisbn=\"{0}\" bookamount=\"{1}\" />", nowBook.BookIsbn,Bks.Length);
						for(int i=0;i<eachBookState.Length;i++)
						{
							ret += String.Format("<bookstate bookextisbn=\"{0}\" bookstate=\"{1}\" />", eachBookState[i].BookName, eachBookState[i].BookState);
						}
						ret += "</protocol>";
						return ret;
					}
				case RequestMode.UserBookCommentLoad:
					{
						//string ret = "<protocol>";
						//ret += String.Format("<file mode=\"{0}\" port=\"{1}\" />", mode, port);
						//ret += String.Format("<book bookisbn=\"{0}\" bookamount=\"{1}\" />", nowBook.BookIsbn, Bks.Length);
						//for (int i = 0; i < eachBookState.Length; i++)
						//{
						//	ret += String.Format("<bookstate bookextisbn=\"{0}\" bookstate=\"{1}\" />", eachBookState[i].BookName, eachBookState[i].BookState);
						//}
						//ret += "</protocol>";
						break;
					}
				case RequestMode.UserBookLoad:
					{
						string ret = "<protocol>";
						ret += String.Format("<file mode=\"{0}\" port=\"{1}\" retval=\"{2}\" />", mode, port, Retval);

						ret += String.Format("<book bookname=\"{0}\" bookauthor=\"{1}\" bookpublisher=\"{2}\" bookisbn=\"{3}\" bookamo=\"{4}\" booklable1=\"{5}\" booklable2=\"{6}\" booklable3=\"{7}\" bookintroduction=\"{8}\" bookpic=\"{9}\" bookpublishtime=\"{10}\" />", nowBook.BookName, nowBook.BookAuthor, nowBook.BookPublisher, nowBook.BookIsbn, nowBook.BookAmount, nowBook.BookLable1, nowBook.BookLable2, nowBook.BookLable3, nowBook.BookIntroduction, NowBook.BookPicHash, nowBook.BookPublishTime);
						
						for (int i = 0; i < eachBookState.Length; i++)
						{
							ret += String.Format("<bookstate bookextisbn=\"{0}\" bookstate=\"{1}\" />", eachBookState[i].BookIsbn, eachBookState[i].BookState);
						}
						ret += "</protocol>";
						return ret;
					}
				case RequestMode.PicSend:
					{
						string ret = "<protocol>";
						ret += String.Format("<file mode=\"{0}\" port=\"{1}\" filename=\"{2}\" />", mode, port, fileName);
						ret += "</protocol>";
						return ret;
					}
				case RequestMode.PicReceive:
					{
						string ret = "<protocol>";
						ret += String.Format("<file mode=\"{0}\" port=\"{1}\" filename=\"{2}\" />", mode, port, fileName);
						ret += "</protocol>";
						return ret;
					}
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
