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
    [Route("api")]
    [ApiController]
    public class CipherController : ControllerBase
    {
        private static Sdes sdes;

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
            string mensaje = "Laboratorio 5 - Inicio Exitoso...\n * Para configurar las permutaciones del algoritmo SDES suba un archivo de texto a la ruta 'api/sdes/config' en el formato correcto \n * Cifrar un archivo - Suba un archivo de texto a cifrar además de otro archivo de texto con la clave en la ruta '/api/cipher/{method}' \n * Descifrar un archivo - Suba un archivo .csr, .zz o .sdes más el archivo .txt con la clave a la ruta '/api/descipher' y se le devolverá el archivo descifrado";
            return mensaje;
        }

        private IHostingEnvironment _env;

        public CipherController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Route("[controller]/{method}")]
        [HttpPost]
        public async Task<ActionResult> CifrarArchivo([FromForm] IFormFile file, [FromForm] IFormFile key, string method)
        {
            if (file == null || key == null) return StatusCode(400, "Bad request");

            if (method.Equals("cesar"))
            {
                try
                {
                    // Escribir archivos subidos hacia el servidor para trabajar con ellos
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    string pathCesar = Path.Combine(path, "Cesar");


                    if (System.IO.File.Exists($"{pathCesar}/{file.FileName}"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{file.FileName}");
                    }

                    using var saverArchivo = new FileStream($"{pathCesar}/{file.FileName}", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverArchivo);
                    saverArchivo.Close();

                    if (System.IO.File.Exists($"{pathCesar}/{key.FileName}"))
                    {
                        System.IO.File.Delete($"{pathCesar}/{key.FileName}");
                    }

                    using var saverLlave = new FileStream($"{pathCesar}/{key.FileName}", FileMode.OpenOrCreate);
                    await key.CopyToAsync(saverLlave);
                    saverLlave.Close();


                    // Proceso Cifrado César
                    string rutaArchivo = $"{pathCesar}/{file.FileName}";
                    string rutaLlave = $"{pathCesar}/{key.FileName}";
                    string rutaCifrado = pathCesar;
                    string[] fileName = file.FileName.Split(".");

                    Cesar cesar = new Cesar();
                    cesar.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, fileName[0]);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaCifrado}/{fileName[0]}.csr", "text/plain", $"{fileName[0]}.csr");
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

                    string pathZigZag = Path.Combine(path, "ZigZag");


                    if (System.IO.File.Exists($"{pathZigZag}/{file.FileName}"))
                    {
                        System.IO.File.Delete($"{pathZigZag}/{file.FileName}");
                    }

                    using var saverArchivo = new FileStream($"{pathZigZag}/{file.FileName}", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverArchivo);
                    saverArchivo.Close();

                    if (System.IO.File.Exists($"{pathZigZag}/{key.FileName}"))
                    {
                        System.IO.File.Delete($"{pathZigZag}/{key.FileName}");
                    }

                    using var saverLlave = new FileStream($"{pathZigZag}/{key.FileName}", FileMode.OpenOrCreate);
                    await key.CopyToAsync(saverLlave);
                    saverLlave.Close();


                    // Proceso Cifrado ZigZag
                    string rutaArchivo = $"{pathZigZag}/{file.FileName}";
                    string rutaLlave = $"{pathZigZag}/{key.FileName}";
                    string rutaCifrado = pathZigZag;
                    string[] fileName = file.FileName.Split(".");

                    ZigZag zigZag = new ZigZag();
                    zigZag.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, fileName[0]);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaCifrado}/{fileName[0]}.zz", "text/plain", $"{fileName[0]}.zz");
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

        [Route("sdes/config")]
        [HttpPost]
        public async Task<ActionResult> ConfigurarSdes([FromForm] IFormFile file)
        {
            if (file == null) return StatusCode(400, "Bad request");

            try
            {
                // Escribir archivo subido hacia el servidor para trabajar con él
                var path = _env.ContentRootPath;
                path = Path.Combine(path, "Files");

                string pathCesar = Path.Combine(path, "Sdes");


                if (System.IO.File.Exists($"{pathCesar}/{file.FileName}"))
                {
                    System.IO.File.Delete($"{pathCesar}/{file.FileName}");
                }

                using var saverArchivo = new FileStream($"{pathCesar}/{file.FileName}", FileMode.OpenOrCreate);
                await file.CopyToAsync(saverArchivo);
                saverArchivo.Close();

                if (System.IO.File.Exists($"{pathCesar}/{key.FileName}"))
                {
                    System.IO.File.Delete($"{pathCesar}/{key.FileName}");
                }

                using var saverLlave = new FileStream($"{pathCesar}/{key.FileName}", FileMode.OpenOrCreate);
                await key.CopyToAsync(saverLlave);
                saverLlave.Close();


                // Proceso Cifrado César
                string rutaArchivo = $"{pathCesar}/{file.FileName}";
                string rutaLlave = $"{pathCesar}/{key.FileName}";
                string rutaCifrado = pathCesar;
                string[] fileName = file.FileName.Split(".");

                Cesar cesar = new Cesar();
                cesar.Cifrar(rutaArchivo, rutaLlave, rutaCifrado, fileName[0]);

                //Archivo a mandar de regreso
                return PhysicalFile($"{rutaCifrado}/{fileName[0]}.csr", "text/plain", $"{fileName[0]}.csr");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}