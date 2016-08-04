using System;

namespace JORB
{
    public class RuleParameter
    {
        public Type ParameterType { get; }
        public string Name { get; }

        public RuleParameter(Type type, string name)
        {
            this.ParameterType = type;
            this.Name = name;
        }

    }
}