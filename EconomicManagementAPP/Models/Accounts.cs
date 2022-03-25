using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryAccount", controller: "Accounts")]//Activamos la validacion se dispara peticion http hacia el back 
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int AccountTypeId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public decimal Balance { get; set; }        
        public string Description { get; set; }
    }
}
