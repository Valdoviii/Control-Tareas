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
    public class ProblemaController : Controller
    {
        // GET: Problema
        public ActionResult Index()
        {
            return View();
        }


        //CREACION DE Problema
        [HttpPost]
        public string CreacionProblema(PROBLEMAS p)
        {
            string respuesta = "";
            try
            {
                using (Data.Entities db = new Entities())
                {
                    PROBLEMAS pp = new PROBLEMAS();

                    if (p.NOMBRE != null)
                    {
                        pp.NOMBRE = p.NOMBRE;
                    }
                    if (p.FECHA_CREACION != null)
                    {
                        pp.FECHA_CREACION = p.FECHA_CREACION;
                    }
                    if (p.ID_TAREA != null)
                    {
                        pp.ID_TAREA = p.ID_TAREA;
                    }
                    if (p.PROPIETARIO != null)
                    {
                        pp.PROPIETARIO = p.PROPIETARIO;
                    }
                    if (p.DESCRIPCION != null)
                    {
                        pp.DESCRIPCION = p.DESCRIPCION;
                    }
                    if (p.CRITICIDAD != null)
                    {
                        pp.CRITICIDAD = p.CRITICIDAD;
                    }
                    if (p.ESTADO != null)
                    {
                        pp.ESTADO = p.ESTADO;
                    }
                    db.PROBLEMAS.Add(pp);
                    db.SaveChanges();
                    respuesta = "OK";
                }
            }
            catch (Exception e)
            {
                respuesta = "NOK";
                throw;
            }
            return respuesta;
        }
        //FIN CREACION DE Problema

        //OBTENER problemas
        [HttpGet]
        public JsonResult ObtenerProblemas(int pageindex, int pagesize, string estado)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.PROBLEMAS.Select(x => x.ID_PROBLEMA).ToList().Count();

                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                ListProblemas retorno = new ListProblemas
                {
                    Total = (from ta in db.PROBLEMAS
                             where ta.ESTADO == estado
                             orderby ta.ID_PROBLEMA ascending
                             select new
                             {
                                 ta.ID_PROBLEMA,
                                 ta.NOMBRE,
                                 ta.DESCRIPCION,
                                 ta.PROPIETARIO,
                                 ta.ESTADO,
                                 ta.CRITICIDAD
                             }).ToArray().Count(),

                    Resultado = (from ta in db.PROBLEMAS
                                 where ta.ESTADO == estado
                                 orderby ta.ID_PROBLEMA ascending
                                 select new ListProblemas
                                 {
                                     id_problemas = (short)ta.ID_PROBLEMA,
                                     id_tareas=(short)ta.ID_TAREA,
                                     nombre = ta.NOMBRE,
                                     estado = ta.ESTADO,
                                     descripcion = ta.DESCRIPCION,
                                     criticidad = ta.CRITICIDAD,
                                     propietario = ta.PROPIETARIO
                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()
                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }
        //FIN OBTENER problemas

        //OBTENER problemas
        [HttpGet]
        public JsonResult ObtenerProblemaEdit(int pageindex, int pagesize, int id_problemas)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.PROBLEMAS.Select(x => x.ID_PROBLEMA).ToList().Count();

                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                ListProblemas retorno = new ListProblemas
                {
                    Total = (from ta in db.PROBLEMAS
                             where ta.ID_PROBLEMA == id_problemas
                             orderby ta.ID_PROBLEMA ascending
                             select new
                             {
                                 ta.ID_PROBLEMA,
                                 ta.NOMBRE,
                                 ta.DESCRIPCION,
                                 ta.PROPIETARIO,
                                 ta.ESTADO,
                                 ta.CRITICIDAD
                             }).ToArray().Count(),

                    Resultado = (from ta in db.PROBLEMAS
                                 where ta.ID_PROBLEMA == id_problemas
                                 orderby ta.ID_PROBLEMA ascending
                                 select new ListProblemas
                                 {
                                     id_problemas = (short)ta.ID_PROBLEMA,
                                     id_tareas = (short)ta.ID_TAREA,
                                     nombre = ta.NOMBRE,
                                     estado = ta.ESTADO,
                                     descripcion = ta.DESCRIPCION,
                                     criticidad = ta.CRITICIDAD,
                                     propietario = ta.PROPIETARIO
                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()
                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }
        //FIN OBTENER problemas

        //EDITAR  problemas
        [HttpPost]
        public string editarProblema(PROBLEMAS a)
        {
            string respuesta = "";

            try
            {

                using (Data.Entities db = new Data.Entities())
                {
                    PROBLEMAS al = db.PROBLEMAS.Find(a.ID_PROBLEMA);

                    if (a.NOMBRE != null)
                    {
                        al.NOMBRE = a.NOMBRE;
                    }
                    if (!string.IsNullOrWhiteSpace(a.DESCRIPCION))
                    {
                        al.DESCRIPCION = a.DESCRIPCION;
                    }
                    if (!string.IsNullOrWhiteSpace(a.CRITICIDAD))
                    {
                        al.CRITICIDAD = a.CRITICIDAD;
                    }
                    if (a.ESTADO != null)
                    {
                        al.ESTADO = a.ESTADO;
                    }
                    db.PROBLEMAS.Attach(al);
                    db.Entry(al).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return respuesta = "OK";
                }
            }
            catch (Exception ex)
            {
                return respuesta = "NOK";

                throw;
            }
        }
        //FIN EDITAR problemas

        //OBTENER problemas por id de tarea
        [HttpGet]
        public JsonResult ObtenerProblemaPorTarea(int pageindex , int pagesize, int ID_TAREA)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.PROBLEMAS.Select(x => x.ID_PROBLEMA).ToList().Count();
                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                ListProblemas retorno = new ListProblemas
                {
                    Total = (from ta in db.PROBLEMAS
                             where ta.ID_TAREA == ID_TAREA
                             orderby ta.ID_PROBLEMA ascending
                             select new
                             {
                                 ta.ID_PROBLEMA,
                                 ta.NOMBRE,
                                 ta.DESCRIPCION,
                                 ta.PROPIETARIO,
                                 ta.ESTADO,
                                 ta.CRITICIDAD
                             }).ToArray().Count(),

                    Resultado = (from ta in db.PROBLEMAS
                                 where ta.ID_TAREA == ID_TAREA
                                 orderby ta.ID_PROBLEMA ascending
                                 select new ListProblemas
                                 {
                                     id_problemas = (short)ta.ID_PROBLEMA,
                                     id_tareas = (short)ta.ID_TAREA,
                                     nombre = ta.NOMBRE,
                                     estado = ta.ESTADO,
                                     descripcion = ta.DESCRIPCION,
                                     criticidad = ta.CRITICIDAD,
                                     propietario = ta.PROPIETARIO
                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()
                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
        }
        //FIN OBTENER problemas


    }
}

public class ListProblemas
{
    public int id_tareas { get; set; }
    public int id_problemas { get; set; }
    public string nombre { get; set; }
    public string estado { get; set; }
    public string descripcion { get; set; }
    public string criticidad { get; set; }
    public string propietario { get; set; }
    public int pageindex { get; set; }
    public int pagesize { get; set; }
    public List<ListProblemas> Resultado { get; set; }
    public int Total { get; set; }
    public DateTime fechacreacion { get; set; }


}