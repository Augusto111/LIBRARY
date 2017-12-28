//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LibrarySystemBackEnd
//{
//	class ClassMain
//	{
//		public static void Main(string[] args)
//		{
//			ClassBackEnd bk = new ClassBackEnd();
//			FileStream fs = File.Open("1.jpg", FileMode.Open);
//			byte[] bt = new byte[fs.Length];
//			fs.Read(bt, 0, Convert.ToInt32(fs.Length));
//			for (int i = 100; i < 300; i++)
//			{
//				ClassBook kb = new ClassBook("book"+i.ToString(), "2332332332"+i.ToString(), 5, DateTime.Now, DateTime.Now, "admin", "计算机", "数学", "人工智能", "人民邮电出版社", "吴军", bt, "数学之美");
//				if (!bk.AddBook("admin", "admin", kb))
//					Console.WriteLine("RFG");
//			}
//			//ClassSQL sql = new ClassSQL();
//			//sql.AddBook(kb);

//			ClassBackEnd be = new ClassBackEnd();
//			//be.BorrowBook("2015211255", "xzxxzx", "2332332332333");
//			//int k = 0;
//			//ClassBook[] bk = be.SearchBook(2, "233", 1, ref k);

//			//Console.WriteLine(bk.Length);

//			Console.ReadKey();
//		}
//	}
//}
