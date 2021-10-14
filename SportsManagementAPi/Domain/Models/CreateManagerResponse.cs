
namespace SportsManagementAPi.Domain.Models
{
    public class CreateManagerResponse : BaseResponse
    {
        public Manager User { get; private set; }

        public CreateManagerResponse(bool success, string message, Manager user) : base(success, message)
        {
            User = user;
        }
    }
}