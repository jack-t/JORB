using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JORB;

namespace Demo.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Rule> CurrentRules { get; private set; }
        public IEnumerable<RuleGroup> CurrentGroups { get; private set; }

        public HomeIndexViewModel(IEnumerable<Rule> rules, IEnumerable<RuleGroup> groups)
        {
            CurrentRules = rules;
            CurrentGroups = groups;
        }
    }
}
