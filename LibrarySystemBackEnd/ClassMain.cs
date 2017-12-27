using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemBackEnd
{
	class ClassMain
	{
		public static void Main(string[] args)
		{
			//FileStream fs = File.Open("1.jpg", FileMode.Open);
			//byte[] bt = new byte[fs.Length];
			//fs.Read(bt, 0, Convert.ToInt32(fs.Length));
			//ClassBook kb = new ClassBook("性爱宝典", "2332332332333", 5, DateTime.Now, DateTime.Now, "admin", "两性教育", "保健书籍", "两性", "6-632出版社", "肖蒯", bt, "性爱指南");

			//ClassSQL sql = new ClassSQL();
			//sql.AddBook(kb);

			ClassBackEnd be = new ClassBackEnd();
			//be.BorrowBook("2015211255", "xzxxzx", "2332332332333");

			Console.ReadKey();
		}
	}
}
