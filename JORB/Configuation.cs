using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORB
{
    public class Configuation
    {

        private Configuation()
        {
        }

        public static Configuation Instance { get; private set; } = new Configuation();

        public List<string> DefaultUsingNamespaces => new List<string> { "System", "System.Text" };
    }
}
