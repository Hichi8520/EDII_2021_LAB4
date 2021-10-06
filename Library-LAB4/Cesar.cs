using System;
using System.Collections.Generic;
using System.IO;

namespace Library_LAB4
{
    public class Cesar : ICifrado
    {
        public static int buffer = 10000;
        public static int bufferLlave = 255;
        public List<byte> llave = new List<byte>();
        public Dictionary<byte, byte> tablaCesar = new Dictionary<byte, byte>(); // <bytes normales, bytes con corrimiento>

        public bool Cifrar(string rutaArchivo, string rutaLlave, string rutaCifrado, string nombreArchivo)
        {
            try
            {
                // LEER ARCHIVO CON LLAVE
                using (FileStream Fs = new FileStream(rutaLlave, FileMode.Open))
                {
                    // Leer en un buffer de bytes el archivo con la llave
                    using BinaryReader Br = new BinaryReader(Fs);
                    var bytes = new byte[buffer];
                    while (Br.BaseStream.Position != Br.BaseStream.Length)
                    {
                        bytes = Br.ReadBytes(buffer);

                        // Recorrer byte por byte en el buffer
                        foreach (var item in bytes)
                        {
                            //var value = Convert.ToString(item, 2).PadLeft(8, '0'); // Convertir a binario el byte
                            if (!llave.Contains(item))
                            {
                                llave.Add(item);
                            }
                        }
                    }
                }

                // llenar tabla cesar con el corrimiento
                llenarTablaCesarCifrado();

                escribirCifrado(rutaArchivo, rutaCifrado, nombreArchivo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private void llenarTablaCesarCifrado()
        {
            int indice = 0;
            foreach (var item in llave)
            {
                tablaCesar.Add(Convert.ToByte(indice), item);
                indice++;
            }

            for (int i = 0; i < bufferLlave; i++) // i < 255
            {
                if(!tablaCesar.ContainsValue(Convert.ToByte(i)))
                {
                    tablaCesar.Add(Convert.ToByte(indice), Convert.ToByte(i));
                    indice++;
                }
            }
        }

        private void escribirCifrado(string rutaArchivo, string rutaCifrado, string nombreArchivo)
        {
            try
            {
                using var Fs = new FileStream(rutaArchivo, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaCifrado}/{nombreArchivo}.csr", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    byteBuffer = Br.ReadBytes(buffer);
                    foreach (var item in byteBuffer)
                    {
                        // Intercambiar byte del texto por byte del diccionario con corrimiento
                        var caracter = tablaCesar[item];
                        Bw.Write(caracter);
                    }
                }

                tablaCesar.Clear();
                llave.Clear();
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
                    var bytes = new byte[buffer];
                    while (Br.BaseStream.Position != Br.BaseStream.Length)
                    {
                        bytes = Br.ReadBytes(buffer);

                        // Recorrer byte por byte en el buffer
                        foreach (var item in bytes)
                        {
                            //var value = Convert.ToString(item, 2).PadLeft(8, '0'); // Convertir a binario el byte
                            if (!llave.Contains(item))
                            {
                                llave.Add(item);
                            }
                        }
                    }
                }

                // llenar tabla cesar con el corrimiento
                llenarTablaCesarDescifrado();

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
            int indice = 0;
            foreach (var item in llave)
            {
                tablaCesar.Add(item, Convert.ToByte(indice));
                indice++;
            }

            for (int i = 0; i < bufferLlave; i++) // i < 255
            {
                if (!tablaCesar.ContainsKey(Convert.ToByte(i)))
                {
                    tablaCesar.Add(Convert.ToByte(i), Convert.ToByte(indice));
                    indice++;
                }
            }
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
                        // Intercambiar byte del texto por byte del diccionario con corrimiento
                        var caracter = tablaCesar[item];
                        Bw.Write(caracter);
                    }
                }

                tablaCesar.Clear();
                llave.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
