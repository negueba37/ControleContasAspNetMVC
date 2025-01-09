using ConsoleData.DTO;
using ControleContasData.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Repositories.Interfaces;
using System;

namespace ControleContas.Controllers
{
	public class CardController : Controller
	{
		private readonly ICardRepository _cardRepository;

		public CardController(ICardRepository cardRepository)
		{
			_cardRepository = cardRepository;
		}

		public IActionResult Index()
		{			
			var listCard = _cardRepository.Cards();
			return View(listCard);
		}	
		public IActionResult Edit(int id)
		{
			CardService cardService = new CardService();
			var card = cardService.GetById(id);
			return View(card);
		}
		[HttpPost]
		public IActionResult Edit(CardDTO cardDTO)
		{
			if (ModelState.IsValid) 
			{
				CardService cardService = new CardService();
                TimeZoneInfo fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                cardService.Update(cardDTO);
				return RedirectToAction("Index");
			}
			return View(cardDTO);

		}
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(CardDTO cardDTO)
        {
            if (ModelState.IsValid)
            {
                CardService cardService = new CardService();
                cardService.Save(cardDTO);
                return RedirectToAction("Index");
            }
            return View(cardDTO);
        }
        public IActionResult Delete(int id)
		{
            CardService cardService = new CardService();
            cardService.Delete(id);          
            return RedirectToAction("Index");
		}
	}
}
