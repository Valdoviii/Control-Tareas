using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Control_Tareas.Controllers
{
    public class ResponStandard
    {
        public int CodRespuesta { get; set; }
        public string MensajeR { get; set; }
        public object dataR { get; set; }
        public int CantReg { get; set; }
    }
}