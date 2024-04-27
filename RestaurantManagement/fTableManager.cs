using RestaurantManagement.DAO;
using RestaurantManagement.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestaurantManagement
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();

            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }

        #region Methods
        void LoadTable()
        {
            flpTable.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table table in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height=TableDAO.TableHeight };  
                btn.Text = table.Name + "\n" + table.Status;
                btn.Click += btn_Click;
                btn.Tag = table;

                switch (table.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.Blue;
                        break;
                }

                flpTable.Controls.Add(btn); 
            }
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            int discount = (int)nmDiscount.Value;

            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }
            totalPrice = totalPrice - (totalPrice / 100) * discount;
            CultureInfo culture = new CultureInfo("vi-VN"); //đổi currency theo khu vực

            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        #endregion

        #region Events
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile m = new fAccountProfile();
            m.ShowDialog(); 
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin m = new fAdmin();
            m.ShowDialog();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
            {
                return;
            }
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetBillUncheckIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if(idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);

            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetBillUncheckIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho bàn {0}?",table.Name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount);
                    ShowBill(table.ID );

                    LoadTable();
                }
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            ShowBill(table.ID);
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;

            if (MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển bản từ bàn {0} sang bàn {1} không?", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name),"Thông báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }

        }
        #endregion

    }
}
