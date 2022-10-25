using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Control_Tareas.Data;

namespace Control_Tareas.Controllers
{
    public class TareasController : Controller
    {
        // GET: Tareas
        public ActionResult Index()
        {
            return View();
        }

        //OBTENER TAREAS
        [HttpGet]
        public JsonResult ObtenerTareas(int pageindex, int pagesize, string estado)
        {
            using (Data.Entities db = new Data.Entities())
            {
                var CantReg = db.TAREAS.Select(x => x.ID_TAREAS).ToList().Count();

                if (pagesize == 0)
                {
                    pagesize = CantReg;
                }
                ListTareas retorno = new ListTareas
                {
                    Total = (from ta in db.TAREAS
                             where ta.ESTADO2 == estado
                             orderby ta.ID_TAREAS ascending
                             select new
                             {
                                 ta.ID_TAREAS,
                                 ta.NOMBRE,
                                 ta.DESCRIPCION,
                                 ta.OBSERVACIONES,
                                 ta.ESTADO

                             }).ToArray().Count(),

                    Resultado = (from ta in db.TAREAS
                                 where ta.ESTADO2 == estado
                                 orderby ta.ID_TAREAS ascending
                                 select new ListTareas
                                 {
                                     id_tareas = ta.ID_TAREAS,
                                     nombre = ta.NOMBRE,
                                     estado = ta.ESTADO,
                                     descripcion = ta.DESCRIPCION,
                                     observaciones = ta.OBSERVACIONES

                                 }).Skip(((pageindex - 1) * pagesize)).Take(pagesize).ToList()

                };
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

        }
        //FIN OBTENER TAREAS

        //OBTENER TAREAS  POR USUARIO(ver detalle tareas)
        [HttpGet]
        public JsonResult ObtenerTareasUsuario(int id_tarea)
        {
            using (Data.Entities db = new Data.Entities())
            {

                TAREAS_USUARIOS LstTareaUser = new TAREAS_USUARIOS();

                var LstPro = (from ta in db.TAREAS
                              where ta.ID_TAREAS == id_tarea
                              select new
                              {
                                  id_tareas = ta.ID_TAREAS,
                                  nombre = ta.NOMBRE,
                                  estado = ta.ESTADO,
                                  descripcion = ta.DESCRIPCION,
                                  observaciones = ta.OBSERVACIONES,
                                  fechdesde = ta.FECHADESDE.Value,

                                  LstTareaUser = (from taU in db.TAREAS_USUARIOS
                                                  join pri in db.USUARIO on taU.ID_USUARIO equals pri.ID
                                                  where taU.ID_TAREAS == id_tarea
                                                  select new
                                                  {
                                                      ID = taU.ID,
                                                      IDUSER = taU.ID_USUARIO,
                                                      IDTAREA = taU.ID_TAREAS,
                                                      nombreUser = pri.NOMBRE,
                                                      apellidoUser = pri.APELLIDO

                                                  }).ToList()
                              }).ToList();


                return Json(LstPro, JsonRequestBehavior.AllowGet);



            };


        }

        //FIN OBTENER TAREAS POR USUARIO

        //CREACION TAREAS
        [HttpPost]
        public string CrearTareas(TAREAS1 a)
        {
            string Respuesta = "";
            if (!ModelState.IsValid)
                return "objeto no valido";
            try
            {
                using (Data.Entities db = new Data.Entities())
                {
                    if (true)
                    {
                        TAREAS s = new TAREAS();
                        TAREAS_USUARIOS se = new TAREAS_USUARIOS();
                        if (a.NOMBRE != null)
                        {
                            s.NOMBRE = a.NOMBRE;
                        }
                        if (a.ESTADO != null)
                        {
                            s.ESTADO = a.ESTADO;
                        }
                        if (a.ESTADO2 != null)
                        {
                            s.ESTADO2 = a.ESTADO2;
                        }
                        if (a.DESCRIPCION != null)
                        {
                            s.DESCRIPCION = a.DESCRIPCION;
                        }
                        if (a.FECHADESDE != null)
                        {
                            s.FECHADESDE = a.FECHADESDE;
                        }
                        if (a.FECHAHASTA != null)
                        {
                            s.FECHAHASTA = a.FECHAHASTA;
                        }
                        if (a.FECHACREACION != null)
                        {
                            s.FECHACREACION = a.FECHACREACION;
                        }
                        db.TAREAS.Add(s);
                        db.SaveChanges();
                        var IdTareaReg = db.TAREAS.OrderByDescending(x => x.FECHACREACION).First().ID_TAREAS;
                        if (a.listUsers != null)
                        {
                            foreach (var item in a.listUsers)
                            {
                                TAREAS_USUARIOS tu = new TAREAS_USUARIOS();
                                tu.ID_TAREAS = IdTareaReg;
                                tu.ESTADO = "ingresado";
                                tu.ID_USUARIO = (short)item.ID;
                                db.TAREAS_USUARIOS.Add(tu);
                            }
                            db.SaveChangesAsync();
                        }
                        Respuesta = "OK";
                        return Respuesta;
                    }
                    else
                    {
                        Respuesta = "No fue posible crear tarea";
                        return Respuesta;
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al registrar tarea" + ex.Message);
                return Respuesta;
            }
        }
        //FIN CREACION TAREAS

        //EDITAR TAREAS ADMIN
        [HttpPost]
        public ActionResult editarTareas(TAREAS1 a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                using (Data.Entities db = new Data.Entities())
                {
                    TAREAS al = db.TAREAS.Find(a.ID_TAREAS);
                    if (!string.IsNullOrWhiteSpace(a.NOMBRE))
                    {
                        al.NOMBRE = a.NOMBRE;
                    }
                    if (!string.IsNullOrWhiteSpace(a.DESCRIPCION))
                    {
                        al.DESCRIPCION = a.DESCRIPCION;
                    }
                    if (!string.IsNullOrWhiteSpace(a.ESTADO))
                    {
                        al.ESTADO = a.ESTADO;
                    }
                    if (!string.IsNullOrWhiteSpace(a.ESTADO2))
                    {
                        al.ESTADO2 = a.ESTADO2;
                    }
                    if (!string.IsNullOrWhiteSpace(a.OBSERVACIONES))
                    {
                        al.OBSERVACIONES = a.OBSERVACIONES;
                    }
                    if (a.FECHADESDE != null)
                    {
                        al.FECHADESDE = a.FECHADESDE;
                    }
                    if (a.FECHAHASTA != null)
                    {
                        al.FECHAHASTA = a.FECHAHASTA;
                    }
                    db.TAREAS.Attach(al);
                    db.Entry(al).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    if (a.listUserRem != null)
                    {
                        if (a.listUserRem != null && a.listUsers == null)
                        {
                            foreach (var itemRem in a.listUserRem)
                            {
                                var liss = (from uss in db.TAREAS_USUARIOS
                                            where uss.ID_TAREAS == a.ID_TAREAS && uss.ID_USUARIO == itemRem.ID
                                            select new
                                            {
                                                uss.ID
                                            }).FirstOrDefault();
                                if (liss != null)
                                {
                                    TAREAS_USUARIOS tt1 = db.TAREAS_USUARIOS.Find(liss.ID);
                                    db.TAREAS_USUARIOS.Remove(tt1);
                                    db.Entry(tt1).State = System.Data.Entity.EntityState.Deleted;
                                    db.SaveChangesAsync();
                                }
                            }
                        }

                        else
                        {
                            relacionarEditar(a);
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //FIN EDITAR TAREAS ADMIN

        public string relacionarEditar(TAREAS1 a)
        {
            StringBuilder res = new StringBuilder();
            using (Data.Entities db = new Data.Entities())
                try
                {

                    foreach (var item in a.listUserRem)
                    {
                        var lis = (from us in db.TAREAS_USUARIOS
                                   where us.ID_TAREAS == a.ID_TAREAS && us.ID_USUARIO == item.ID
                                   select new
                                   {
                                       us.ID
                                   }).FirstOrDefault();
                        if (lis != null)
                        {
                            TAREAS_USUARIOS tt = db.TAREAS_USUARIOS.Find(lis.ID);
                            tt.ID_USUARIO = null;
                            db.TAREAS_USUARIOS.Attach(tt);
                            db.Entry(tt).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChangesAsync();
                        }
                    }

                    if (a.listUsers != null)
                    {
                        foreach (var item3 in a.listUsers)
                        {
                            var lis2 = (from u in db.TAREAS_USUARIOS
                                        where u.ID_TAREAS == a.ID_TAREAS && u.ID_USUARIO == null
                                        select new
                                        {
                                            u.ID
                                        }).FirstOrDefault();
                            if (lis2 != null)
                            {
                                TAREAS_USUARIOS tt = db.TAREAS_USUARIOS.Find(lis2.ID);
                                tt.ID_USUARIO = (short)item3.ID;
                                db.TAREAS_USUARIOS.Attach(tt);
                                db.Entry(tt).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChangesAsync();
                            }
                        }
                    }

                }
                catch (Exception e)
                {

                    throw;
                }

            return res.ToString();



        }

        [HttpGet]
        public JsonResult ObtenerTareasUserLog(_FiltroTareas_Usuarios a)
        {
            try
            {
                List<TAREAS_USUARIOS> lstProp = new List<TAREAS_USUARIOS>();
                using (Data.Entities db = new Data.Entities())
                {

                    var LstPro = (from ta in db.TAREAS_USUARIOS
                                  where ta.ID_USUARIO == a.id_usuario
                                  select new
                                  {
                                      id_tareas = ta.ID_TAREAS

                                  }).ToList();

                    if (LstPro.Count > 0)
                    {
                  

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                throw;

            }

        }








    }
}

public class ListTareas
{
    public int id_tareas { get; set; }
    public string nombre { get; set; }
    public string estado { get; set; }
    public string descripcion { get; set; }
    public string observaciones { get; set; }
    public int pageindex { get; set; }
    public int pagesize { get; set; }
    public List<ListTareas> Resultado { get; set; }
    public int Total { get; set; }

    public int id_usuarios { get; set; }

    public DateTime? fechdesde { get; set; }
    public DateTime fechahasta { get; set; }
    public DateTime fechacreacion { get; set; }


}

public class TAREAS1
{

    public short ID_TAREAS { get; set; }
    public string NOMBRE { get; set; }
    public string ESTADO { get; set; }
    public string ESTADO2 { get; set; }
    public string DESCRIPCION { get; set; }
    public string OBSERVACIONES { get; set; }
    public Nullable<System.DateTime> FECHADESDE { get; set; }
    public Nullable<System.DateTime> FECHAHASTA { get; set; }
    public Nullable<System.DateTime> FECHACREACION { get; set; }
    public List<USUARIO> listUserRem { get; set; }

    public List<USUARIO> listUsers { get; set; }
}

public class TAREAS_USUARIOS1
{

    public short ID { get; set; }
    public Nullable<short> ID_TAREAS { get; set; }
    public Nullable<short> ID_USUARIO { get; set; }
    public string ESTADO { get; set; }

    public int pageindex { get; set; }
    public int pagesize { get; set; }

    public List<TAREAS_USUARIOS1> Resultado { get; set; }
    public int Total { get; set; }

}

public class _FiltroTareas_Usuarios
{
    public int id_usuario { get; set; }
    public int pageindex { get; set; }
    public int pagesize { get; set; }
}