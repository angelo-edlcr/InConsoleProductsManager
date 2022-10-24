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
        static void Main()
        {
            Product.Initialize();

            do
            {
                Console.Clear();
                Console.WriteLine("Select an option");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Show all");
                Console.WriteLine("3. Get one");
                Console.WriteLine("4. Update");
                Console.WriteLine("5. Delete");
                Console.WriteLine("6. Delete ALL");
                Console.WriteLine("0. Exit");

            } while (Product.ExecuteOption());
        }
    }
}
