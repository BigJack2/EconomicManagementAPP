using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
       //[FirstCapitalLetter]
       //Verificamos si el usuario ya existe mediante el correo
       //[Remote(action: "VerificaryUsers", controller: "Users")]

       //Validamos que la casilla no este vacia y que el correo inicie por mayuscula
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        //[FirstCapitalLetter]

        //Validamos que el correo sea obligatorio
        public string StandarEmail { get; set; }
        [Required(ErrorMessage = "{0} is required")]

        //Validamos que la contraseña sea obligatoria
        public string Password { get; set; }
    }
}
