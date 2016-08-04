using System.Collections.Generic;
using System.Linq;
using JORB;

namespace Demo.Services
{
    public interface IRuleRepository : IRepository<Rule, string>
    {
        IEnumerable<RuleExecutionResult> Run(string group, object[] parameters);
    }

    public class RuleRepository : IRuleRepository, IRuleProvider
    {
        private static Dictionary<string, Rule> _rules = new Dictionary<string, Rule>();

        internal static void Seed()
        {
            var group = new GroupRepository().GetFor("group");
            string source = "return true;";
            _rules.Add("rule0", Rule.CreateRule().Named("rule0").WithGroup(group).WithSource(source).LoadNow().Build());
            _rules.Add("rule1", Rule.CreateRule().Named("rule1").WithGroup(group).WithSource(source).LoadNow().Build());
            _rules.Add("rule2", Rule.CreateRule().Named("rule2").WithGroup(group).WithSource(source).LoadNow().Build());

        }

        public IQueryable<Rule> GetAll()
        {
            return _rules.Values.AsQueryable();
        }

        public Rule GetFor(string param)
        {
            return _rules[param];
        }

        public void Add(Rule t)
        {
            _rules[t.RuleCode] = t;
        }

        public void Remove(Rule t)
        {
            _rules.Remove(t.RuleCode);
        }


        public IEnumerable<RuleExecutionResult> Run(string group, object[] parameters)
        {
            RuleRunner runner = new RuleRunner(this);
            return runner.RunRules(group, parameters);
        }

        public Rule GetRule(string ruleName)
        {
            return GetFor(ruleName);
        }

        public List<Rule> GetRulesByGroup(string groupName)
        {
            return _rules.Where(r => r.Value.Group.GroupName.Equals(groupName)).Select(t => t.Value).ToList();
        }

        public RuleGroup GetRuleGroup(string groupName)
        {
            return new GroupRepository().GetFor(groupName);
        }
    }
}