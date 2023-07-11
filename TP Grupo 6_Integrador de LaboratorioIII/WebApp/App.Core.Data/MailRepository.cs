using App.Core.Entities;
using Microsoft.Data.SqlClient;
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

        
        public List<Mail> Search(string textToSearch, int pageSize, int pageIndex, string destinatario, string remitente)
        {
            var skipRows = ((pageIndex - 1) * pageSize);

            using (var context = new MailsContext())
            {
                var query = from m in context.Mails
                            where (m.Asunto.Contains(textToSearch) &&
                                   ((m.Destinatario == destinatario && m.Remitente == remitente) ||
                                    (m.Destinatario == remitente && m.Remitente == destinatario)))
                            select m;

                return query.Skip(skipRows)
                            .Take(pageSize)
                            .ToList();
            }
        }

        

    }
}
