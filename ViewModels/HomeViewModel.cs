using ConsoleData.DTO;
using Shared.Data.Repositories.Interfaces;

namespace ControleContas.ViewModels
{
	public class HomeViewModel
	{
		public int Month { get; set; }
        public int Year { get; set; }
        public int Cartao { get; set; } = 0;
        public IEnumerable<InstallmentDTO> Installments { get; set; } = [];
        public IEnumerable<InstallmentDTO> InstallmentsBankSlip { get; set; } = [];
        public Decimal TotalInstallment()
        {
            decimal totalInstallment = 0;
            foreach (var installment in Installments) {
                totalInstallment += installment.Price;
            }
            return totalInstallment;
        }
        public Decimal TotalExpenses() 
        {            
            return TotalInstallment() + TotalInstallmentBankSlip();
        }
		public Decimal TotalInstallmentBankSlip()
		{
			decimal totalInstallment = 0;
			foreach (var installment in InstallmentsBankSlip)
			{
				totalInstallment += installment.Price;
			}
			return totalInstallment;
		}
		public Decimal TotalInstallmentCard(int id) 
        {
            decimal totalInstallmentCard = 0;
            foreach (var installment in Installments)
            {
                if (installment.Account.Card.Id == id) 
                {
                    totalInstallmentCard += installment.Price;
                }
            }
            return totalInstallmentCard;
        }
        public IEnumerable<CardDTO> Cards() 
        {
            var listaIdCards = new List<int>();
            var listaCards = new List<CardDTO>();
            foreach (var installment in Installments)
            {
                if (!listaIdCards.Contains(installment.Account.Card.Id))
                {
                    listaIdCards.Add(installment.Account.Card.Id);
                    listaCards.Add(installment.Account.Card);
                }
            }
            return listaCards;
        }
    }
}
