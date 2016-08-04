using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics.Contracts;

namespace JORB
{
    /// <summary>
    /// A rule as retreived from the database and made ready to use.
    /// Everything required is compiled.
    /// </summary>
    public class Rule
    {

        public string Source { get; private set; }
        public string RuleCode { get; private set; }
        public RuleGroup Group { get; private set; }

        private readonly RuleMethod _ruleMethod;
        public int Version
        {
            get
            {

                string declared = _ruleMethod.DeclaredClass;
                if (declared != null)
                    return int.Parse(declared.Substring(declared.LastIndexOf('_') + 1));

                var them = AppDomain.CurrentDomain.GetAssemblies();
                var selectmany = them.SelectMany(assem => assem.GetTypes()).ToList();
                var where = selectmany.Where(t => t.Name.StartsWith(SimpleClassName)).ToList();
                var select = @where.Select(t => int.Parse(t.Name.Substring(t.Name.LastIndexOf('_') + 1))).ToList();
                if (!@select.Any()) return 0;
                else return @select.Union(new List<int> { 0 }).Max() + 1;
            }
        }
        // Possibly add an ExtraUsingNamespaces property; add also to UsingStatements

        private Rule()
        {
            _ruleMethod = new RuleMethod(this);
        }

        public bool ApplyRule(params object[] parameters)
        {
            try
            {
                return _ruleMethod.Run(parameters);
            }
            catch (Exception ex)
            {
                throw new RuleExecutionException("Rule execution failed", ex); // possibly try to distinguish between a problem calling the method and an exception raised by the method?
            }
        }

        public string FullSource => $@"
                {UsingStatements}
                namespace JORB {{
                    public class {ClassName} {{
                        public static bool Run({Group.RenderedParameters}) {{
                            {Source}
                        }}
                    }}
                }}
            ";

        public string ClassName => SimpleClassName + "_" + Version;
        private string SimpleClassName => "Rule_" + RuleCode + "_" + (Group.GroupName ?? "");

        public string UsingStatements
        {
            get
            {
                return string.Join("\n", Configuation.Instance.DefaultUsingNamespaces.Select(ns => "using " + ns + ";"));
            }
        }

        #region Builder

        public static Builder CreateRule()
        {
            return new BuilderBuilder();
        }

        public class Builder
        {
            private bool _loadNow;
            private Rule _rule;

            protected Builder()
            {
                _loadNow = false;
                _rule = new Rule();
            }

            public Builder Named(string name)
            {
                Contract.Requires(!string.IsNullOrEmpty(name));
                _rule.RuleCode = name; return this;
            }
            public Builder WithGroup(RuleGroup group)
            {
                Contract.Requires(group != null);
                _rule.Group = group; return this;
            }
            public Builder WithGroup(string group, IRuleProvider manager)
            {
                Contract.Requires(!string.IsNullOrEmpty(group) && manager != null);
                _rule.Group = manager.GetRuleGroup(group); return this;
            }
            public Builder WithSource(string source)
            {
                Contract.Requires(!string.IsNullOrEmpty(source));
                _rule.Source = source; return this;
            }
            public Builder LoadNow()
            {
                _loadNow = true;
                return this;
            }
            public Rule Build()
            {
                if (_loadNow)
                    _rule._ruleMethod.Load();
                return _rule;
            }

        }

        /// <summary>
        /// This class is to allow Builder's constructor to be hidden and yet allow access to CreateRule()
        /// </summary>
        private class BuilderBuilder : Builder
        {
        }
        #endregion
    }
}
