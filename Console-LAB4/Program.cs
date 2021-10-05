using System;
using System.IO;
using Library_LAB4;

namespace Console_LAB4
{
    class Program
    {
        static void Main(string[] args)
        {
            string nombreArchivo = "file.txt";
            string nombreLlave = "key.txt";

            //************************************* CÉSAR *************************************

            //Cesar cesar = new Cesar();

            //// RUTAS 
            //string rutaArchivoCsr = $@"C:\Users\hichi\Desktop\Lab4\Cesar\{nombreArchivo}";
            //string rutaLlaveCsr = $@"C:\Users\hichi\Desktop\Lab4\Cesar\{nombreLlave}";
            //string rutaCifradoCsr = @"C:\Users\hichi\Desktop\Lab4\Cesar\";

            //// CIFRAR
            //string[] fileNameCsr = nombreArchivo.Split(".");
            //cesar.Cifrar(rutaArchivoCsr, rutaLlaveCsr, rutaCifradoCsr, fileNameCsr[0]);
            //Console.WriteLine($@"César: Archivo cifrado en la ruta: {rutaCifradoCsr}{fileNameCsr[0]}.csr");

            //// DESCIFRAR
            //cesar.Descifrar($@"C:\Users\hichi\Desktop\Lab4\Cesar\{fileNameCsr[0]}.csr", rutaLlaveCsr, @"C:\Users\hichi\Desktop\Lab4\Cesar\", fileNameCsr[0]);
            //Console.WriteLine($@"César: Archivo descifrado en la ruta: {rutaCifradoCsr}{fileNameCsr[0]}.csr");

            //// BORRAR ARCHIVO CIFRADO
            ////File.Delete($@"C:\Users\hichi\Desktop\Lab4\Cesar\{fileNameCsr[0]}.csr");


            //************************************* ZIGZAG *************************************

            ZigZag zigZag = new ZigZag();

            // RUTAS 
            string rutaArchivoZz = $@"C:\Users\hichi\Desktop\Lab4\ZigZag\{nombreArchivo}";
            string rutaLlaveZz = $@"C:\Users\hichi\Desktop\Lab4\ZigZag\{nombreLlave}";
            string rutaCifradoZz = @"C:\Users\hichi\Desktop\Lab4\ZigZag\";

            // CIFRAR
            string[] fileNameZz = nombreArchivo.Split(".");
            zigZag.Cifrar(rutaArchivoZz, rutaLlaveZz, rutaCifradoZz, fileNameZz[0]);
            Console.WriteLine($@"ZigZag: Archivo cifrado en la ruta: {rutaCifradoZz}{fileNameZz[0]}.zz");

            // DESCIFRAR
            zigZag.Descifrar($@"C:\Users\hichi\Desktop\Lab4\ZigZag\{fileNameZz[0]}.zz", rutaLlaveZz, @"C:\Users\hichi\Desktop\Lab4\ZigZag\", fileNameZz[0]);
            Console.WriteLine($@"ZigZag: Archivo descifrado en la ruta: {rutaCifradoZz}{fileNameZz[0]}.zz");

            // BORRAR ARCHIVO CIFRADO
            //File.Delete($@"C:\Users\hichi\Desktop\Lab4\ZigZag\{fileNameZz[0]}.zz");
        }
    }
}
