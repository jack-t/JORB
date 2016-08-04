using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JORB
{
    internal class RuleMethod
    {
        private Rule _rule;
        private MethodInfo _method;

        public RuleMethod(Rule rule)
        {
            _rule = rule;
            _method = null;
        }

        public string DeclaredClass
        {
            get { return _method?.DeclaringType.FullName; }
        }

        private string FindRuleName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name + "." + _rule.ClassName;
        }

        public void Load()
        {
            var name = FindRuleName();
            if (!AssignExistingMethod()) return;
            // the method still hasn't been loaded and couldn't be found; load method
            var assembly = Compile();

            _method = assembly.GetType(name).GetMethod("Run"); // ensures that the just-compiled assembly is the one whose method is referenced.
        }

        private bool AssignExistingMethod()
        {
            if (_method != null) return false;

            _method = AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(a => a.GetType(FindRuleName()))
                .SingleOrDefault(t => t != null)
                ?.GetMethod("Run"); // ?. because singleordefault returns null in case of default.

            return _method == null;
        }

        private Assembly Compile()
        {
            // the current type names that match this rule.
           

            return RuntimeCodeCompiler.CompileCode(_rule.FullSource);
        }

        public bool Run(params object[] parameters)
        {
            Load();
            return (bool)_method.Invoke(null, parameters);
        }
    }
}