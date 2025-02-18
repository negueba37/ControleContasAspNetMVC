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
		private readonly IConfigurationParametersRepository _configurationParametersRepository;

		public HomeController(ILogger<HomeController> logger, IInstalmentRepository instalmentRepository, IConfigurationParametersRepository configurationParametersRepository)
		{
			_logger = logger;
			_instalmentRepository = instalmentRepository;
			_configurationParametersRepository = configurationParametersRepository;
		}

		public IActionResult Index(int month = 0, int year = 0,int card = 0)
		{
			var configurationParameters = _configurationParametersRepository.Get();

			if (month == 0) 
				month = configurationParameters.DashboardPrincipalMes;
			if (year == 0)
				year = configurationParameters.DashboardPrincipalAno;
			
			configurationParameters.DashboardPrincipalMes = month;
			configurationParameters.DashboardPrincipalAno = year;
			configurationParameters.DashboardPrincipalCartao = card;
			
			_configurationParametersRepository.Update(configurationParameters);
			HomeViewModel homeViewModel = new HomeViewModel();
			homeViewModel.Month = month;
			homeViewModel.Year = year;
			homeViewModel.Cartao = card;
			homeViewModel.Installments = _instalmentRepository.GetByMonthMaturity(month, year, card);
			homeViewModel.InstallmentsBankSlip = _instalmentRepository.GetByMonthMaturity(month, year);
			return View(homeViewModel);
		}

		public IActionResult DashboardAnual()
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
		[HttpPost]
		public void UpdateIsPaidInstallment(int installment) 
		{
			var installmentDTO = _instalmentRepository.GetById(installment);
			installmentDTO.IsPaid = true;
			_instalmentRepository.Update(installmentDTO);
			
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
