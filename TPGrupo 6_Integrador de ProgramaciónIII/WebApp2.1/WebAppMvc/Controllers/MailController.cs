using App.Core.Business;
using App.Core.Data;
using App.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAppMvc.Controllers
{
    public class MailController : Controller
    {
        public static string UsuarioActual;
        MailsContext _dbContext;
        
        public MailController(MailsContext dbcontext)
        {
            _dbContext = dbcontext;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Enter(string user, string password)
        {
            UsuarioActual= user;
            try
            {
                var userEntity = _dbContext.Users.FirstOrDefault(u => u.UserName == user);

                if (userEntity != null)
                {
                    if (VerifyPassword(password, userEntity.UserPassword))
                    {
                        return Content("1"); // Credenciales válidas
                    }
                }

                return Content("Usuario y/o contraseña incorrecta");
            }
            catch (Exception ex)
            {
                return Content("Ocurrió un error: " + ex.Message);
            }
        }
        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            
            return enteredPassword == storedPassword;
        }

        public IActionResult Detalle(int? id)
        {

            if (!id.HasValue 
                || id.Value <= 0)
            {
                return NotFound();
            }


            var mail = new Mail()
            {
                MailId = id.Value,
                Asunto = $"Asunto demo {id}",
                Contenido = $"Contenido demo {id}"
            };

            return View(mail);
        }
        
        public IActionResult MensajesEnviados(string remitente, string destinatario)
        {
            var user = UsuarioActual; // Reemplazar con el usuario actualmente autenticado

            var mensajes = _dbContext.Mails
                .Where(m => (m.Remitente == remitente && m.Destinatario == destinatario) ||
                            (m.Remitente == destinatario && m.Destinatario == remitente) ||
                            (m.Remitente == user && m.Destinatario == destinatario) ||
                            (m.Remitente == destinatario && m.Destinatario == user))
                .ToList();

            return View(mensajes);
        }



        public IActionResult Inbox()
        {
            var user = UsuarioActual/* User.Identity.Name*/; // Reemplaza con la lógica para obtener el usuario actualmente autenticado

            var mensajes = _dbContext.Mails
                .Where(m => m.Destinatario == user)
                .ToList();

            return View(mensajes);
        }
        public IActionResult Outbox()
        {
            var user = UsuarioActual/* User.Identity.Name*/; // Reemplaza con la lógica para obtener el usuario actualmente autenticado

            var mensajes = _dbContext.Mails
                .Where(m => m.Remitente == user)
                .ToList();

            return View(mensajes);
        }
        public IActionResult Enviar(string asunto, string contenido, string remitente, string destinatario)
        {
            try
            {
                
                var nuevoMail = new Mail
                {
                    Asunto = asunto,
                    Contenido = contenido,
                    Remitente = remitente,
                    Destinatario = destinatario
                };

                
                _dbContext.Mails.Add(nuevoMail);
                _dbContext.SaveChanges();

                return Content("El mensaje se envió correctamente");
            }
            catch (Exception ex)
            {
                return Content("Ocurrió un error al enviar el mensaje: " + ex.Message);
            }
        }

        public IActionResult Buscar(string buscar)
        {
            var user = UsuarioActual; 

            var mensajes = _dbContext.Mails
                .Where(m => m.Destinatario == user && m.Asunto.Contains(buscar)|| m.Remitente.Contains(buscar)|| m.Destinatario.Contains(buscar))
                .ToList();

            return View(mensajes);
        }

    }
}
