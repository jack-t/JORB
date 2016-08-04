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
    public class RuleGroupTests
    {

        [TestMethod()]
        public void RenderedParameters_ReturnsEmptyString_GivenEmptyParameters()
        {
            RuleGroup group = new RuleGroup("group", new List<RuleParameter>());
            Assert.AreEqual("", group.RenderedParameters);
        }

        [TestMethod()]
        public void RenderedParameters_ReturnsEmptyString_GivenNullParameters()
        {
            RuleGroup group = new RuleGroup("group", null);
            Assert.AreEqual("", group.RenderedParameters);
        }

        [TestMethod()]
        public void RenderedParameters_ReturnsProperParameters_GivenSingleParameter()
        {
            RuleGroup group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(int), "abc") });
            Assert.AreEqual("System.Int32 abc", group.RenderedParameters);
        }

        [TestMethod()]
        public void RenderedParameters_ReturnsProperParameters_GivenMultipleParameters()
        {
            RuleGroup group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(int), "abc"), new RuleParameter(typeof(string), "def") });

            Assert.AreEqual("System.Int32 abc, System.String def", group.RenderedParameters);
        }
        [TestMethod()]
        public void RenderedParameters_ReturnsProperParameters_GivenMultipleThingParameters()
        {
            RuleGroup group = new RuleGroup("group", new List<RuleParameter> { new RuleParameter(typeof(int), "abc"), new RuleParameter(typeof(Thing), "def") });

            Assert.AreEqual("System.Int32 abc, JORB.Tests.Thing def", group.RenderedParameters);
        }
    }
}