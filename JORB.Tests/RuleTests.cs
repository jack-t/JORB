using Microsoft.VisualStudio.TestTools.UnitTesting;
using JORB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORB.Tests
{
    [TestClass()]
    public class RuleTests
    {
        [TestMethod()]
        public void Rule_Version_GivenNewRule_ShouldReturn_0()
        {
            var source = "return true;";
            var group = new RuleGroup("group", null);
            var rule0 = Rule.CreateRule().Named("rule_rule0").WithGroup(group).WithSource(source).LoadNow().Build();

            Assert.AreEqual(0, rule0.Version);
        }



    }

    [TestClass()]
    public class RuleVersionTests
    {
        private static string source = "return true;";
        private static RuleGroup group = new RuleGroup("group", null);
        private static Rule rule_rule1 = null;

        [ClassInitialize]
        public static void ClassInit(TestContext t)
        {
            rule_rule1 = Rule.CreateRule().Named("rule_rule1").WithGroup(group).WithSource(source).LoadNow().Build();
        }

        [TestMethod()]
        public void Rules_GivenTwoRules_VersionShouldReturn_0_Then_1()
        {
            Assert.AreEqual(0, rule_rule1.Version);
            Step2();
        }
        private void Step2()
        {
            var rule0_1 = Rule.CreateRule().Named("rule_rule1").WithGroup(@group).WithSource(source).LoadNow().Build();

            Assert.AreEqual(1, rule0_1.Version);
            Assert.AreEqual(0, rule_rule1.Version);
        }
    }
}