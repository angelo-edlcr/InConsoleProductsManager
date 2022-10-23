using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

// ANTES DE INICIAR EL PROGRAMA
// Leer los datos del archivo por lineas y colocarlos en Product
// Poner cada Product en la lista

namespace ProductsManager
{
    class Program
    {
        const string file = "myProducts.txt";
        static List<Product> Products = new();

        // Update the file for any change
        public static void UpdateFile(string file)
        {
            StringBuilder sb = new();
            foreach (var product in Products)
            {
                sb.AppendLine(product.ToString());
            }
            File.WriteAllText(file, sb.ToString());
        }

        // Initialize the product list based on the .txt file
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

        static void Main()
        {
            Initialize();

            do
            {
                Console.Clear();
                Console.WriteLine("Select an option");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Show all");
                Console.WriteLine("3. Get one");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Delete ALL");
                Console.WriteLine("0. Exit");
            Console.Write("> ");

            } while (ExecuteOption());
        }

        // Execute the selected action
        public static bool ExecuteOption()
        {
            int op = Convert.ToInt32(Console.ReadLine());
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
                    DeleteProduct();
                    break;

                case 5:
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

        // Shows a little message for a pause
        static void Continue()
        {
            Console.WriteLine("\nPress any key to continue");
            Console.ReadLine();
        }

        // Add a new product to the list
        public static void AddProducts()
        {
            Console.WriteLine("== ADD NEW PRODUCT ==");
            var products = Products;
            int actualSize = products.Count;

            //Console.Write("Id: ");
            //int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Code: ");
            string code = Console.ReadLine();

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Category: ");
            string category = Console.ReadLine();

            Console.Write("Price: ");

            double price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Stock: ");
            int stock = Convert.ToInt32(Console.ReadLine());


            Product product = new(code.ToUpper(), name, category, price, stock);
            SaveInFile(product);
            products.Add(product);

            if (actualSize >= products.Count)
            {
                Console.WriteLine("** PRODUCT CANNOT BE ADDED **");
            }
            else Console.WriteLine("== PRODUCT HAS BEEN ADDED ==");
        }

        // Show all the products saved inside the .txt file
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

        // Remove the specified product
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

        // Remove all products in the list and update the .txt file
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

        // Get a product by an specified code
        public static void GetProductByCode()
        {
            var products = Products;

            Console.WriteLine("== GET A PRODUCT ==");
            var code = GetCodeToFind();

            if (products.Exists(p => p.Code == code))
            {
                var product = products.Find(p => p.Code == code);
                PrintProduct(product);
                Console.WriteLine("=========================\n");
                return;
            }

            Console.WriteLine("** THIS PRODUCT DOESN'T EXISTS **");
        }

        // Read the code for find or delete a product
        public static string GetCodeToFind()
        {
            Console.WriteLine("Code to find: ");
            Console.Write("> ");
            return Console.ReadLine().ToString().ToUpper();
        }

        // Print the product data in the console
        public static void PrintProduct(Product product)
        {
            //Console.WriteLine($"ID: {product.Id, -20}");
            Console.WriteLine($"CODE: {product.Code, -20}");
            Console.WriteLine($"NAME: {product.Name,-20}");
            Console.WriteLine($"CATEGORY: {product.Category,-20}");
            Console.WriteLine($"PRICE: {product.Price,-20}");
            Console.WriteLine($"STOCK: {product.Stock,-20}\n");
            Console.WriteLine("---------------------------------\n");
        }

        // Save the new product in the .txt file
        public static void SaveInFile(Product product)
        {
            using var writer = File.AppendText(file);
                if (!File.Exists(file))
                {
                    File.Create(file);
                }
                
                writer.WriteLine(product.ToString());
        }
    }
}
