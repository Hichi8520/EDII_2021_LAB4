using System;
using System.Collections.Generic;
using System.IO;

namespace Library_LAB4
{
    public class Cesar : ICifrado
    {
        public static int buffer = 255;
        public static int bufferLlave = 255;
        public List<byte> llave = new List<byte>();
        public Dictionary<byte, byte> tablaCesar = new Dictionary<byte, byte>();

        public bool Cifrar(string rutaArchivo, string rutaLlave, string rutaCifrado, string[] nombreArchivo)
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

                escribirCifrado(rutaArchivo, rutaCifrado, nombreArchivo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public string Descifrar(string rutaCifrado, string rutaDescifrado)
        {

        }

        private void escribirCifrado(string rutaArchivo, string rutaCifrado, string[] nombreArchivo)
        {
            
        }
    }
}
