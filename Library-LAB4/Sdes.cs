using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Library_LAB4
{
    public class Sdes : ICifrado
    {
        public static int buffer = 255;
        public static string llave = "";
        private int[] P10;
        private int[] P8;
        private int[] P4;
        private int[] EP;
        private int[] IP;
        private int[] IPINV;
        Dictionary<int, char> value = new Dictionary<int, char>();

        public Sdes(string rutaPermutaciones)
        {
            try
            {
                string[] permutaciones = File.ReadAllLines(rutaPermutaciones);

                P10 = permutaciones[0].Split(',').Select(int.Parse).ToArray();
                P8 = permutaciones[1].Split(',').Select(int.Parse).ToArray();
                P4 = permutaciones[2].Split(',').Select(int.Parse).ToArray();
                EP = permutaciones[3].Split(',').Select(int.Parse).ToArray();
                IP = permutaciones[4].Split(',').Select(int.Parse).ToArray();

                IPINV = new int[8];
                for (int i = 0; i < 8; i++) IPINV[i] = Array.IndexOf(IP, i);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Cifrar(string rutaArchivo, string rutaLlave, string rutaCifrado, string nombreArchivo)
        {
            try
            {
                // LEER ARCHIVO CON LLAVE
                int llaveInt = Convert.ToInt32(File.ReadAllText(rutaLlave));
                llave = Convert.ToString(llaveInt, 2);
                llave = llave.PadLeft(10, '0');
                Console.WriteLine("Llave: " + llave);

                escribirCifrado(rutaArchivo, rutaCifrado, nombreArchivo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private void escribirCifrado(string rutaArchivo, string rutaCifrado, string nombreArchivo)
        {
            try
            {
                using var Fs = new FileStream(rutaArchivo, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaCifrado}/{nombreArchivo}.sdes", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    while (Br.BaseStream.Position != Br.BaseStream.Length)
                    {
                        string binario = DecimalaBinario(Br.ReadChar(),8);
                        for(int i = 1; i <= binario.Length; i++)
                        {
                            value.Add(i, binario[i - 1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        string DecimalaBinario(int deci, int val) //convierte el decimal enviado a un binario
        {
            string binario = string.Empty;
            int residuo = 0;
            for (int x = 0; deci > 1; x++)
            {
                residuo = deci % 2;
                deci = deci / 2;
                binario = residuo.ToString() + binario;
            }

            if (deci == 1)
            {
                binario = deci.ToString() + binario;
            }
            if (binario.Length != val)
            {
                for (int d = 0; d < (val - binario.Length); deci++)
                {
                    binario = '0' + binario;
                }
            }
            return binario;
        }

        public bool Descifrar(string rutaCifrado, string rutaLlave, string rutaDescifrado, string nombreArchivo)
        {
            try
            {

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private string FuncP10(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 10; i++)
            {
                formateado += stringArr[P10[i]];
            }
            return formateado;
        }

        private string FuncP8(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[P8[i]];
            }
            return formateado;
        }

        private string FuncP4(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 4; i++)
            {
                formateado += stringArr[P4[i]];
            }
            return formateado;
        }

        private string FuncEP(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[EP[i]];
            }
            return formateado;
        }

        private string FuncIP(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[IP[i]];
            }
            return formateado;
        }

        private string FuncIPINV(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[IPINV[i]];
            }
            return formateado;
        }
    }
}
