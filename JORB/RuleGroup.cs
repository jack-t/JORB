using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JORB
{
    public class RuleGroup
    {
        public string GroupName { get; }

        public List<RuleParameter> Parameters { get; }

        public string RenderedParameters
        {
            get
            {
                if (Parameters == null || Parameters.Count == 0) return string.Empty;
                return string.Join(", ", Parameters.Select(p => p.ParameterType.FullName + " " + p.Name));
            }
        }

        public RuleGroup(string name, List<RuleParameter> parameters)
        {
            GroupName = name;
            Parameters = parameters;
        }
    }
}
