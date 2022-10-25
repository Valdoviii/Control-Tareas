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
     class CrearPasswords
    {
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
    }
}