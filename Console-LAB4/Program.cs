using System;
using System.IO;
using Library_LAB4;

namespace Console_LAB4
{
    class Program
    {
        static void Main(string[] args)
        {
            Cesar cesar = new Cesar();
            string nombreArchivo = "file.txt";
            string nombreLlave = "key.txt";

            // RUTAS 
            string rutaArchivo = $@"C:\Users\hichi\Desktop\Lab4\Cesar\{nombreArchivo}";
            string rutaLlave = $@"C:\Users\hichi\Desktop\Lab4\Cesar\{nombreLlave}";
            string rutaCifrado = @"C:\Users\hichi\Desktop\Lab4\Cesar\";

            // CIFRAR
            string[] fileName = nombreArchivo.Split(".");
            cesar.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, fileName);
            Console.WriteLine($@"Archivo cifrado en la ruta: {rutaCifrado}{fileName[0]}.csr");

            // DESCIFRAR
            cesar.Descifrar($@"C:\Users\hichi\Desktop\Lab4\Cesar\{fileName[0]}.csr", @"C:\Users\hichi\Desktop\Lab4\Cesar\");
            Console.WriteLine($@"Archivo descifrado en la ruta: {rutaCifrado}{fileName[0]}.csr");

            // BORRAR ARCHIVO CIFRADO
            //File.Delete($@"C:\Users\hichi\Desktop\Lab4\Cesar\{fileName[0]}.csr");
        }
    }
}
