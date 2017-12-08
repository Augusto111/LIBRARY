using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	class ClassMain
	{
		public static void Main(string[] args)
		{
			UserBasicInfo user = new UserBasicInfo("2015211255", "xzx", "xzxxzx", "计算机学院", USERTYPE.Student);
			SQL.AddUser(user);
		}
	}
}
