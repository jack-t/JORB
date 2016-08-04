using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models.ViewModels;
using Demo.Services;
using JORB;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private IRuleRepository _ruleRepository;
        private IGroupRepository _groupRepository;

        public HomeController(IRuleRepository repository, IGroupRepository groups)
        {
            _ruleRepository = repository;
            _groupRepository = groups;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel vmodel = new HomeIndexViewModel(_ruleRepository.GetAll().ToList(), _groupRepository.GetAll().ToList());

            return View(vmodel);
        }

        [HttpPost]
        public IActionResult DeleteRule(string ruleName)
        {
            _ruleRepository.Remove(_ruleRepository.GetFor(ruleName));
            return CurrentRules();
        }
        [HttpPost]
        public IActionResult DeleteGroup(string groupName)
        {
            _groupRepository.Remove(_groupRepository.GetFor(groupName));
            return CurrentGroups();
        }

        public IActionResult CurrentRules()
        {
            return PartialView("_CurrentRules", _ruleRepository.GetAll().ToList());
        }
        public IActionResult CurrentGroups()
        {
            return PartialView("_CurrentGroups", _groupRepository.GetAll().ToList());
        }
        [HttpPost]
        public IActionResult CreateRule(string code, string group, string source)
        {
            RuleGroup rgroup = _groupRepository.GetFor(group);

            if (rgroup == null)
            {
                return StatusCode(400, "Invalid Group Code");
            }

            Rule rule = Rule.CreateRule().Named(code).WithGroup(rgroup).WithSource(source).Build();

            _ruleRepository.Add(rule);

            return CurrentRules();
        }

        [HttpPost]
        public IActionResult CreateGroup(string name, List<RuleParameterMiddleMan> parameters)
        {
            RuleGroup group = new RuleGroup(name, parameters.Select(param => param.OutgoingRuleParameter).ToList()); // performs the type lookups
            _groupRepository.Add(group);

            return CurrentGroups();
        }
    }

    // Excelllent/horrible naming courtesy Rob Dusseau :)
    public class RuleParameterMiddleMan
    {
        public string Name { get; set; }
        public string FullyQualifiedTypeName { get; set; }

        public RuleParameter OutgoingRuleParameter => new RuleParameter(Type.GetType(FullyQualifiedTypeName), Name);
    }
}
