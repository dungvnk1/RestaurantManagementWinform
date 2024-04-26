using RestaurantManagement.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            set => FoodDAO.instance = value;
        }

        private FoodDAO() { }

        public List<Food> GetListFoodByCategoryID(int categoryID)
        {
            List<Food> list = new List<Food> ();

            string query = "SELECT * FROM Food WHERE idCategory = " + categoryID;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }

            return list;
        } 
    }
}
