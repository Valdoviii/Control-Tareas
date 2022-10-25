using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Control_Tareas.Data;


namespace Control_Tareas.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ValidaLogin(string usuario, string contrasenna)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var listado = (from cot in db.USUARIO
                               where cot.CORREO == usuario.Trim() && cot.PASS == contrasenna.Trim()
                               select new
                               {
                                   cot.NOMBRE,
                                   cot.ID,
                                   cot.PERFIL,
                                   cot.CORREO,
                                   cot.APELLIDO
                               }).ToList();

                if (listado.Count > 0)
                {
                    Usuario cot = new Usuario
                    {
                        Nombre = listado[0].NOMBRE,
                        apellido = listado[0].APELLIDO,
                        id_usuario = listado[0].ID,
                        id_perfil = (decimal)listado[0].PERFIL,
                        correo = listado[0].CORREO
                    };
                    return Json(cot, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }



        //RECUPERAR CONTRASEÑA
        public ResponStandard RecuperarContrasenia(string email1)
        {
            ResponStandard res = new ResponStandard();
            List<string> lstCorreos = new List<string>();
            _Correo correo = new _Correo();

            string pass =  CrearPassword();

            try
            {
                using (Data.Entities db = new Data.Entities())
                //using (var bd = new TrackBDI())
                {
                    USUARIO us = db.USUARIO.Where(u => u.CORREO == email1).FirstOrDefault();


                    if (us != null)
                    {
                        if (pass.Contains("Error"))
                        {
                            res.CodRespuesta = -2;
                            res.MensajeR = "Error creando contraseña nueva";
                            res.dataR = us;
                        }
                        else
                        {
                            us.PASS = pass;
                            db.Set<USUARIO>().AddOrUpdate(us);
                            db.SaveChanges();
                            correo.Asunto = "Cambio de contraseña";
                            correo.Cuerpo = $@"Se ha actualizado su contraseña con exito. </br>
                                               La nueva contraseña es: <strong> {pass} </strong> </br>
                                               Saludos.";
                            lstCorreos.Add(email1);
                            correo.Destinatario = lstCorreos;
                            SendMessage(correo);

                            res.CodRespuesta = 1;
                            res.MensajeR = "Contraseña actualizada";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                res.CodRespuesta = -1;
                res.dataR = e;
                res.MensajeR = "Error: " + e.Message + "\n" + e.InnerException + "\n" + e.StackTrace + "\n" + e.Source;
            }
            return res;
        }

        private string CrearPassword()
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

        private static ResponStandard SendMessage(_Correo correo)
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
        //FIN RECUPERAR CONTRASEÑA

        public class _Correo
        {
            public List<string> Destinatario { get; set; }
            public string Cuerpo { get; set; }
            public string Asunto { get; set; }
            public string Remitente { get; set; }
        }



    }






}

public class Usuario
{
    public string Nombre { get; set; }
    public string apellido { get; set; }
    public string correo { get; set; }
    public string pass { get; set; }
    public int id_usuario { get; set; }
    public decimal id_perfil { get; set; }
    public int pageindex { get; set; }
    public int pagesize { get; set; }
    public string UNIDADINTERNA { get; set; }

}

