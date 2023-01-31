using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Domain.Results
{
    public class BaseResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
    }
}
