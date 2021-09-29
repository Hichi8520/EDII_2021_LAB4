using System;
using System.Collections.Generic;
using System.IO;

namespace Library_LAB4
{
    public class ZigZag : ICifrado
    {
        public static int buffer = 255;
        public static int llave = 0;

        public bool Cifrar(string rutaArchivo, string rutaLlave, string rutaCifrado, string nombreArchivo)
        {
            try
            {
                // LEER ARCHIVO CON LLAVE
                using (FileStream Fs = new FileStream(rutaLlave, FileMode.Open))
                {
                    // Leer en un buffer de bytes el archivo con la llave
                    using BinaryReader Br = new BinaryReader(Fs);
                    var bytes = new byte[10];
                    while (Br.BaseStream.Position != Br.BaseStream.Length)
                    {
                        bytes = Br.ReadBytes(buffer);
                        llave = Convert.ToInt32(bytes);

                        Console.WriteLine("Llave: " + llave);
                    }
                }

                // llenar tabla zigzag
                llenarTablaZigZag();

                escribirCifrado(rutaArchivo, rutaCifrado, nombreArchivo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private void llenarTablaZigZag()
        {

        }

        private void escribirCifrado(string rutaArchivo, string rutaCifrado, string nombreArchivo)
        {
            try
            {
                using var Fs = new FileStream(rutaArchivo, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaCifrado}/{nombreArchivo}.zz", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    byteBuffer = Br.ReadBytes(buffer);
                    foreach (var item in byteBuffer)
                    {

                    }
                }

                llave = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public bool Descifrar(string rutaCifrado, string rutaLlave, string rutaDescifrado, string nombreArchivo)
        {
            try
            {
                // LEER ARCHIVO CON LLAVE
                using (FileStream Fs = new FileStream(rutaLlave, FileMode.Open))
                {
                    // Leer en un buffer de bytes el archivo con la llave
                    using BinaryReader Br = new BinaryReader(Fs);
                    var bytes = new byte[10];
                    while (Br.BaseStream.Position != Br.BaseStream.Length)
                    {
                        bytes = Br.ReadBytes(buffer);
                        llave = Convert.ToInt32(bytes);

                        Console.WriteLine("Llave: " + llave);
                    }
                }

                // llenar tabla zigzag
                llenarTablaZigZag();

                escribirDescifrado(rutaCifrado, rutaDescifrado, nombreArchivo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private void llenarTablaCesarDescifrado()
        {

        }

        private void escribirDescifrado(string rutaCifrado, string rutaDescifrado, string nombreArchivo)
        {
            try
            {
                using var Fs = new FileStream(rutaCifrado, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaDescifrado}/{nombreArchivo}.txt", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    byteBuffer = Br.ReadBytes(buffer);
                    foreach (var item in byteBuffer)
                    {

                    }
                }

                llave = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
