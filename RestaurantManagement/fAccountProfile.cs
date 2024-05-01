using RestaurantManagement.DAO;
using RestaurantManagement.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantManagement
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;
        }

        private void ChangeAccount(Account acc)
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;
        }

        void UpdateAccount()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassword.Text;
            string username = txbUserName.Text;
            string newPassword = txbNewPassword.Text;
            string reEnterPassword = txbReEnterPassword.Text;

            if(!newPassword.Equals(reEnterPassword))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu trùng với mật khẩu mới!");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(username, displayName, password, newPassword)) 
                {
                    MessageBox.Show("Cập nhật thành công!");
                    if(updateDisplayAccount != null)
                    {
                        updateDisplayAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUsername(username)));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu!");
                }
            }
        }

        private event EventHandler<AccountEvent> updateDisplayAccount;
        public event EventHandler<AccountEvent> UpdateDisplayAccount
        {
            add { updateDisplayAccount += value; } 
            remove { updateDisplayAccount -= value; }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
