using EconomicManagementAPP.Models;
using EconomicManagementAPP.Services;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositorieCategories repositorieCategories;

        //Inicializamos l variable repositorieAccountTypes para despues inyectarle las funcionalidades de la interfaz
        public CategoriesController(IRepositorieCategories repositorieCategories)
        {
            this.repositorieCategories = repositorieCategories;
        }

        // Creamos index para ejecutar la interfaz
        public async Task<IActionResult> Index()
        {
            var categories = await repositorieCategories.getCategories();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories categories)
        {
            if (!ModelState.IsValid)
            {
                return View(categories);
            }

            // Validamos si ya existe antes de registrar
            var categoriesExist =
               await repositorieCategories.Exist(categories.Name);

            if (categoriesExist)
            {
                // AddModelError ya viene predefinido en .net
                // nameOf es el tipo del campo
                ModelState.AddModelError(nameof(categories.Name),
                    $"The categorie {categories.Name} already exist.");

                return View(categories);
            }
            await repositorieCategories.Create(categories);
            // Redireccionamos a la lista
            return RedirectToAction("Index");
        }

        // Hace que la validacion se active automaticamente desde el front
        [HttpGet]
        public async Task<IActionResult> VerificaryCategorie(string Name)
        {
            var categoriesExist = await repositorieCategories.Exist(Name);

            if (categoriesExist)
            {
                // permite acciones directas entre front y back
                return Json($"The categories {Name} already exist");
            }

            return Json(true);
        }

        //Actualizar
        //Este retorna la vista tanto del modify
        [HttpGet]
        public async Task<ActionResult> Modify(int Id)
        {
            var categorie = await repositorieCategories.getCategoriesById(Id);

            if (categorie is null)
            {
                //Redireccio cuando esta vacio
                return RedirectToAction("NotFound", "Home");
            }

            return View(categorie);
        }
        //Este es el que modifica y retorna al index
        [HttpPost]
        public async Task<ActionResult> Modify(Categories categories)
        {
            var categorie = await repositorieCategories.getCategoriesById(categories.Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Modify(categories);// el que llega
            return RedirectToAction("Index");
        }
        // Eliminar
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var categorie = await repositorieCategories.getCategoriesById(Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(categorie);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategories(int Id)
        {
            var categorie = await repositorieCategories.getCategoriesById(Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieCategories.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
