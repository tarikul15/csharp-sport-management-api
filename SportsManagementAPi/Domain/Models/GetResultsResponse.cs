using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsManagementAPi.Domain.Models
{
    public class GetResultsResponse : BaseResponse
    {
        public List<Result> Results { get; private set; }

        public GetResultsResponse(bool success, string message, List<Result> results) : base(success, message)
        {
            Results = results;
        }
    }
}
