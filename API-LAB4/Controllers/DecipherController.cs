﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library_LAB4;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LAB4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecipherController : ControllerBase
    {
        private IHostingEnvironment _env;

        public DecipherController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public async Task<ActionResult> DecompressFile([FromForm] IFormFile file, [FromForm] IFormFile key)
        {
            string[] archivo = file.FileName.Split('.');
            if(archivo[1].Equals("csr"))
            {
                try
                {
                    // Escribir archivos subidos hacia el servidor para trabajar con ellos
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    string pathCesar = Path.Combine(path, "Cesar");


                    if (System.IO.File.Exists($"{pathCesar}/{file.FileName}.csr"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{file.FileName}.csr");
                    }

                    using var saverArchivo = new FileStream($"{pathCesar}/{file.FileName}.csr", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverArchivo);
                    saverArchivo.Close();

                    if (System.IO.File.Exists($"{pathCesar}/{key.FileName}.txt"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{key.FileName}.txt");
                    }

                    using var saverLave = new FileStream($"{pathCesar}/{key.FileName}.txt", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverLave);
                    saverLave.Close();


                    // Proceso Cifrado César
                    string rutaCifrado = $"{pathCesar}/{file.FileName}.csr";
                    string rutaLlave = $"{pathCesar}/{key.FileName}.txt";
                    string rutaDescifrado = pathCesar;

                    Cesar cesar = new Cesar();
                    cesar.Descifrar(rutaCifrado, rutaLlave, rutaDescifrado, file.FileName);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaDescifrado}/{file.FileName}.txt", "text/plain", $"{file.FileName}.txt");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            else if (archivo[1].Equals("zz"))
            {
                try
                {
                    // Escribir archivos subidos hacia el servidor para trabajar con ellos
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    string pathCesar = Path.Combine(path, "Cesar");


                    if (System.IO.File.Exists($"{pathCesar}/{file.FileName}.csr"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{file.FileName}.csr");
                    }

                    using var saverArchivo = new FileStream($"{pathCesar}/{file.FileName}.csr", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverArchivo);
                    saverArchivo.Close();

                    if (System.IO.File.Exists($"{pathCesar}/{key.FileName}.txt"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{key.FileName}.txt");
                    }

                    using var saverLave = new FileStream($"{pathCesar}/{key.FileName}.txt", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverLave);
                    saverLave.Close();


                    // Proceso Cifrado César
                    string rutaCifrado = $"{pathCesar}/{file.FileName}.csr";
                    string rutaLlave = $"{pathCesar}/{key.FileName}.txt";
                    string rutaDescifrado = pathCesar;

                    Cesar cesar = new Cesar();
                    cesar.Descifrar(rutaCifrado, rutaLlave, rutaDescifrado, file.FileName);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaDescifrado}/{file.FileName}.txt", "text/plain", $"{file.FileName}.txt");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            else
            {
                return StatusCode(400, "Bad request");
            }
        }
    }
}