using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Categories
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryCategorie", controller: "Categories")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int OperationTypeId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int UserId { get; set; }
    }
}
