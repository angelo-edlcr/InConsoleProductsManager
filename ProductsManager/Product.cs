using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductsManager
{
    public class Product
    {
        const string file = "myProducts.txt";
        static readonly List<Product> Products = new();

        public static void Initialize()
        {
            if (!File.Exists(file)) File.Create(file);

            var lines = File.ReadAllLines(file).ToList();
            foreach (var line in lines)
            {
                string[] field = line.Split('|');

                Product p = new(
                    //Convert.ToInt32(items[0]),
                    field[0].ToString(),
                    field[1].ToString(),
                    field[2].ToString(),
                    Convert.ToDouble(field[3]),
                    Convert.ToInt32(field[4]));

                Products.Add(p);
            }
            UpdateFile(file);
        }


        //public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

        public static void UpdateFile(string file)
        {
            StringBuilder sb = new();
            foreach (var product in Products)
            {
                sb.AppendLine(product.ToString());
            }
            File.WriteAllText(file, sb.ToString());
        }
        
        public Product(string code, string name, string category, double price, int stock)
        {
            //Id = id;
            Code = code;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
        }

        public override string ToString()
        {
            return $"{Code}|{Name}|{Category}|{Price}|{Stock}";
        }

        public static bool ExecuteOption()
        {
            int op = -1;
            do
            {
                Console.Write("> ");
                try
                {
                    op = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("** INSERT A VALID VALUE **");
                }
            } while (true);
            Console.Clear();

            switch (op)
            {
                case 1:
                    AddProducts();
                    break;

                case 2:
                    GetProducts();
                    break;

                case 3:
                    GetProductByCode();
                    break;

                case 4:
                    UpdateProduct();
                    break;

                case 5:
                    DeleteProduct();
                    break;

                case 6:
                    DeleteAllProducts();
                    break;

                case 0:
                    return false;

                default:
                    Console.WriteLine("** CHOOSE A VALID OPTION **");
                    Console.Read();
                    break;
            }
            Console.WriteLine("======================\n");
            UpdateFile(file);
            if (op != 0) Continue();

            return true;
        }


        public static void AddProducts()
        {
            Console.WriteLine("== ADD NEW PRODUCT ==");
            var products = Products;
            int actualSize = products.Count;

            Console.Write("Code: ");
            string code = Console.ReadLine().ToUpper();

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Category: ");
            string category = Console.ReadLine();

            double price;
            do
            {
                Console.Write("Price: ");
                try
                {
                    price = Convert.ToDouble(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("** INSERT A CORRECT VALUE **");
                }
            } while (true);

            int stock;
            do
            {
                Console.Write("Stock: ");
                try
                {
                    stock = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("** INSERT A CORRECT VALUE **");
                }
            } while (true);

            Product product = new(code, name, category, price, stock);
            SaveInFile(product);
            products.Add(product);

            if (actualSize >= products.Count)
            {
                Console.WriteLine("** PRODUCT CANNOT BE ADDED **");
            }
            else Console.WriteLine("== PRODUCT HAS BEEN ADDED ==");
        }

        public static void UpdateProduct()
        {
            Console.WriteLine("== UPDATE A PRODUCT ==");
            var products = Products;
            var code = GetCodeToFind();
            Console.WriteLine();

            Product product = products.Find(p => p.Code == code);
            int index = products.IndexOf(product);

            Console.Write("-- ACTUAL DATA --\n");
            PrintProduct(product);

            Console.Write("-- NEW DATA --\n");
            Console.Write($"Code: {code}\n");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            if (name.Equals(string.Empty)) name = product.Name;

            Console.Write("Category: ");
            string category = Console.ReadLine();
            if (category.Equals(string.Empty)) category = product.Category;

            double price = -1;
            do
            {
                Console.Write("Price: ");
                try
                {
                    price = Convert.ToDouble(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    if (price < 0)
                    {
                        price = product.Price;
                        break;
                    }
                    Console.WriteLine("** INSERT A CORRECT VALUE **");
                }
            } while (true);

            int stock = -1;
            do
            {
                Console.Write("Stock: ");
                try
                {
                    stock = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception)
                {
                    if (stock < 0)
                    {
                        stock = product.Stock;
                        break;
                    }
                    Console.WriteLine("** INSERT A CORRECT VALUE **");
                }
            } while (true);

            product.Name = name;
            product.Category = category;
            product.Price = price;
            product.Stock = stock;
        }

        public static void GetProducts()
        {
            Console.WriteLine("== ALL PRODUCTS ==");
            if (!File.Exists(file) || File.ReadAllLines(file).ToString().Equals(string.Empty))
            {
                Console.WriteLine("** THERE ARE NO PRODUCTS **");
                return;
            }

            var lines = File.ReadAllLines(file).ToList();
            foreach (var line in lines)
            {
                string[] data = line.Split('|');
                Product p = new(data[0], data[1], data[2], Convert.ToDouble(data[3]), Convert.ToInt32(data[4]));
                PrintProduct(p);
            }
        }
        
        public static void GetProductByCode()
        {
            var products = Products;

            Console.WriteLine("== GET A PRODUCT ==");
            var code = GetCodeToFind();
            Console.WriteLine();

            if (products.Exists(p => p.Code == code))
            {
                var product = products.Find(p => p.Code == code);
                PrintProduct(product);
                return;
            }

            Console.WriteLine("** THIS PRODUCT DOESN'T EXISTS **");
        }

        public static void DeleteProduct()
        {
            var products = Products;
            Console.WriteLine("== DELETE A PRODUCT ==");
            var code = GetCodeToFind();

            if (products.Exists(p => p.Code == code))
            {
                products.RemoveAll(p => p.Code == code);
                Console.WriteLine("\n** PRODUCT DELETED **");
                return;
            }

            Console.WriteLine("** THE PRODUCT NOT EXISTS **");
        }

        public static void DeleteAllProducts()
        {
            var products = Products;

            Console.WriteLine("== DELETE ALL PRODUCTS ==");
            Console.WriteLine("Are you sure you want to DELETE ALL the products?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            Console.Write("> ");
            var op = Console.ReadLine();

            switch (op)
            {
                case "1":
                    products.Clear();
                    File.WriteAllText(file, string.Empty);
                    Console.WriteLine("== ALL PRODUCTS HAS BEEN DELETED ==");
                    break;

                default:
                    Console.WriteLine("** OPERATION CANCELED **");
                    break;
            }
        }

        
        public static string GetCodeToFind()
        {
            Console.WriteLine("Code to find: ");
            Console.Write("> ");
            return Console.ReadLine().ToString().ToUpper();
        }
        public static void PrintProduct(Product product)
        {
            //Console.WriteLine($"ID: {product.Id, -20}");
            Console.WriteLine($"CODE: {product.Code,-20}");
            Console.WriteLine($"NAME: {product.Name,-20}");
            Console.WriteLine($"CATEGORY: {product.Category,-20}");
            Console.WriteLine($"PRICE: {product.Price,-20}");
            Console.WriteLine($"STOCK: {product.Stock,-20}\n");
            Console.WriteLine("---------------------------------\n");
        }
        public static void SaveInFile(Product product)
        {
            using var writer = File.AppendText(file);
            if (!File.Exists(file))
            {
                File.Create(file);
            }

            writer.WriteLine(product.ToString());
        }
        

        static void Continue()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }
    }
}
