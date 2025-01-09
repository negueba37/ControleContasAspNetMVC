using ControleContasData.DTO;

namespace ControleContas.ViewModels
{
	public class BankSlipFilterViewModel
	{
		public int Month { get; set; }
		public int Year { get; set; }
		public IEnumerable<BankSlipDTO> BankSlip { get; set; } = [];
		public Decimal Total() { 
			Decimal total = 0;
			foreach (var item in BankSlip)
				total += item.Price;
			return total;
		}
	}
}
