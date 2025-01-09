using ConsoleData.DTO;
using ControleContas.ViewModels;
using ControleContasData.DTO;
using ControleContasData.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Repositories.Interfaces;

namespace ControleContas.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountRepository _accountRepository;
		private readonly ICardRepository _cardRepository;
		private readonly IUserRepository _userRepository;
		private readonly IInstalmentRepository _installmentRepository;

		public AccountController(IAccountRepository accountRepository,
			ICardRepository cardRepository, 
			IUserRepository userRepository, 
			IInstalmentRepository installmentRepository)
		{
			_accountRepository = accountRepository;
			_cardRepository = cardRepository;
			_userRepository = userRepository;
			_installmentRepository = installmentRepository;
			
		}

		public IActionResult Index(int month = 0, int year = 0)
		{
			if (month == 0)
				month = DateTime.Now.Month;
			if (year == 0)
				year = DateTime.Now.Year;

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
