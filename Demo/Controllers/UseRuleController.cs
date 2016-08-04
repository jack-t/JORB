using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Services;
using JORB;
using Microsoft.AspNetCore.Mvc;


namespace Demo.Controllers
{
    public class UseRuleController : Controller
    {
        private IGroupRepository _groups;
        private IRuleRepository _rules;

        public UseRuleController(IRuleRepository _rules, IGroupRepository _groups)
        {
            this._rules = _rules;
            this._groups = _groups;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Run(string group, List<string> parameters)
        {
            if (_groups.GetFor(group) == null) return StatusCode(400, "No such group");

            var results = _rules.Run(group, parameters.ToArray());

            return PartialView("_RunResults", results);
        }
    }
}
