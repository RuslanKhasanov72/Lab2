using System;
using System.Diagnostics;
using System.Xml;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;

namespace Lab2 
{
    internal class Program
    {
        static void Main(string[] args)
        {
                InfoFileRead infoFile = new InfoFileRead();
                infoFile.FileRead();
                Console.WriteLine("Введите путь к приложению:");
                string path = Console.ReadLine();
                infoFile.RunApp(path);
           
        }
    }
}