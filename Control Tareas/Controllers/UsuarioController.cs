using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Control_Tareas.Data;



namespace Control_Tareas.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        //INICIO VISTA USUARIOS
        [HttpGet]
        public JsonResult VistaUsuario(int pageindex, int pagesize, string usuario)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.USUARIO.Select(x => x.ID).ToList().Count();
                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                UsuarioList retorno = new UsuarioList
                {

                    Total = (from cot in db.USUARIO
                                 //where cot.genero == "f"
                             orderby cot.ID ascending
                             select new
                             {
                                 cot.ID,
                                 cot.NOMBRE,
                                 cot.CORREO,
                                 cot.PERFIL,
                                 cot.APELLIDO


                             }).ToArray().Count(),

                    Resultado = (from cot in db.USUARIO
                                     //where cot.genero == "f"
                                 orderby cot.ID ascending
                                 select new ListUsuarios
                                 {
                                     id_usuario = cot.ID,
                                     nombre = cot.NOMBRE,
                                     correo = cot.CORREO,
                                     perfil = ((decimal)cot.PERFIL),
                                     apellido = cot.APELLIDO

                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()


                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

        }
        // FIN VISTA USUARIOS


        //EDITAR  USUARIOS
        [HttpPost]
        public ActionResult editarUser(Usuario a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                using (Data.Entities db = new Data.Entities())

                {
                    USUARIO al = db.USUARIO.Find(a.id_usuario);

                    if (!string.IsNullOrWhiteSpace(a.Nombre))
                    {
                        al.NOMBRE = a.Nombre;
                    }

                    if (!string.IsNullOrWhiteSpace(a.apellido))
                    {
                        al.APELLIDO = a.apellido;
                    }

                    if (!string.IsNullOrWhiteSpace(a.correo))
                    {
                        al.CORREO = a.correo;
                    }

                    if (!string.IsNullOrWhiteSpace(a.pass))
                    {
                        al.PASS = a.pass;
                    }

                    if (a.id_perfil > 0)
                    {
                        al.PERFIL = a.id_perfil;
                    }
                    if (a.UNIDADINTERNA != null)
                    {
                        al.UNIDADINTERNA = a.UNIDADINTERNA;
                    }
                    db.USUARIO.Attach(al);
                    db.Entry(al).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //FIN EDITAR USUARIOS


        //CREAR USUARIO
        [HttpPost]
        public string CrearUsuario(USUARIO a)
        {
            string Respuesta = "";
            string pass = CrearPass();
            _Correo correo = new _Correo();
            List<string> lstCorreos = new List<string>();

            if (!ModelState.IsValid)
                return "objeto no valido";
            try
            {
                using (Data.Entities db = new Data.Entities())
                {
                    var listado = (from us in db.USUARIO
                                   where us.CORREO == a.CORREO
                                   select new
                                   {
                                   }).ToList();
                    if (listado.Count <= 0)
                    {

                        a.PASS = pass;

                        db.USUARIO.Add(a);
                        db.SaveChanges();
                      Respuesta = "OK";


                        correo.Asunto = "Creación de contraseña";
                        correo.Cuerpo = $@"Se ha asociado a su usuario una contraseña con exito. </br>
                                               La nueva contraseña es: <strong> {pass} </strong> </br>
                                               Saludos.";
                        lstCorreos.Add(a.CORREO);
                        correo.Destinatario = lstCorreos;

                        SendMessages(correo);

                        return Respuesta;
                    }
                    else
                    {
                        Respuesta = "NOK";
                        return Respuesta;
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al registrar Usuario" + ex.Message);
                return Respuesta;
            }
        }
        public string CrearPass()
        {
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            int longitud = 8;
            Random rnd = new Random();
            try
            {
                while (0 < longitud--)
                {
                    res.Append(caracteres[rnd.Next(caracteres.Length)]);
                }
            }
            catch (Exception e)
            {
                res.Append("\n Error " + e.Message);
            }

            return res.ToString();
        }
        //FIN CREAR USUARIO  


        //ELIMINAR USUARIO
        [HttpPost]
        public string eliminarusuario(USUARIO a)
        {
            string Respuesta = "";
            try
            {
                //if (!ModelState.IsValid)
                //    return View();

                using (Data.Entities db = new Data.Entities())
                {
                    USUARIO al = db.USUARIO.Find(a.ID);

                    db.USUARIO.Remove(al);
                    db.Entry(al).State = System.Data.Entity.EntityState.Deleted;

                    db.SaveChanges();

                    Respuesta = "OK";
                    return Respuesta;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //FIN ELIMINAR USUARIO


        //OBTENER PERFILES
        [HttpGet]
        public JsonResult ObtenerPerfiles(int pageindex, int pagesize, string estado)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.PERFIL.Select(x => x.ID_PERFIL).ToList().Count();
                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                listaPerfil retorno = new listaPerfil
                {

                    Total = (from per in db.PERFIL
                              where per.ESTADO == estado
                             orderby per.ID_PERFIL ascending
                             select new
                             {
                                 per.ID_PERFIL,
                                 per.NOMBRE,
                                 per.FECHACREACION,
                                 per.ESTADO

                             }).ToArray().Count(),

                    Resultado = (from per in db.PERFIL
                                 where per.ESTADO == estado

                                 orderby per.ID_PERFIL ascending
                                 select new listaPerfil
                                 {
                                     id_perfil = per.ID_PERFIL,
                                     nombre = per.NOMBRE,
                                     //fechacreacion = (DateTime)per.FECHACREACION,
                                     estado = per.ESTADO

                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()


                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

        }
        // FIN OBTENER PERFILES

        //OBTENER PERFILES
        [HttpGet]
        public JsonResult ObtenerPerfiles1(int pageindex, int pagesize)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.PERFIL.Select(x => x.ID_PERFIL).ToList().Count();
                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                listaPerfil retorno = new listaPerfil
                {

                    Total = (from per in db.PERFIL
                             where per.ESTADO == "Activo"
                             orderby per.ID_PERFIL ascending
                             select new
                             {
                                 per.ID_PERFIL,
                                 per.NOMBRE,
                                 per.FECHACREACION,
                                 per.ESTADO

                             }).ToArray().Count(),

                    Resultado = (from per in db.PERFIL
                               where per.ESTADO == "Activo"

                                 orderby per.ID_PERFIL ascending
                                 select new listaPerfil
                                 {
                                     id_perfil = per.ID_PERFIL,
                                     nombre = per.NOMBRE,
                                     //fechacreacion = (DateTime)per.FECHACREACION,
                                     estado = per.ESTADO

                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()


                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

        }
        // FIN OBTENER PERFILES



        private static ResponStandard SendMessages(_Correo correo)
        {
            ResponStandard res = new ResponStandard();

            correo.Remitente = "controltareas@psirius.cl";// System.Configuration.ConfigurationManager.AppSettings["Remitente"]; 
            var fromt = new MailAddress(correo.Remitente, "Notificación Control Tareas");

            //var dest = new MailAddress(Email);
            string Pass = "Control.2022";// System.Configuration.ConfigurationManager.AppSettings["Password"]; 
            string subject = correo.Asunto;

            try
            {
                var mensaje = new MailMessage();
                mensaje.From = fromt;
                foreach (var i in correo.Destinatario)
                {
                    mensaje.To.Add(i);
                }
                mensaje.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Host = "mail.psirius.cl",
                    Port = 587,
                    EnableSsl = false,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(correo.Remitente, Pass)
                };
                mensaje.Subject = subject;
                mensaje.Body = correo.Cuerpo;
                smtp.Send(mensaje);
                smtp.Dispose();
                res.CodRespuesta = 1;
                res.MensajeR = "Contraseña actualizada y enviada al correo.";
                res.dataR = correo.Destinatario;
            }
            catch (Exception e)
            {
                subject = e.Message;
                res.CodRespuesta = -1;
                res.dataR = e;
                res.MensajeR = "Error: " + e.Message + "\n" + e.InnerException + "\n" + e.StackTrace + "\n" + e.Source;
            }
            return res;
        }

        //CREACION DE PERFILES DE SISTEMA
        [HttpPost]
        public string CreacionPerfil(PERFIL p)
        {
            string respuesta = "";
            try
            {
                using (Data.Entities db = new Entities())
                {
                    PERFIL pp = new PERFIL();

                    if (p.NOMBRE != null)
                    {
                        pp.NOMBRE = p.NOMBRE;
                    }
                    if (p.FECHACREACION != null)
                    {
                        pp.FECHACREACION = p.FECHACREACION;
                    }
                    if (p.ESTADO != null)
                    {
                        pp.ESTADO = p.ESTADO;
                    }
                    if (pp.FECHACREACION != null || pp.NOMBRE != null)
                    {
                        db.PERFIL.Add(pp);
                        db.SaveChanges();
                        respuesta = "perfil creado sin problemas";
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = "No fue posible crear perfil";
                throw;
            }
            return respuesta;
        }

        public string EditarPErfil(PERFIL p)
        {
            string respuesta = "";
            try
            {
                using (Data.Entities db = new Data.Entities())
                {
                    PERFIL pe = db.PERFIL.Find(p.ID_PERFIL);

                    if (pe != null)
                    {
                        if (p.ESTADO != null)
                        {
                            pe.ESTADO = p.ESTADO;
                        }
                        if (p.NOMBRE != null)
                        {
                            pe.NOMBRE = p.NOMBRE;
                        }
                        if (p.ESTADO != null || p.NOMBRE != null)
                        {
                            db.PERFIL.Attach(pe);
                            db.Entry(pe).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    else
                    {
                        respuesta = "Perfil no existe o es incorrecto";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return respuesta = "Perfil actualizado correctamente";
        }
        //FIN METODO CREACION DE PERFILES 





    }
}

public class UsuarioList // INICIO VISTA USUARIOS
{
    public List<ListUsuarios> Resultado { get; set; }
    public int Total { get; set; }
    public List<listaPerfil> listperfiles { get; set; }

    public object dataR { get; set; }
}

public class listaPerfil
{
    public int id_perfil { get; set; }
    public string nombre { get; set; }
    public DateTime fechacreacion { get; set; }
    public string estado { get; set; }
    public int Total { get; set; }
    public List<listaPerfil> Resultado { get; set; }


}

public class ListUsuarios
{
    public int id_usuario { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public string correo { get; set; }
    public decimal perfil { get; set; }
    public int pageindex { get; set; }
    public int pagesize { get; set; }

} // FIN VISTA USUARIOS

public class _Correo
{
    public List<string> Destinatario { get; set; }
    public string Cuerpo { get; set; }
    public string Asunto { get; set; }
    public string Remitente { get; set; }
}