namespace JoKenPo.Domain.Models.Requests
{
    public class PlayerRequest : Player
    {
        public string SessionId { get; set; }
    }
}