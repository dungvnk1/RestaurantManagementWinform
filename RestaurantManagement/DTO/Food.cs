﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.DTO
{
    public class Food
    {
        private int iD;
        private string name;
        private int categoryID;
        private float price;

        public Food(int iD, string name, int categoryID, float price)
        {
            ID = iD;
            Name = name;
            CategoryID = categoryID;
            Price = price;
        }

        public Food(DataRow row)
        {
            ID = (int)row["id"];
            Name = row["name"].ToString();
            CategoryID = (int)row["idCategory"];
            Price = (float)Convert.ToDouble(row["price"].ToString());
        }

        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public float Price { get => price; set => price = value; }
    }
}
