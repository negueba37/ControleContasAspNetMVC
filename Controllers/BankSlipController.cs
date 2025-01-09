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

		public BankSlipController(IBankSlipRepository bankSlipRepository, 
								  ICardRepository cardRepository, 
								  IUserRepository userRepository)
		{
			_bankSlipRepository = bankSlipRepository;
			_cardRepository = cardRepository;
			_userRepository = userRepository;
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
			if (month == 0)
				month = DateTime.Now.Month;
			if (year == 0)
				year = DateTime.Now.Year;

			BankSlipFilterViewModel viewModel = new BankSlipFilterViewModel();
			viewModel.BankSlip = _bankSlipRepository.AccountsByMonthAndYear(month,year);
			viewModel.Month = month;
			viewModel.Year = year;
			return View(viewModel);
		}
	}
}
