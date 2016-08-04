using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JORB.Tests
{
    [TestClass()]
    public class RuleRunnerTests
    {
        [TestMethod()]
        [ExpectedException(typeof(RuleExecutionException))]
        public void RunRule_ImproperParams_ShouldThrow()
        {
            const string source = "return true;";
            var pm = new StubRuleProvider();
            var group = new RuleGroup("group", null);
            var rule = Rule.CreateRule().Named("rule").WithGroup(group).WithSource(source).Build();
            pm.SetRules(rule);
            RuleRunner runner = new RuleRunner(pm);
            var results = runner.RunRules("group", 4).ToList(); // to list is required for the rules to actually run.
            Assert.IsFalse(results.Single().HasResult);
            Assert.AreEqual("rule", results.Single().RuleName);
            var x = results.Single().Result;
        }


        [TestMethod()]
        public void RulesShouldReturnTrue()
        {
            const string source = "return abc.abc == 4;";
            const string source2 = "return abc.abc != 4;";
            var pm = new StubRuleProvider();
            var group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(Thing), "abc") });

            var rule0 = Rule.CreateRule().Named("rule0").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule1 = Rule.CreateRule().Named("rule1").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule2 = Rule.CreateRule().Named("rule2").WithGroup(group).WithSource(source2).LoadNow().Build();
            pm.SetRules(rule0, rule1, rule2);
            RuleRunner runner = new RuleRunner(pm);


            var res = runner.RunRules("group", new Thing { abc = 4 }); // try with dummy data class
            var en = res.GetEnumerator();

            en.MoveNext();
            Assert.AreEqual("rule0", en.Current.RuleName);
            Assert.AreEqual(true, en.Current.Result);

            en.MoveNext();
            Assert.AreEqual("rule1", en.Current.RuleName);
            Assert.AreEqual(true, en.Current.Result);

            en.MoveNext();
            Assert.AreEqual("rule2", en.Current.RuleName);
            Assert.AreEqual(false, en.Current.Result);
        }

        [TestMethod()]
        public void RulesWithMultipleParameters_ShouldReturnProperly()
        {
            const string source = "return abc.abc == def.abc;";
            const string source2 = "return abc.abc != def.abc;";
            var pm = new StubRuleProvider();
            var group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(Thing), "abc"), new RuleParameter(typeof(Thing), "def") });

            var rule0 = Rule.CreateRule().Named("rule10").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule1 = Rule.CreateRule().Named("rule11").WithGroup(group).WithSource(source2).LoadNow().Build();
            pm.SetRules(rule0, rule1);

            RuleRunner runner = new RuleRunner(pm);
            
            var res = runner.RunRules("group", new Thing { abc = 4 }, new Thing { abc = 4 }); // try with dummy data class
            var en = res.GetEnumerator();

            en.MoveNext();
            Assert.AreEqual("rule10", en.Current.RuleName);
            Assert.AreEqual(true, en.Current.Result);

            en.MoveNext();
            Assert.AreEqual("rule11", en.Current.RuleName);
            Assert.AreEqual(false, en.Current.Result);
        }


        [TestMethod()]
        public void RunRulesWithListTest()
        {
            const string source = "return abc.abc == 4;";
            const string source2 = "return abc.abc != 4;";
            var pm = new StubRuleProvider();
            var group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(Thing), "abc") });

            var rule0 = Rule.CreateRule().Named("rule0").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule1 = Rule.CreateRule().Named("rule1").WithGroup(group).WithSource(source).LoadNow().Build();
            var rule2 = Rule.CreateRule().Named("rule2").WithGroup(group).WithSource(source2).LoadNow().Build();
            pm.SetRules(rule0, rule1, rule2);
            RuleRunner runner = new RuleRunner(pm);


            var res = runner.RunRulesWithList("group", new List<Tuple<string, object>> { Tuple.Create("abc", (object)new Thing { abc = 4 }) }); // try with dummy data class
            var en = res.GetEnumerator();

            en.MoveNext();
            Assert.AreEqual("rule0", en.Current.RuleName);
            Assert.AreEqual(true, en.Current.Result);

            en.MoveNext();
            Assert.AreEqual("rule1", en.Current.RuleName);
            Assert.AreEqual(true, en.Current.Result);

            en.MoveNext();
            Assert.AreEqual("rule2", en.Current.RuleName);
            Assert.AreEqual(false, en.Current.Result);
        }
    }
    public class Thing
    {
        public int abc;
    }
}