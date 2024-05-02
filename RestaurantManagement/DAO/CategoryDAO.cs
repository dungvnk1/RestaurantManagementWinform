using RestaurantManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            set => CategoryDAO.instance = value;
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "SELECT * FROM FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Category category = new Category(row);
                list.Add(category);
            }

            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "SELECT * FROM FoodCategory WHERE id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                category = new Category(row);
                return category;
            }

            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT FoodCategory (name) VALUES (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(int id, string name)
        {
            string query = string.Format("UPDATE FoodCategory SET name = N'{0}' WHERE id = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int idCategory)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategoryID(idCategory);
            foreach (Food item in listFood)
            {
                BillInfoDAO.Instance.DeleteBillInfoByFoodID(item.ID);
            }
            FoodDAO.Instance.DeleteFoodByCategoryID(idCategory);
            string query = string.Format("DELETE FoodCategory WHERE id = {0}", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
