using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library_LAB4
{
    public class Sdes : ICifradoSDES
    {
        public static int buffer = 255;
        public static int llave = 0;
        Dictionary<int, int> p10 = new Dictionary<int, int>();
        Dictionary<int, int> p8 = new Dictionary<int, int>();
        Dictionary<int, int> p4 = new Dictionary<int, int>();
        Dictionary<int, int> ep = new Dictionary<int, int>();
        Dictionary<int, int> ip = new Dictionary<int, int>();
        Dictionary<int, int> ipinv = new Dictionary<int, int>();
        Dictionary<int, char> value = new Dictionary<int, char>();

        public bool Cifrar(string rutaArchivo, string rutaCifrado, string nombreArchivo, IDictionary<string, string[]> permutaciones)
        {
            try
            {
                string[] p10v = default;
                permutaciones.TryGetValue("P10", out p10v);
                for (int i = 1; i <= p10v.Length; i++)
                {
                    p10.Add(i, Convert.ToInt32(p10v[i - 1]));
                }
                string[] p8v = default;
                permutaciones.TryGetValue("P8", out p8v);
                for (int i = 1; i <= p8v.Length; i++)
                {
                    p8.Add(i, Convert.ToInt32(p8v[i - 1]));
                }
                string[] p4v = default;
                permutaciones.TryGetValue("P4", out p4v);
                for (int i = 1; i <= p4v.Length; i++)
                {
                    p4.Add(i, Convert.ToInt32(p4v[i - 1]));
                }
                string[] pep = default;
                permutaciones.TryGetValue("EP", out pep);
                for (int i = 1; i <= pep.Length; i++)
                {
                    ep.Add(i, Convert.ToInt32(pep[i - 1]));
                }
                string[] pip = default;
                permutaciones.TryGetValue("IP", out pip);
                for (int i = 1; i <= pip.Length; i++)
                {
                    ip.Add(i, Convert.ToInt32(pip[i - 1]));
                }
                string[] pipv = default;
                permutaciones.TryGetValue("IP-1", out pipv);
                for (int i = 1; i <= pipv.Length; i++)
                {
                    ipinv.Add(i, Convert.ToInt32(pipv[i - 1]));
                }
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

        public bool Descifrar(string rutaCifrado, string rutaDescifrado, string nombreArchivo, IDictionary<string, string[]> permutaciones)
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
    }
}
