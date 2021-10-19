using System;
using System.Collections.Generic;
using System.Text;

namespace Library_LAB4
{
    interface ICifradoSDES
    {
        bool Cifrar(string rutaArchivo, string rutaCifrado, string nombreArchivo, IDictionary<string, string[]> permutaciones);
        bool Descifrar(string rutaCifrado, string rutaDescifrado, string nombreArchivo, IDictionary<string, string[]> permutaciones);
    }
}
