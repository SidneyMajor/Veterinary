using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Veterinary.Data.Repository;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAnimalRepository _animalRepository;

        public HomeController(IClientRepository clientRepository, IDoctorRepository doctorRepository, IAnimalRepository animalRepository)
        {
           _clientRepository = clientRepository;
            _doctorRepository = doctorRepository;
            _animalRepository = animalRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Clients = _clientRepository.GetAll().Count();
            ViewBag.Doctors = _doctorRepository.GetAll().Count();
            ViewBag.Animals =_animalRepository.GetAll().Count();
            return View();
        }

        public IActionResult About()
        {           
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
