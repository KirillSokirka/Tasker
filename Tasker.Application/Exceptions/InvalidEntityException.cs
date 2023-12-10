using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasker.Application.Exceptions
{
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException(string? message) : base(message)
        {
        }
    }
}
