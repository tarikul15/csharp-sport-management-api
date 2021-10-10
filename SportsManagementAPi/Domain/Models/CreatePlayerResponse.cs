namespace SportsManagementAPi.Domain.Models
{
    public class CreatePlayerResponse : BaseResponse
    {
        public Player Player { get; private set; }

        public CreatePlayerResponse(bool success, string message, Player player) : base(success, message)
        {
            Player = player;
        }
    }
}
