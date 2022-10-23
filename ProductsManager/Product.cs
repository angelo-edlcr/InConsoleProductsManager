using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsManager
{
    public class Product
    {
        //public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

        public Product(string code, string name, string category, double price, int stock)
        {
            //Id = id;
            Code = code;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
        }

        // Override the ToString() method to a format for save it in the .txt file
        public override string ToString()
        {
            return $"{Code}|{Name}|{Category}|{Price}|{Stock}";
        }
    }
}
