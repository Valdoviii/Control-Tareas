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
    public class UnidadInternaController : Controller
    {
        // GET: UnidadInterna
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public string CreaUnidad(UNIDAD_INTERNA a) //metodo crear unidad
		{
            string respuesta = "";

			try
			{
                using (Data.Entities db = new Data.Entities())
				{
                    UNIDAD_INTERNA uu = new UNIDAD_INTERNA();

					if (a.NOMBRE != null)
					{
                        uu.NOMBRE = a.NOMBRE;
					}

					if (a.FECHA_CREACION != null)
					{
						uu.FECHA_CREACION = a.FECHA_CREACION;
					}

                    if (a.ESTADO != null)
                    {
                        uu.ESTADO = a.ESTADO;
                    }

                    db.UNIDAD_INTERNA.Add(uu);
					db.SaveChanges();
                    respuesta = "UNIDAD INTERNA CREADA EXITOSAMENTE";
                }

            }
			catch (Exception e)
			{
                respuesta = "ERROS AL CREAR UNIDAD INTERNA";

                throw;
			} 




            return respuesta;


		}



        //OBTENER UNIDADES INTERNAS 
        [HttpGet]
        public JsonResult ObtenerUnidadesInternas(int pageindex, int pagesize, string estado)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.UNIDAD_INTERNA.Select(x => x.ID_UNIDAD).ToList().Count();
                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                listaUnidadesInternas retorno = new listaUnidadesInternas
                {

                    Total = (from per in db.UNIDAD_INTERNA
                             where per.ESTADO == estado
                             orderby per.ID_UNIDAD ascending
                             select new
                             {
                                 per.ID_UNIDAD,
                                 per.NOMBRE,
                                 per.FECHA_CREACION,
                                 per.ESTADO

                             }).ToArray().Count(),

                    Resultado = (from per in db.UNIDAD_INTERNA
                                 where per.ESTADO == estado

                                 orderby per.ID_UNIDAD ascending
                                 select new listaUnidadesInternas
                                 {
                                     id_unidad = per.ID_UNIDAD,
                                     nombre = per.NOMBRE,
                                     //fechacreacion = (DateTime)per.FECHACREACION,
                                     estado = per.ESTADO

                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()


                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

        }
        // FIN OBTENER UNIDADES INTERNAS 

        //EDITAR UNIDAD INTERNA

        public string EditarUnidadINterna(UNIDAD_INTERNA p)
        {
            string respuesta = "";
            try
            {
                using (Data.Entities db = new Data.Entities())
                {
                    UNIDAD_INTERNA pe = db.UNIDAD_INTERNA.Find(p.ID_UNIDAD);

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
                            db.UNIDAD_INTERNA.Attach(pe);
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
        //FIN EDITAR UNIDAD INTERNA
    }
}


public class listaUnidadesInternas
{
    public int id_unidad { get; set; }
    public string nombre { get; set; }
    public DateTime fechacreacion { get; set; }
    public string estado { get; set; }
    public int Total { get; set; }
    public List<listaUnidadesInternas> Resultado { get; set; }


}