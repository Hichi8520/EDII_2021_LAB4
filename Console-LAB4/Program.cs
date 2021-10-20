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
            string basePath = Environment.CurrentDirectory;
            string nombreArchivo = "../../../../files/file.txt";
            string nombreLlave = "../../../../files/key.txt";


            //************************************* SDES *************************************
            
            // Tabla 
            string rutaTabla = Path.GetFullPath("../../../../files/table.txt", basePath);

            // Configuración de permutaciones
            Sdes Sdes = new Sdes(rutaTabla);

            string rutaArchivo = Path.GetFullPath(nombreArchivo, basePath);
            string rutaLlave = Path.GetFullPath(nombreLlave, basePath);
            string rutaCifrado = Path.GetFullPath("../../../../files/SDES/", basePath);

            // CIFRAR
            string[] fileName = nombreArchivo.Split("/");
            fileName = fileName[5].Split(".");

            Sdes.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, fileName[0]);
            Console.WriteLine($@"Archivo cifrado en la ruta: {rutaCifrado}/{fileName[0]}.sdes");

            // DESCIFRAR
            Sdes.Descifrar($@"{rutaCifrado}/{fileName[0]}.sdes", rutaLlave, rutaCifrado, fileName[0]);
            Console.WriteLine($@"Archivo descifrado en la ruta: {rutaCifrado}/{fileName[0]}.txt");
        }
    }
}
