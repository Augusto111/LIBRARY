﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using LibrarySystemBackEnd;

namespace LIBRARY
{
    public partial class UserReturnForm : DMSkin.Main
    {
        private int index;
        public UserReturnForm(int _index)
        {
            index = _index;
            InitializeComponent();
        }

        private void ReturnForm_Load(object sender, EventArgs e)
        {
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddEllipse(0, 0, 102, 102);
            ReturnButton.Region = new Region(myPath);
            ReBorrowButton.Region = new Region(myPath);
            BookDetailLoad();
        }


        #region 按钮动画
        private void ReturnButton_MouseMove(object sender, MouseEventArgs e)
        {
            ReturnButton.BackgroundImage = ReturnButton.DM_HoverImage;
        }

        private void ReturnButton_MouseLeave(object sender, EventArgs e)
        {
            ReturnButton.BackgroundImage = ReturnButton.DM_NolImage;
        }

        private void ReBorrowButton_MouseMove(object sender, MouseEventArgs e)
        {
            ReBorrowButton.BackgroundImage = ReBorrowButton.DM_HoverImage;
        }

        private void ReBorrowButton_MouseLeave(object sender, EventArgs e)
        {
            ReBorrowButton.BackgroundImage = ReBorrowButton.DM_NolImage;
        }
        #endregion
        private void BookDetailLoad()
        {
            BookNameText.Text = ClassBackEnd.Currentbook.BookName;
            AuthorText.Text = ClassBackEnd.Currentbook.BookAuthor;
            BookIDText.Text = ClassBackEnd.Currentbook.BookIsbn;
            PublisherText.Text = ClassBackEnd.Currentbook.BookPublisher;
            BorrowDateText.Text = ClassBackEnd.BorrowedBookI.Bsdate;
            ReturnDateText.Text = ClassBackEnd.BorrowedBookI.Rgdate;
            try
            {
                BookPictureBox.Image = Image.FromFile(ClassBackEnd.Currentbook.BookImage);
            }
            catch
            {
                BookPictureBox.Image = Properties.Resources.BookNullImage;//set default image
            }

        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            if (ClassBackEnd.ReturnBook())
            {
                MessageBox ib = new MessageBox(16);
                ib.ShowDialog();
                ib.Dispose();
                Close();
            }
            else
            {
                MessageBox ib = new MessageBox(9);
                ib.ShowDialog();
                ib.Dispose();
            }
        }

        private void ReBorrowButton_Click(object sender, EventArgs e)
        {
            var t = ClassBackEnd.RenewBook();
            if (t == 1)
            {
                MessageBox ib = new MessageBox(17);
                ib.ShowDialog();
                ib.Dispose();
                Close();
            }
            else if (t == 2)
            {
                MessageBox ib = new MessageBox(18);
                ib.ShowDialog();
                ib.Dispose();
            }
            else if (t == 3)
            {
                MessageBox ib = new MessageBox(20);
                ib.ShowDialog();
                ib.Dispose();
            }
            else
            {
                MessageBox ib = new MessageBox(19);
                ib.ShowDialog();
                ib.Dispose();
            }
        }

        private void ShutDownButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
