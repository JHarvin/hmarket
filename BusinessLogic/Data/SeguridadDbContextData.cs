using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class SeguridadDbContextData
    {
        public static async Task seedUserAsync(UserManager<Usuario> userManager) {
            if (!userManager.Users.Any()) {
                var usuario = new Usuario { 
                   Nombre = "Harvin",
                   Apellido = "Ramos",
                   Email = "harv.vin@gmail.com",
                   UserName = "har.vin",
                   Direccion = new Direccion {
                      Calle = "sexta decima San Salvador",
                      Ciudad = "San Salvador",
                      Departamento = "San Salvador",
                      CodigoPostal = "1701"
                   }
                };
               await userManager.CreateAsync(usuario, "VaxiDrez2025$");
            }
        }
    }
}
