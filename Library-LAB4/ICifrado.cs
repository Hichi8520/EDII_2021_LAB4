﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Library_LAB4
{
    interface ICifrado
    {
        bool Cifrar(string rutaArchivo, string rutaLlave, string rutaCifrado, string[] nombreArchivo);
        bool Descifrar(string rutaCifrado, string rutaLlave, string rutaDescifrado, string[] nombreArchivo);
    }
}
