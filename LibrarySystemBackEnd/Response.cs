using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	static class Response
	{
		public static void dealRequest(ref FileProtocol protocol)
		{
			if (protocol.Mode == RequestMode.UserLogin)
			{
				ClassBackEnd bk = new ClassBackEnd();

				int res = bk.Login(protocol.Userinfo.UserId, protocol.Userinfo.UserPassword);

				protocol.Retval = res;
				Thread.Sleep(5000);
			}
			else if (protocol.Mode == RequestMode.UserRegist)
			{
				ClassBackEnd bk = new ClassBackEnd();

				bool res = bk.RegisterUser(protocol.Userinfo.UserId, protocol.Userinfo.UserName, protocol.Userinfo.UserPassword, protocol.Userinfo.UserSchool, protocol.Userinfo.UserType);

				protocol.Retval = Convert.ToInt32(res);

				Thread.Sleep(5000);
			}
			else if (protocol.Mode == RequestMode.UserSearchBook)
			{
				ClassBackEnd bk = new ClassBackEnd();
				int k = 0;
				protocol.Resbook = bk.SearchBook(protocol.SearchCat, protocol.SearchWords, protocol.CurNum, ref k);
				protocol.EndNum = k;

				Thread.Sleep(5000);
			}
		}
	}
}
