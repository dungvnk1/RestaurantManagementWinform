using RestaurantManagement.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance 
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            set => TableDAO.instance = value; 
        }

        private TableDAO() { }

        public static int TableWidth = 130;
        public static int TableHeight = 100;

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow row in data.Rows)
            {
                Table table = new Table(row);
                tableList.Add(table);
            }

            return tableList;
        }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTabel2", new object[] { id1, id2 });
        }

        public Table GetTableByID(int id)
        {
            Table table = null;

            string query = "SELECT * FROM TableFood WHERE id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                table = new Table(row);
                return table;
            }

            return table;
        }

        public Table GetLastTable()
        {
            Table table = null;

            string query = "SELECT * FROM TableFood WHERE id = (SELECT MAX(id) FROM TableFood)";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                table = new Table(row);
                return table;
            }

            return table;
        }

        public bool InsertTable()
        {
            Table table = GetLastTable();
            int number = Convert.ToInt32(table.Name.Split(' ')[1]);
            string query = string.Format("INSERT TableFood (name, status) VALUES (N'Bàn {0}', N'Trống')", number + 1);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(int id, string name, string status)
        {
            string query = string.Format("UPDATE TableFood SET name = N'{0}', status = N'{1}' WHERE id = {2}", name, status, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int id)
        {
            string query = string.Format("DELETE TableFood WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
