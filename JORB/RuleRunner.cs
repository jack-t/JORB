using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORB
{
    /// <summary>
    /// Some sort of Rules service (?) creates these. This is handled by clients of this application; they are responsible for wiring the persistence together with this class.
    /// </summary>
    public class RuleRunner
    {

        public IRuleProvider Provider { get; }

        public RuleRunner(IRuleProvider manager)
        {
            Provider = manager;
        }

        public IEnumerable<RuleExecutionResult> RunRules(string ruleGroup, params object[] parameters)
        {
            List<Rule> rules = Provider.GetRulesByGroup(ruleGroup);
            foreach (Rule rule in rules)
            {
                bool? res = null;
                RuleExecutionException ex = null;
                try
                {
                    res = rule.ApplyRule(parameters);
                }
                catch (RuleExecutionException e)
                {
                    ex = e;
                } // eat this exception. (maybe R.E.R. should maintain a copy of the exception?)
                yield return new RuleExecutionResult(rule.RuleCode, res, ex);
            }
        }

        /// <summary>
        /// The idea here is that more convoluted calls would benefit from having named parameters.
        /// </summary>
        public IEnumerable<RuleExecutionResult> RunRulesWithList(string ruleGroup, List<Tuple<string, object>> parameters)
        {
            return RunRules(ruleGroup, parameters.Select(p => p.Item2).ToArray());
        }

    }
}
