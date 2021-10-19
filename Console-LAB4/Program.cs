using System;
using System.IO;
using Library_LAB4;
using System.Collections.Generic;

namespace Console_LAB4
{
    class Program
    {
        static void Main(string[] args)
        {
            //************************************* CÉSAR *************************************

            //Cesar cesar = new Cesar();
            string basePath = Environment.CurrentDirectory;
            string nombreArchivo = "../../../../files/file.txt";
            //string nombreLlave = "../../../../files/key.txt";
            //// RUTAS 
            //string rutaArchivo = Path.GetFullPath(nombreArchivo, basePath);
            //string rutaLlave = Path.GetFullPath(nombreLlave, basePath);
            //string rutaCifrado = Path.GetFullPath("../../../../files/Cesar/", basePath);

            //// CIFRAR
            //string[] fileName = nombreArchivo.Split("/");
            //fileName = fileName[5].Split(".");
            //cesar.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, fileName[0]);
            //Console.WriteLine($@"Archivo cifrado en la ruta: {rutaCifrado}/{fileName[0]}.csr");

            //// DESCIFRAR
            //cesar.Descifrar(rutaCifrado + fileName[0] + ".csr", rutaLlave, rutaCifrado, fileName[0]);
            //Console.WriteLine($@"Archivo descifrado en la ruta: {rutaCifrado}/{fileName[0]}.txt");


            //************************************* ZIGZAG *************************************

            //ZigZag zigZag = new ZigZag();

            //// RUTAS 
            //string rutaCifradoZz = Path.GetFullPath("../../../../files/ZigZag/", basePath);

            //// CIFRAR
            //string[] fileNameZz = nombreArchivo.Split("/");
            //fileNameZz = fileNameZz[5].Split(".");
            //zigZag.Cifrar(rutaArchivo, rutaLlave, rutaCifradoZz, fileNameZz[0]);
            //Console.WriteLine($@"ZigZag: Archivo cifrado en la ruta: {rutaCifradoZz}{fileNameZz[0]}.zz");

            //// DESCIFRAR
            //zigZag.Descifrar($@"C:\Users\hichi\Desktop\Lab4\ZigZag\{fileNameZz[0]}.zz", rutaLlave, @"C:\Users\hichi\Desktop\Lab4\ZigZag\", fileNameZz[0]);
            //Console.WriteLine($@"ZigZag: Archivo descifrado en la ruta: {rutaCifradoZz}{fileNameZz[0]}.zz");

            // BORRAR ARCHIVO CIFRADO
            //File.Delete($@"C:\Users\hichi\Desktop\Lab4\ZigZag\{fileNameZz[0]}.zz");


            //************************************* SDES *************************************

            Sdes Sdes = new Sdes();
            // Tabla 
            string rutaTabla = Path.GetFullPath("../../../../files/table.txt", basePath);
            string[] lines = File.ReadAllLines(rutaTabla);
            IDictionary<string, string[]> permutaciones = new Dictionary<string, string[]>();
            string rutaArchivo = Path.GetFullPath(nombreArchivo, basePath);
            string rutaCifrado = Path.GetFullPath("../../../../files/SDES/", basePath);
            // CIFRAR
            string[] fileName = nombreArchivo.Split("/");
            fileName = fileName[5].Split(".");
            Console.WriteLine($@"Archivo cifrado en la ruta: {rutaCifrado}/{fileName[0]}.sdes");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] temp = lines[i].Split("/");
                permutaciones.Add(temp[0],temp[1].Split(","));
            }
            Sdes.Cifrar(rutaArchivo, rutaCifrado, fileName[0], permutaciones);
        }
    }
}
