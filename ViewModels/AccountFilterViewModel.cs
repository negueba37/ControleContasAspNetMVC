using ControleContasData.DTO;

namespace ControleContas.ViewModels
{
	public class AccountFilterViewModel
	{
		public int Month { get; set; }
		public int Year { get; set; }

		public IEnumerable<AccountDTO> Accounts { get; set; } = [];
	}
}
