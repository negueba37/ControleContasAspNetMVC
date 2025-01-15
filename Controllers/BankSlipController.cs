using ControleContas.ViewModels;
using ControleContasData.DTO;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Repositories;
using Shared.Data.Repositories.Interfaces;

namespace ControleContas.Controllers
{
	public class BankSlipController : Controller
	{
		private readonly IBankSlipRepository _bankSlipRepository;
		private readonly ICardRepository _cardRepository;
		private readonly IUserRepository _userRepository;
		private readonly IConfigurationParametersRepository _configurationParametersRepository;
		public BankSlipController(IBankSlipRepository bankSlipRepository, 
								  ICardRepository cardRepository,
								  IUserRepository userRepository,
								  IConfigurationParametersRepository configurationParametersRepository)
		{
			_bankSlipRepository = bankSlipRepository;
			_cardRepository = cardRepository;
			_userRepository = userRepository;
			_configurationParametersRepository = configurationParametersRepository;
		}

		public IActionResult New()
		{
			BankSlipViewModel viewModel = new BankSlipViewModel();
			viewModel.Cards = _cardRepository.Cards();
			viewModel.Users = _userRepository.Users();
			return View(viewModel);
		}
		[HttpPost]
		public IActionResult New(BankSlipViewModel viewModel) 
		{			

			try
			{
				_bankSlipRepository.Save(viewModel.BankSlip);
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				viewModel.IsError = true;
				viewModel.Cards = _cardRepository.Cards();
				return View(viewModel);
			}

		}
		public IActionResult Index(int month = 0, int year = 0)
		{
			var configurationParameters = _configurationParametersRepository.Get();

			if (month == 0)
				month = configurationParameters.BankslipMes;
			if (year == 0)
				year = configurationParameters.BankslipAno;

			configurationParameters.AccountAno = year;
			configurationParameters.AccountMes = month;

			_configurationParametersRepository.Update(configurationParameters);

			BankSlipFilterViewModel viewModel = new BankSlipFilterViewModel();
			viewModel.BankSlip = _bankSlipRepository.AccountsByMonthAndYear(month,year);
			viewModel.Month = month;
			viewModel.Year = year;
			return View(viewModel);
		}
	}
}
