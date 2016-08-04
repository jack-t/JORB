using Microsoft.VisualStudio.TestTools.UnitTesting;
using JORB.RuleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JORB.Tests;

namespace JORB.RuleProviders.Tests
{
    [TestClass()]
    public class SerializedRulesProviderTests
    {

        [TestMethod()]
        public void PersistTest()
        {
            const string source = "return abc.abc == 4;";
            const string source2 = "return abc.abc != 4;";
            var group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(Thing), "abc") });
            var rule0 = Rule.CreateRule().Named("rule0").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule1 = Rule.CreateRule().Named("rule1").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule2 = Rule.CreateRule().Named("rule2").WithGroup(group).WithSource(source2).LoadNow().Build();

            SerializedRulesProvider srp = new SerializedRulesProvider();
            srp.AddRule(rule0);
            srp.AddRule(rule1);
            srp.AddRule(rule2);
            srp.AddGroup(group);
            var sw = new MemoryStream();
            srp.Persist(sw);
            



        }

        [TestMethod()]
        public void ReadFromFileTest()
        {
            Assert.Fail();
        }
    }
}