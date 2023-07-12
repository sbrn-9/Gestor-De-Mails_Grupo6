using App.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Data
{
    public class MailRepository
    {
        public MailRepository()
        {
                
        }

        //public string DatosDeInicio(string user, string password)
        //{
        //    if (user != null && password != null)
        //    {
        //        using (var context = new MailsContext(new DbContextOptions<MailsContext>()))
        //        {
        //            var query = from m in context.Users
        //                        where m.UserName == user && m.UserPassword == password
        //                        select m;
        //            if (query != null)
        //            {
        //                return "1";
        //            }
        //            else { return "0"; }
        //        }
        //    }else { return "Error"; }
        //}
        public string DatosDeInicio(string user, string password)
        {
            if (user != null && password != null)
            {
                using (var context = new MailsContext(new DbContextOptions<MailsContext>()))
                {
                    var query = from u in context.Users
                                where u.UserName == user && u.UserPassword == password
                                select u;

                    if (query.Any())
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            else
            {
                return "-1"; // Valor de error
            }
        }
        public List<Mail> Search(string textToSearch,
                                int pageSize,
                                int pageIndex)
        {

            //pageIndex = 1 (pageSize = 5) > ... fila/row 1
            //pageIndex = 2 (pageSize = 5) > ... fila/row 6
            //pageIndex = 3 (pageSize = 5) > ... fila/row 11
            //pageIndex = 4 (pageSize = 5) > ... fila/row 16

            var skipRows = ((pageIndex - 1) * pageSize);

            using (var context = new MailsContext(new DbContextOptions<MailsContext>()))
            {
                var query = from m in context.Mails
                            where m.Asunto.Contains(textToSearch)
                            select m;

                //pageTotal = 3;

                return query.Skip(skipRows)
                            .Take(pageSize)
                            .ToList();
            }

        }


        public void Enviar(string asunto, string contenido, string remitente, string destinatario)
        {
             var movie = new Mail();

            using (var conection = new MailsContext(new DbContextOptions<MailsContext>()))
            {
                while(movie is not null)
                {
                    movie.Asunto = asunto;
                    movie.Contenido = contenido;
                    movie.Remitente = remitente;
                    movie.Destinatario = destinatario;
                }

            } 
               
        }


        public List<Mail> BSalida()
         {
            var lista = new List<Mail>();

            return lista;
            } 

         public List<Mail> BEntrada()
        {
            var lista= new List<Mail>();

            return lista;
    } 

    }
}
