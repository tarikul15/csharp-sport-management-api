using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class PatchResultResponse : BaseResponse
    {
        public Result Result { get; private set; }

        public PatchResultResponse(bool success, string message, Result result) : base(success, message)
        {
            Result = result;
        }
    }
}
