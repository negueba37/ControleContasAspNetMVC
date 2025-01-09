using ConsoleData.DTO;
using ControleContasData.DTO;
using Shared.Data.Domain;

namespace ControleContas.ViewModels
{
    public class AccountCardViewModel
    {
        public bool IsError { get; set; } = false;
        public AccountDTO Account { get; set; } 
        public IEnumerable<CardDTO> Cards { get; set; } = new List<CardDTO>();
        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
    }
}
