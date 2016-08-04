using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace JORB.RuleProviders
{
    [Serializable]
    public class SerializedRulesProvider : IRuleProvider
    {

        private List<Rule> _rules = new List<Rule>();
        private List<RuleGroup> _groups = new List<RuleGroup>();


        public void AddRule(Rule rule)
        {
            _rules.Add(rule);
        }

        public void AddGroup(RuleGroup group)
        {
            _groups.Add(group);
        }

        public void Persist(Stream output)
        {
            Contract.Assert(output.CanWrite);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(output, this);
        }

        public static IRuleProvider ReadFromFile(Stream input)
        {
            Contract.Assert(input.CanRead);
            BinaryFormatter formatter = new BinaryFormatter();
            return (SerializedRulesProvider)formatter.Deserialize(input);
        }

        public List<Rule> GetRulesByGroup(string groupName)
        {
            return _rules.Where(r => r.Group.GroupName.Equals(groupName)).ToList();
        }

        public RuleGroup GetRuleGroup(string groupName)
        {
            return _groups.First(g => g.GroupName.Equals(groupName));
        }
    }
}