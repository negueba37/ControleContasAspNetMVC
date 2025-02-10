using ConsoleData.DTO;
using ControleContas.Domain;
using ControleContas.ViewModels;
using ControleContasData.DTO;
using ControleContasData.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Domain;
using Shared.Data.Repositories;
using Shared.Data.Repositories.Interfaces;

namespace ControleContas.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountRepository _accountRepository;
		private readonly ICardRepository _cardRepository;
		private readonly IUserRepository _userRepository;
		private readonly IInstalmentRepository _installmentRepository;
		private readonly IConfigurationParametersRepository _configurationParametersRepository;
		private readonly ICategoryRepository _categoryRepository;
		public AccountController(IAccountRepository accountRepository,
			ICardRepository cardRepository, 
			IUserRepository userRepository, 
			IInstalmentRepository installmentRepository,
			IConfigurationParametersRepository configurationParametersRepository,
			ICategoryRepository categoryRepository)
		{
			_accountRepository = accountRepository;
			_cardRepository = cardRepository;
			_userRepository = userRepository;
			_installmentRepository = installmentRepository;
			_configurationParametersRepository = configurationParametersRepository;
			_categoryRepository = categoryRepository;
		}

		public IActionResult Index(int month = 0, int year = 0)
		{
			var configurationParameters = _configurationParametersRepository.Get();
			
			if (month == 0)
				month = configurationParameters.AccountMes;
			if (year == 0)
				year = configurationParameters.AccountAno;

			configurationParameters.AccountAno = year;
			configurationParameters.AccountMes = month;

			_configurationParametersRepository.Update(configurationParameters);
			AccountFilterViewModel viewModel = new AccountFilterViewModel();
			viewModel.Accounts = _accountRepository.AccountsByMonthAndYear(month,year);			
			viewModel.Month = month;
			viewModel.Year = year;
			return View(viewModel);
		}	
		public IActionResult Edit(int id)
		{            
            var accountDTO = _accountRepository.AccountById(id);
			accountDTO.Installments = _installmentRepository.GetByAccount(accountDTO.Id);
			return View(accountDTO);
		}
		[HttpPost]
		public IActionResult Edit(AccountDTO accountDTO)
		{
			if (ModelState.IsValid) 
			{
				_accountRepository.Update(accountDTO);                
				return RedirectToAction("Index");
			}
			return View(accountDTO);

		}
        public IActionResult New()
        {
            AccountCardViewModel accountViewModel = new AccountCardViewModel();
            accountViewModel.Cards = _cardRepository.Cards();
            accountViewModel.Users = _userRepository.Users();
			accountViewModel.Categories = _categoryRepository.GetAll();
            return View(accountViewModel);
        }
        [HttpPost]
        public IActionResult New(AccountCardViewModel accountViewModel)
        {
            accountViewModel.Account.Card = _cardRepository.CardById(accountViewModel.Account.CardId);

            try
            {
				_accountRepository.Save(accountViewModel.Account);
				return RedirectToAction("Index");
			}
			catch (Exception e) {
				accountViewModel.IsError = true;
				accountViewModel.Cards = _cardRepository.Cards();
				return View(accountViewModel);
			}

		}
		public IActionResult EditInstallment(int id,int idParcela, Decimal valor, DateTime data) {
			var installment = new InstallmentDTO();
			installment.Id = idParcela;
			installment.Due = data;
			installment.Price = valor;
			_installmentRepository.Update(installment);
			return RedirectToAction("Edit",new { id = id });
		}
		public IActionResult Delete(int id)
		{
           _accountRepository.Delete(id);          
            return RedirectToAction("Index");
		}
	}
}
