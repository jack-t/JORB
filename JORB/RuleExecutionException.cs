using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORB
{
    public class RuleExecutionException : Exception
    {
        public RuleExecutionException(string message) : base(message)
        {
        }

        public RuleExecutionException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
