using JoKenPo.Domain.Enum;

namespace JoKenPo.Domain.Models
{
    public class PlayerTurn
    {
        public string PlayerId { get; set; }
        public HandSymbol PlayerMove { get; set; }
    }
}