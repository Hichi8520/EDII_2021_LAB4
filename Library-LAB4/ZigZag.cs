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
            
            //try
            //{
                // LEER ARCHIVO CON LLAVE
                llave = Convert.ToInt32(File.ReadAllText(rutaLlave));
                Console.WriteLine("Llave: " + llave);

                escribirCifrado(rutaArchivo, rutaCifrado, nombreArchivo);
                return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    return false;
            //}
        }

        private void escribirCifrado(string rutaArchivo, string rutaCifrado, string nombreArchivo)
        {
            //try
            //{
                using var Fs = new FileStream(rutaArchivo, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaCifrado}/{nombreArchivo}.zz", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];
                string[] ZigZag = new string[llave];
                ZigZag.Initialize();

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    byteBuffer = Br.ReadBytes(buffer);
                    //foreach (var item in byteBuffer)
                    //{

                    //}

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

                llave = 0;

                for (int i = 0; i < llave; i++) if (ZigZag[i] != null) Bw.Write(Encoding.UTF8.GetBytes(ZigZag[i]));
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
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

        private string[] DText(BinaryReader br, int key, int TotalChars)
        {
            string[] ZigZag = new string[key];
            ZigZag.Initialize();

            int n = (int)Math.Ceiling((double)((TotalChars - 1.0) / (2 * (key - 1))) + 1);
            int MissedChars = (int)(n + (key - 2) * (2 * (n - 1)) + (n - 1) - TotalChars);


            //For Top Line || If Missing Spaces != 0 |=>| Fill Top - 1, else fill Top
            if (MissedChars != 0) ZigZag[0] = GetString(br.ReadChars(n - 1));
            else ZigZag[0] = GetString(br.ReadChars(n));


            //If the key its bigger than two, and missing characters >= 2 |=>| Fill second - 1
            if (key > 2)
            {
                if (MissedChars >= 2) ZigZag[1] = GetString(br.ReadChars(2 * n - 3));
                else ZigZag[1] = GetString(br.ReadChars(2 * n - 2));
            }

            //If the key its bigger than three, check if "i" line needs to be Fiiled (complete || complete - 1 || complete - 2) 
            if (key > 3)
            {
                for (int i = 2; i < key - 1; i++)
                {
                    //This if valuates if i need to skip characters or not
                    if ((key - MissedChars < 0) || MissedChars >= (i + 1))//If its negative that means that in all lines have to skip one or two || If missed chars in last column are more that current line
                    {
                        //This valuates if i need to skip 1 or two /*If true |=>| two, else |=>| one*/
                        if (MissedChars >= (key + (key - (i + 1)))) ZigZag[i] = GetString(br.ReadChars(2 * n - 4));
                        else ZigZag[i] = GetString(br.ReadChars(2 * n - 3));
                    }
                    else ZigZag[i] = GetString(br.ReadChars(2 * n - 2));
                }
            }


            //For Bottom Line, If MissingSpaces < Key |=>| Fill Last, Else FIll Last - 1
            if (MissedChars > key) ZigZag[key - 1] = GetString(br.ReadChars(n - 2));
            else ZigZag[key - 1] = GetString(br.ReadChars(n - 1));

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
                using var Fs = new FileStream(rutaCifrado, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                using FileStream writeStream = new FileStream($"{rutaDescifrado}/{nombreArchivo}.txt", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    byteBuffer = Br.ReadBytes(buffer);
                    //foreach (var item in byteBuffer)
                    //{

                    //}

                    // llenar tabla zigzag
                    int TotalChars = File.ReadAllText(rutaCifrado).Length;
                    string[] ZigZag;
                    ZigZag = DText(Br, llave, TotalChars);

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
