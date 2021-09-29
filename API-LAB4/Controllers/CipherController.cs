using System;
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
    public class CipherController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            // Se crean los directorios y archivos que servirán de apoyo para la compresión y descompresión
            var path = _env.ContentRootPath;
            path = Path.Combine(path, "Files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string pathCesar = Path.Combine(path, "Cesar");
            if (!Directory.Exists(pathCesar))
            {
                Directory.CreateDirectory(pathCesar);
            }
            string pathZigZag = Path.Combine(path, "ZigZag");
            if (!Directory.Exists(pathZigZag))
            {
                Directory.CreateDirectory(pathZigZag);
            }

            // Mensaje de bienvenida
            string mensaje = "Laboratorio 4 - Inicio Exitoso...\n * Cifrar un archivo - Suba un archivo de texto a cifrar además de otro archivo de texto con la clave en la ruta '/api/cipher/{method}' \n * Descifrar un archivo - Suba un archivo .csr o .zz más el archivo .txt con la clave a la ruta '/api/descipher' y se le devolverá el archivo descifrado";
            return mensaje;
        }

        private IHostingEnvironment _env;

        public CipherController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("{method}")]
        [HttpPost]
        public async Task<ActionResult> CifrarArchivo([FromForm] IFormFile file, [FromForm] IFormFile key, string method)
        {
            if(method.Equals("cesar"))
            {
                try
                {
                    // Escribir archivos subidos hacia el servidor para trabajar con ellos
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    string pathCesar = Path.Combine(path, "Cesar");


                    if (System.IO.File.Exists($"{pathCesar}/{file.FileName}.txt"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{file.FileName}.txt");
                    }

                    using var saverArchivo = new FileStream($"{pathCesar}/{file.FileName}.txt", FileMode.OpenOrCreate);
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
                    string rutaArchivo = $"{pathCesar}/{file.FileName}.txt";
                    string rutaLlave = $"{pathCesar}/{key.FileName}.txt";
                    string rutaCifrado = pathCesar;

                    Cesar cesar = new Cesar();
                    cesar.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, file.FileName);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaCifrado}/{file.FileName}.csr", "text/plain", $"{file.FileName}.csr");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            else if (method.Equals("zigzag"))
            {
                try
                {
                    // Escribir archivos subidos hacia el servidor para trabajar con ellos
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    string pathCesar = Path.Combine(path, "Cesar");


                    if (System.IO.File.Exists($"{pathCesar}/{file.FileName}.txt"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{file.FileName}.txt");
                    }

                    using var saverArchivo = new FileStream($"{pathCesar}/{file.FileName}.txt", FileMode.OpenOrCreate);
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
                    string rutaArchivo = $"{pathCesar}/{file.FileName}.txt";
                    string rutaLlave = $"{pathCesar}/{key.FileName}.txt";
                    string rutaCifrado = pathCesar;

                    Cesar cesar = new Cesar();
                    cesar.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, file.FileName);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaCifrado}/{file.FileName}.csr", "text/plain", $"{file.FileName}.csr");
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