using JORB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORB.Tests
{
    class StubRuleProvider : IRuleProvider
    {
        public StubRuleProvider()
        {
        }

        public Rule GetRule(string ruleName)
        {
            if (rules?.Count > 0) return rules[0];
            return Rule.CreateRule().Named(ruleName).WithGroup("group", this).WithSource("source").Build();
        }

        public RuleGroup GetRuleGroup(string groupName)
        {
            return new RuleGroup(groupName, null);
        }

        public List<Rule> GetRulesByGroup(string groupName)
        {
            return rules;
        }

        private List<Rule> rules;
        
        internal void SetRules(params Rule[] rules1)
        {
            rules = rules1.ToList();
        }
    }
}
