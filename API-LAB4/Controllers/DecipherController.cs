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
            if(file == null || key == null) return StatusCode(400, "Bad request");

            string[] archivo = file.FileName.Split('.');
            if(archivo[1].Equals("csr"))
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

                    using var saverLave = new FileStream($"{pathCesar}/{key.FileName}", FileMode.OpenOrCreate);
                    await key.CopyToAsync(saverLave);
                    saverLave.Close();


                    // Proceso Cifrado César
                    string rutaCifrado = $"{pathCesar}/{file.FileName}";
                    string rutaLlave = $"{pathCesar}/{key.FileName}";
                    string rutaDescifrado = pathCesar;
                    string[] fileName = file.FileName.Split(".");

                    Cesar cesar = new Cesar();
                    cesar.Descifrar(rutaCifrado, rutaLlave, rutaDescifrado, fileName[0]);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaDescifrado}/{fileName[0]}.txt", "text/plain", $"{fileName[0]}.txt");
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

                    using var saverLave = new FileStream($"{pathZigZag}/{key.FileName}", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverLave);
                    saverLave.Close();


                    // Proceso Cifrado ZigZag
                    string rutaCifrado = $"{pathZigZag}/{file.FileName}";
                    string rutaLlave = $"{pathZigZag}/{key.FileName}";
                    string rutaDescifrado = pathZigZag;
                    string[] fileName = file.FileName.Split(".");

                    ZigZag zigZag = new ZigZag();
                    zigZag.Descifrar(rutaCifrado, rutaLlave, rutaDescifrado, fileName[0]);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaDescifrado}/{fileName[0]}.txt", "text/plain", $"{fileName[0]}.txt");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            else if (archivo[1].Equals("sdes"))
            {
                try
                {
                    // Escribir archivos subidos hacia el servidor para trabajar con ellos
                    var path = _env.ContentRootPath;
                    path = Path.Combine(path, "Files");

                    string pathSdes = Path.Combine(path, "Sdes");


                    if (System.IO.File.Exists($"{pathSdes}/{file.FileName}"))
                    {
                        System.IO.File.Delete($"{pathSdes}/{file.FileName}");
                    }

                    using var saverArchivo = new FileStream($"{pathSdes}/{file.FileName}", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverArchivo);
                    saverArchivo.Close();

                    if (System.IO.File.Exists($"{pathSdes}/{key.FileName}"))
                    {
                        System.IO.File.Delete($"{pathSdes}/{key.FileName}");
                    }

                    using var saverLave = new FileStream($"{pathSdes}/{key.FileName}", FileMode.OpenOrCreate);
                    await file.CopyToAsync(saverLave);
                    saverLave.Close();


                    // Proceso Cifrado Sdes
                    string rutaCifrado = $"{pathSdes}/{file.FileName}";
                    string rutaLlave = $"{pathSdes}/{key.FileName}";
                    string rutaDescifrado = pathSdes;
                    string[] fileName = file.FileName.Split(".");

                    CipherController.sdes.Descifrar(rutaCifrado, rutaLlave, rutaDescifrado, fileName[0]);

                    //Archivo a mandar de regreso
                    return PhysicalFile($"{rutaDescifrado}/{fileName[0]}.txt", "text/plain", $"{fileName[0]}.txt");
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