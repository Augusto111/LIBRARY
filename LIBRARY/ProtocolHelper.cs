﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LIBRARY
{
    class ProtocolHelper
    {
        private XmlNode fileNode;
        private XmlNode root;

        public ProtocolHelper(string protocol)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(protocol);
            root = doc.DocumentElement;
            fileNode = root.SelectSingleNode("file");
        }

        // 此时的protocal一定为单条完整protocal
        // 获取单条协议包含的信息
        public FileProtocol GetProtocol()
        {
            RequestMode mode = (RequestMode)Enum.Parse(typeof(RequestMode), fileNode.Attributes["mode"].Value, false);
            int port = Convert.ToInt32(fileNode.Attributes["port"].Value);
            FileProtocol pro = new FileProtocol(mode, port);

            switch (mode)
            {
                case RequestMode.UserLogin:
                    {
                        XmlNode usernode = root.SelectSingleNode("file");
                        //ClassUserBasicInfo user = new ClassUserBasicInfo(usernode);
                        //pro.Userinfo = user;
                        pro.Retval = Convert.ToInt32(usernode.Attributes["retval"].Value);
                        break;
                    }
                case RequestMode.UserRegist:
                    {
                        XmlNode usernode = root.SelectSingleNode("file");
                        pro.Retval = Convert.ToInt32(usernode.Attributes["retval"].Value);
                        break;
                    }
                case RequestMode.UserSearchBook:
                    {
                        XmlNode searchnode = root.SelectSingleNode("usersearchbook");
                        pro.Curnum = Convert.ToInt32(searchnode.Attributes["curnum"].Value);
                        pro.Endnum = Convert.ToInt32(searchnode.Attributes["endnum"].Value);
                        pro.Amo = Convert.ToInt32(searchnode.Attributes["amo"].Value);
                        XmlNodeList li = root.SelectNodes("book");
                        List<ClassBook> bk = new List<ClassBook>();
                        foreach (XmlNode no in li)
                        {
                            bk.Add(new ClassBook(no.Attributes["bookname"].Value, no.Attributes["bookisbn"].Value, no.Attributes["bookauthor"].Value, no.Attributes["bookpublisher"].Value));
                        }
                        pro.Resbook = bk.ToArray();
                        break;
                    }
                case RequestMode.UserBookDetailLoad:
                    {
                        XmlNode bookNode = root.SelectSingleNode("book");
                        pro.NowBook = new ClassBook(bookNode.Attributes["bookisbn"].Value);
                        pro.NowBook.BookName = bookNode.Attributes["bookname"].Value;
                        pro.NowBook.BookPublisher = bookNode.Attributes["bookpublisher"].Value;
                        pro.NowBook.BookPublishTime = DateTime.Parse(bookNode.Attributes["bookpublishtime"].Value);
                        pro.NowBook.BookAuthor = bookNode.Attributes["bookauthor"].Value;
                        pro.NowBook.BookIntroduction = bookNode.Attributes["bookintroduction"].Value;
                        pro.NowBook.BookImage = bookNode.Attributes["bookpic"].Value;
                        pro.NowBook.BookLable1 = bookNode.Attributes["booklable1"].Value;
                        pro.NowBook.BookLable2 = bookNode.Attributes["booklable2"].Value;
                        pro.NowBook.BookLable3 = bookNode.Attributes["booklable3"].Value;
                        pro.NowBook.BookAmount = Convert.ToInt32(bookNode.Attributes["bookamo"].Value);

                        break;
                    }
                case RequestMode.UserBookStateLoad:
                    {
                        XmlNode bookNode = root.SelectSingleNode("book");
                        int k = Convert.ToInt32(bookNode.Attributes["bookamount"].Value);
                        XmlNodeList li = root.SelectNodes("bookstate");
                        List<ClassABook> bk = new List<ClassABook>();
                        foreach (XmlNode no in li)
                        {
                            ClassABook abk = new ClassABook(no.Attributes["bookextisbn"].Value);
                            abk.BookState = (Bookstate)Enum.ToObject(typeof(Bookstate), no.Attributes["bookstate"].Value);

                            bk.Add(abk);
                        }
                        XmlNode usernode = root.SelectSingleNode("file");
                        pro.Retval = Convert.ToInt32(usernode.Attributes["retval"].Value);
                        pro.EachBookState = bk.ToArray();
                        break;
                    }
                case RequestMode.UserBookLoad:
                    {
                        XmlNode bookNode = root.SelectSingleNode("book");
                        pro.NowBook = new ClassBook(bookNode.Attributes["bookisbn"].Value);
                        pro.NowBook.BookName = bookNode.Attributes["bookname"].Value;
                        pro.NowBook.BookPublisher = bookNode.Attributes["bookpublisher"].Value;
                        pro.NowBook.BookPublishTime = DateTime.Parse(bookNode.Attributes["bookpublishtime"].Value);
                        pro.NowBook.BookAuthor = bookNode.Attributes["bookauthor"].Value;
                        pro.NowBook.BookIntroduction = bookNode.Attributes["bookintroduction"].Value;
                        pro.NowBook.BookImage = bookNode.Attributes["bookpic"].Value;
                        pro.NowBook.BookLable1 = bookNode.Attributes["booklable1"].Value;
                        pro.NowBook.BookLable2 = bookNode.Attributes["booklable2"].Value;
                        pro.NowBook.BookLable3 = bookNode.Attributes["booklable3"].Value;
                        pro.NowBook.BookAmount = Convert.ToInt32(bookNode.Attributes["bookamo"].Value);
                        
                        int k = Convert.ToInt32(bookNode.Attributes["bookamo"].Value);
                        XmlNodeList li = root.SelectNodes("bookstate");
                        List<ClassABook> bk = new List<ClassABook>();
                        foreach (XmlNode no in li)
                        {
                            ClassABook abk = new ClassABook(no.Attributes["bookextisbn"].Value);
                            abk.BookState = (Bookstate)Enum.Parse(typeof(Bookstate), no.Attributes["bookstate"].Value, false); 

                            bk.Add(abk);
                        }
                        XmlNode usernode = root.SelectSingleNode("file");
                        pro.Retval = Convert.ToInt32(usernode.Attributes["retval"].Value);
                        pro.EachBookState = bk.ToArray();
                        break;
                    }
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


            return pro;
        }
    }
}
