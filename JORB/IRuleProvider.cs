using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORB
{
    public interface IRuleProvider
    {
        Rule GetRule(string ruleName);
        List<Rule> GetRulesByGroup(string groupName);
        RuleGroup GetRuleGroup(string groupName);
    }
}
