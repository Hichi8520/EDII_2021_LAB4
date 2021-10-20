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
        private string K1;
        private string K2;
        private int[] P10;
        private int[] P8;
        private int[] P4;
        private int[] EP;
        private int[] IP;
        private int[] IPINV;
        private bool cifrando;
        private bool descifrando;

        private string[,,] Sbox1 = { { { "01" }, { "00" }, { "11"  }, { "10" } },
                                     { { "11" }, { "10" }, { "01"  }, { "00" } },
                                     { { "00" }, { "10" }, { "01"  }, { "11" } },
                                     { { "11" }, { "01" }, { "11"  }, { "10" } } };

        private string[,,] Sbox2 = { { { "00" }, { "01" }, { "10"  }, { "11" } },
                                     { { "10" }, { "00" }, { "01"  }, { "11" } },
                                     { { "11" }, { "00" }, { "01"  }, { "00" } },
                                     { { "10" }, { "01" }, { "00"  }, { "11" } } };

        public Sdes(string rutaPermutaciones)
        {
            //try
            //{
                string[] permutaciones = File.ReadAllLines(rutaPermutaciones);

                P10 = permutaciones[0].Split(',').Select(int.Parse).ToArray();
                P8 = permutaciones[1].Split(',').Select(int.Parse).ToArray();
                P4 = permutaciones[2].Split(',').Select(int.Parse).ToArray();
                EP = permutaciones[3].Split(',').Select(int.Parse).ToArray();
                IP = permutaciones[4].Split(',').Select(int.Parse).ToArray();

                IPINV = new int[8];
                for (int i = 1; i <= 8; i++) IPINV[i-1] = Array.IndexOf(IP, i);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
        }

        public bool Cifrar(string rutaArchivo, string rutaLlave, string rutaCifrado, string nombreArchivo)
        {
            //try
            //{
                cifrando = true;
                descifrando = false;

                // LEER ARCHIVO CON LLAVE
                int llaveInt = Convert.ToInt32(File.ReadAllText(rutaLlave));
                llave = Convert.ToString(llaveInt, 2);
                llave = llave.PadLeft(10, '0');
                Console.WriteLine("Llave: " + llave);

                if (llaveInt < 0 || llaveInt > 1023) return false;

                GenerarLlaves();

                escribirArchivoResultante(rutaArchivo, rutaCifrado, nombreArchivo);
                return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    return false;
            //}
        }

        public bool Descifrar(string rutaCifrado, string rutaLlave, string rutaDescifrado, string nombreArchivo)
        {
            //try
            //{
                cifrando = false;
                descifrando = true;

                // LEER ARCHIVO CON LLAVE
                int llaveInt = Convert.ToInt32(File.ReadAllText(rutaLlave));
                llave = Convert.ToString(llaveInt, 2);
                llave = llave.PadLeft(10, '0');
                Console.WriteLine("Llave: " + llave);

                if (llaveInt < 0 || llaveInt > 1023) return false;

                GenerarLlaves();

                escribirArchivoResultante(rutaCifrado, rutaDescifrado, nombreArchivo);
                return true;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    return false;
            //}
        }

        private void GenerarLlaves()
        {
            string strP10 = FuncP10(llave);

            string MitadInicialShift = LeftShift1(strP10.Substring(0,5));
            string MitadFinalShift = LeftShift1(strP10.Substring(5,5));

            K1 = FuncP8(MitadInicialShift + MitadFinalShift);

            MitadInicialShift = LeftShift2(MitadInicialShift);
            MitadFinalShift = LeftShift2(MitadFinalShift);

            K2 = FuncP8(MitadInicialShift + MitadFinalShift);
        }

        private void escribirArchivoResultante(string rutaOrigen, string rutaDestino, string nombreArchivo)
        {
            //try
            //{
                using var Fs = new FileStream(rutaOrigen, FileMode.Open);
                using BinaryReader Br = new BinaryReader(Fs);
                string extension = "";
                if (cifrando) extension = ".sdes";
                else if (descifrando) extension = ".txt";
                using FileStream writeStream = new FileStream($"{rutaDestino}/{nombreArchivo}{extension}", FileMode.OpenOrCreate);
                using BinaryWriter Bw = new BinaryWriter(writeStream);

                var byteBuffer = new byte[buffer];
                List<byte> WriteBuffer = new List<byte>();

                while (Br.BaseStream.Position != Br.BaseStream.Length)
                {
                    byteBuffer = Br.ReadBytes(buffer);
                    WriteBuffer = new List<byte>();
                    foreach (var item in byteBuffer)
                    {
                        string caracterTemp = Convert.ToString(item, 2).PadLeft(8, '0');
                        caracterTemp = FuncIP(caracterTemp);

                        if (cifrando) caracterTemp = CodificacionIntermedia(caracterTemp, K1);
                        else if (descifrando) caracterTemp = CodificacionIntermedia(caracterTemp, K2);

                        caracterTemp = Swap(caracterTemp);

                        if (cifrando) caracterTemp = CodificacionIntermedia(caracterTemp, K2);
                        else if (descifrando) caracterTemp = CodificacionIntermedia(caracterTemp, K1);

                        caracterTemp = FuncIPINV(caracterTemp);
                        WriteBuffer.Add((byte)Convert.ToInt32(caracterTemp, 2));
                    }
                    Bw.Write(WriteBuffer.ToArray());
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
        }

        // ******************************* FUNCIONES PROCESO DE CIFRADO *******************************

        private string CodificacionIntermedia(string stringIP, string K)
        {
            string mitadInicial = stringIP.Substring(0,4);
            string mitadFinal = stringIP.Substring(4, 4);

            string expMitad = FuncEP(mitadFinal);

            expMitad = XOR(expMitad, K);

            string mitadInicial_expMitad = expMitad.Substring(0,4);
            string mitadFinal_expMitad = expMitad.Substring(4,4);

            string PrimeraSbox = FuncSbox(mitadInicial_expMitad, Sbox1);
            string SegundaSbox = FuncSbox(mitadFinal_expMitad, Sbox2);

            string UnionSbox = FuncP4(PrimeraSbox + SegundaSbox);

            mitadInicial = XOR(mitadInicial, UnionSbox);

            return mitadInicial + mitadFinal;
        }

        private string XOR(string strArr1, string strArr2)
        {
            string formateado = "";

            for (int i = 0; i < strArr1.Length; i++)
            {
                if (strArr1[i] == strArr2[i])
                {
                    formateado += '0';
                }
                else
                {
                    formateado += '1';
                }
            }
            return formateado;
        }

        private string Swap(string strArr)
        {
            return strArr.Substring(4,4) + strArr.Substring(0,4);
        }

        // ******************************* FUNCIONES DE PERMUTACIONES *******************************

        private string FuncP10(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 10; i++)
            {
                formateado += stringArr[P10[i]-1];
            }
            return formateado;
        }

        private string FuncP8(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[P8[i]-1];
            }
            return formateado;
        }

        private string FuncP4(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 4; i++)
            {
                formateado += stringArr[P4[i]-1];
            }
            return formateado;
        }

        private string FuncEP(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[EP[i]-1];
            }
            return formateado;
        }

        private string FuncIP(string stringArr)
        {
            string formateado = "";

            for (int i = 0; i < 8; i++)
            {
                formateado += stringArr[IP[i]-1];
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

        // ******************************* FUNCIONES SHIFT *******************************

        private string LeftShift1(string stringArr)
        {
            return stringArr.Substring(1, 4) + stringArr.Substring(0, 1);
        }

        private string LeftShift2(string stringArr)
        {
            return stringArr.Substring(2, 3) + stringArr.Substring(0, 2);
        }

        // ******************************* FUNCIÓN DE SWAP BOXES *******************************

        private string FuncSbox(string strArr, string[,,] Sbox)
        {
            int Fila = Convert.ToInt32(strArr.Substring(0,1) + strArr.Substring(3,1), 2);
            int Columna = Convert.ToInt32(strArr.Substring(1, 1) + strArr.Substring(2, 1), 2);
            return Sbox[Fila, Columna, 0] + Sbox[Fila, Columna, 0];
        }
    }
}
