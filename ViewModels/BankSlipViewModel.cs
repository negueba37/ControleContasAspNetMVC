using ConsoleData.DTO;
using ControleContasData.DTO;

namespace ControleContas.ViewModels
{
	public class BankSlipViewModel
	{
		public bool IsError { get; set; } = false;
		public BankSlipDTO BankSlip { get; set; }
		public IEnumerable<CardDTO> Cards { get; set; } = new List<CardDTO>();
		public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
	}
}
