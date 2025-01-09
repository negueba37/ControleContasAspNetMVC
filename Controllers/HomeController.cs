using ControleContas.Models;
using ControleContas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Repositories;
using Shared.Data.Repositories.Interfaces;
using System.Diagnostics;

namespace ControleContas.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IInstalmentRepository _instalmentRepository;

		public HomeController(ILogger<HomeController> logger, IInstalmentRepository instalmentRepository)
		{
			_logger = logger;
			_instalmentRepository = instalmentRepository;
		}

		public IActionResult Index(int month = 0, int year = 0,int card = 0)
		{
			if (month == 0) 
				month = DateTime.Now.Month;
			if (year == 0)
				year = DateTime.Now.Year;

			HomeViewModel homeViewModel = new HomeViewModel();
			homeViewModel.Month = month;
			homeViewModel.Year = year;
			homeViewModel.Installments = _instalmentRepository.GetByMonthMaturity(month, year, card);
			homeViewModel.InstallmentsBankSlip = _instalmentRepository.GetByMonthMaturity(month, year);
			return View(homeViewModel);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Card()
		{
			return View();
		}

		public IActionResult UpdatePayment() 
		{

			return View("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
