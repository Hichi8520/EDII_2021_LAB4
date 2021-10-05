using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                llave = Convert.ToInt32(File.ReadAllText(rutaLlave));
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
                using FileStream writeStream = new FileStream($"{rutaCifrado}/{nombreArchivo}.zz", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];
                string[] ZigZag = new string[llave];
                ZigZag.Initialize();

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {

                    int actual = 0;
                    while (Br.BaseStream.Position != Br.BaseStream.Length && actual < llave - 1)
                    {
                        ZigZag[actual] += Br.ReadChar();
                        actual++;
                    }

                    while (Br.BaseStream.Position != Br.BaseStream.Length && actual > 0)
                    {
                        ZigZag[actual] += Br.ReadChar();
                        actual--;
                    }
                }

                for (int i = 0; i < llave; i++) if (ZigZag[i] != null) Bw.Write(Encoding.UTF8.GetBytes(ZigZag[i]));

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
                llave = Convert.ToInt32(File.ReadAllText(rutaLlave));
                Console.WriteLine("Llave: " + llave);

                escribirDescifrado(rutaCifrado, rutaDescifrado, nombreArchivo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private string[] DescifrarTexto(BinaryReader br, int llave, int CharsTotales)
        {
            string[] ZigZag = new string[llave];
            ZigZag.Initialize();

            int n = (int)Math.Ceiling((double)((CharsTotales - 1.0) / (2 * (llave - 1))) + 1);
            int CharsOmitidos = (int)(n + (llave - 2) * (2 * (n - 1)) + (n - 1) - CharsTotales);


            //Fila superior || If Chars Omitidos != 0 |=>| Llenar Top - 1, else Llenar Top
            if (CharsOmitidos != 0) ZigZag[0] = GetString(br.ReadChars(n - 1));
            else ZigZag[0] = GetString(br.ReadChars(n));


            //Si la llave es mayor a 2, y chars omitidos >= 2 |=>| Llenar segunda fila - 1
            if (llave > 2)
            {
                if (CharsOmitidos >= 2) ZigZag[1] = GetString(br.ReadChars(2 * n - 3));
                else ZigZag[1] = GetString(br.ReadChars(2 * n - 2));
            }

            //Si la llave es mayor a 3, ver si la línea "i" necesita ser llenada (completo || completo - 1 || completo - 2) 
            if (llave > 3)
            {
                for (int i = 2; i < llave - 1; i++)
                {
                    //Evaluar si saltar caracteres o no
                    if ((llave - CharsOmitidos < 0) || CharsOmitidos >= (i + 1))//Si es negativo en todas las lineas hay que saltar uno o dos
                    {
                        //Evaluar si se debe saltar una o dos /*If true |=>| dos, else |=>| una*/
                        if (CharsOmitidos >= (llave + (llave - (i + 1)))) ZigZag[i] = GetString(br.ReadChars(2 * n - 4));
                        else ZigZag[i] = GetString(br.ReadChars(2 * n - 3));
                    }
                    else ZigZag[i] = GetString(br.ReadChars(2 * n - 2));
                }
            }


            //Fila inferior, If CharsOmitidos < Key |=>| llenar última, Else llenar última - 1
            if (CharsOmitidos > llave) ZigZag[llave - 1] = GetString(br.ReadChars(n - 2));
            else ZigZag[llave - 1] = GetString(br.ReadChars(n - 1));

            return ZigZag;
        }

        private string GetString(char[] CharArray)
        {
            string strg = "";
            foreach (var item in CharArray) strg += item;
            return strg;
        }

        private void escribirDescifrado(string rutaCifrado, string rutaDescifrado, string nombreArchivo)
        {
            try
            {
                int TotalChars = File.ReadAllText(rutaCifrado).Length;
                using var Fs = new FileStream(rutaCifrado, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaDescifrado}/{nombreArchivo}.txt", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                string[] ZigZag;
                ZigZag = DescifrarTexto(Br, llave, TotalChars);

                int current = 0;
                while (ZigZag[current].Length != 0)
                {
                    current = 0;
                    while (ZigZag[current].Length != 0 && current < llave - 1)
                    {
                        char TEST_CurrentByte = (ZigZag[current][0]);
                        Bw.Write((byte)(ZigZag[current][0]));
                        ZigZag[current] = ZigZag[current].Substring(1);
                        current++;
                    }

                    while (ZigZag[current].Length != 0 && current > 0)
                    {
                        char TEST_CurrentByte = (ZigZag[current][0]);
                        Bw.Write((byte)(ZigZag[current][0]));
                        ZigZag[current] = ZigZag[current].Substring(1);
                        current--;
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
