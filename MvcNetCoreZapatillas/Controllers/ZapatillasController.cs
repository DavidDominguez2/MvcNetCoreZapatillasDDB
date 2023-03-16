using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers {
    public class ZapatillasController : Controller {

        private RepositoryZapatillas repo;

        public ZapatillasController(RepositoryZapatillas repo) {
            this.repo = repo;
        }

        public async Task<IActionResult> Index() {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> Details(int idZapatilla) {
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idZapatilla);
            return View(zapatilla);
        }

        public async Task<IActionResult> _PaginacionZapatillas(int? posicion, int idZapatilla) {

            PaginacionImagenesZapatilla imagenes = await this.repo.GetImagenesZapatillaAsync(posicion.Value, idZapatilla);

            return PartialView("_PaginacionZapatillas", imagenes);
        }
    }
}
