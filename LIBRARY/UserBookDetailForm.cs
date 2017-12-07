﻿using System;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibrarySystemBackEnd;

namespace LIBRARY
{
    public partial class UserBookDetailForm : DMSkin.Main
    {
        private UserMainForm frmMain;

        private int bookIndex;//booklist索引

        public UserBookDetailForm(UserMainForm frm, int bookindex)
        {
            bookIndex = bookindex;
            frmMain = frm;
            InitializeComponent();
            if (PublicVar.GuestFlag == 1)
            {
                BookBorrowButton.Hide();
                BookOrderButton.Hide();
            }
        }

        public void BookListRefresh()
        {
            ResultDataSheet.Rows.Clear();//清空上一次搜索表
            for (int i = 0; i < ClassBackEnd.Currentbook.BookAmount; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                int index = ResultDataSheet.Rows.Add(row);
                ResultDataSheet.Rows[index].Cells[0].Value = ClassBackEnd.Currentbook.Book[i].Extisbn;
                if (ClassBackEnd.Currentbook.Book[i].Bookstate == BOOKSTATE.Available)
                {
                    ResultDataSheet.Rows[index].Cells[1].Value = "可借";
                }
                else if (ClassBackEnd.Currentbook.Book[i].Bookstate == BOOKSTATE.Borrowed)
                {
                    ResultDataSheet.Rows[index].Cells[1].Value = "已借";
                }
                else if (ClassBackEnd.Currentbook.Book[i].Bookstate == BOOKSTATE.Unavailable)
                {
                    ResultDataSheet.Rows[index].Cells[1].Value = "不可用";
                }
                else if (ClassBackEnd.Currentbook.Book[i].Bookstate == BOOKSTATE.Scheduled)
                {
                    ResultDataSheet.Rows[index].Cells[1].Value = "仅预约";
                }

                ResultDataSheet.Rows[index].Height = 40;
            }
            ResultDataSheet.ClearSelection();
            ResultDataSheet.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ResultDataSheet.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void BookDetailLoad()
        {
            ClassBackEnd.LoadSearchResult(bookIndex);
            BookNameLabel.Text = ClassBackEnd.Currentbook.BookName;
            AuthorText.Text = ClassBackEnd.Currentbook.BookAuthor;
            BookIDText.Text = ClassBackEnd.Currentbook.BookIsbn;
            PublisherText.Text = ClassBackEnd.Currentbook.BookPublisher;
            BookInfoTextbox.Text = ClassBackEnd.Currentbook.BookIntroduction;
            AmountText.Text = ClassBackEnd.Currentbook.BookAmount.ToString();
            Label1Text.Text = ClassBackEnd.Currentbook.BookLable1;
            Label2Text.Text = ClassBackEnd.Currentbook.BookLable2;
            Label3Text.Text = ClassBackEnd.Currentbook.BookLable3;
            try
            {
                BookPictureBox.Image = Image.FromFile(ClassBackEnd.Currentbook.BookImage);
            }
            catch
            {
                BookPictureBox.Image = Properties.Resources.BookNullImage;//set default image
            }

        }

        private void OrderOrBorrow()
        {
            if (ClassBackEnd.HasBorrowed(bookIndex))
            {
                BookBorrowButton.DM_NolImage = Properties.Resources.AlreadyBorrow;
                BookBorrowButton.Enabled = false;
                BookOrderButton.Hide();
            }
            else if (ClassBackEnd.GetBookState(bookIndex) == 1)
            {
                BookOrderButton.Hide();
            }
            else
            {
                if (ClassBackEnd.HasScheduled(bookIndex))
                {
                    BookOrderButton.DM_NolImage = Properties.Resources.AlreadyOrder;
                    BookOrderButton.Enabled = false;
                    BookBorrowButton.Hide();
                }
                else if(ClassBackEnd.GetBookState(bookIndex) == 2)
                {
                    BookBorrowButton.Hide();
                    BookOrderButton.Show();
                }
                else
                {
                    BookBorrowButton.Hide();
                    BookOrderButton.Hide();
                    NoUseButton.Show();
                }
            }
        }

        private void BookDetailForm_Load(object sender, EventArgs e)
        {


            BookDetailLoad();
            if (PublicVar.GuestFlag != 1) OrderOrBorrow();

            frmMain.ReturnButton.Tag = 2;

            #region 设置圆形按钮区域
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddEllipse(0, 0, 96, 96);
            BookBorrowButton.Region = new Region(myPath);
            BookOrderButton.Region = new Region(myPath);
            #endregion

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            BookListRefresh();

        }

        private void BookBorrowButton_Click(object sender, EventArgs e)
        {
            var b = ClassBackEnd.BorrowBook(bookIndex);
            if (b == 1)
            {
                #region Infobox Show
                MessageBox infoBox = new MessageBox(1);
                infoBox.ShowDialog();
                infoBox.Dispose();
                #endregion
                BookBorrowButton.DM_NolImage = Properties.Resources.AlreadyBorrow;
                BookBorrowButton.Enabled = false;
            }
            else if (b == 2)
            {
                #region Infobox Show
                MessageBox infoBox = new MessageBox(11);
                infoBox.ShowDialog();
                infoBox.Dispose();
                #endregion
                BookBorrowButton.DM_NolImage = Properties.Resources.AlreadyBorrow;
                BookBorrowButton.Enabled = false;
            }
            else if (b == 0)
            {
                #region Infobox Show
                MessageBox infoBox = new MessageBox(9);   
                infoBox.ShowDialog();
                infoBox.Dispose();
                #endregion
            }
            BookListRefresh();
        }

        private void BookOrderButton_Click(object sender, EventArgs e)
        {
            var v = ClassBackEnd.ScheduleBook(bookIndex);
            if (v)
            {
                #region Infobox Show
                MessageBox infoBox = new MessageBox(2);
                infoBox.ShowDialog();
                infoBox.Dispose();
                #endregion
            }
            else
            {
                #region Infobox Show
                MessageBox infoBox = new MessageBox(12);
                infoBox.ShowDialog();
                infoBox.Dispose();
                #endregion
            }
            BookOrderButton.DM_NolImage = Properties.Resources.AlreadyOrder;
            BookOrderButton.Enabled = false;
            BookListRefresh();
        }

        private void NoUseButton_Click(object sender, EventArgs e)
        {
            #region Infobox Show
            MessageBox infoBox = new MessageBox(22);
            infoBox.ShowDialog();
            infoBox.Dispose();
            #endregion
        }

        private void BookOrderButton_MouseMove(object sender, MouseEventArgs e)
        {
            BookOrderButton.BackgroundImage = BookOrderButton.DM_HoverImage;
        }

        private void BookOrderButton_MouseLeave(object sender, EventArgs e)
        {
            BookOrderButton.BackgroundImage = BookOrderButton.DM_NolImage;
        }

        private void BookBorrowButton_MouseMove(object sender, MouseEventArgs e)
        {
            BookBorrowButton.BackgroundImage = BookBorrowButton.DM_HoverImage;
        }

        private void BookBorrowButton_MouseLeave(object sender, EventArgs e)
        {
            BookBorrowButton.BackgroundImage = BookBorrowButton.DM_NolImage;
        }
	}
}
