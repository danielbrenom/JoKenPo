using JoKenPo.Domain.Enum;

namespace JoKenPo.Domain.Models.Requests
{
    public class PlayerMove
    {
        public string SessionId { get; set; }
        public string PlayerId { get; set; }
        public HandSymbol Move { get; set; }
    }
}