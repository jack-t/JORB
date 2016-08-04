using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace JORB
{
    /// <summary>
    /// Pulled, slightly modified, from StackOverflow.
    /// </summary>
    /// <see cref="http://stackoverflow.com/questions/3023900/compiling-code-at-runtime-loading-into-current-appdomain-but-type-gettype-cant"/>
    /// <permission cref="Richard Friend"></permission>
    public static class RuntimeCodeCompiler
    {
        private static Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();
        static RuntimeCodeCompiler()
        {

            //AppDomain.CurrentDomain.AssemblyLoad += (sender, e) =>
            //{
            //    _assemblies[e.LoadedAssembly.FullName] = e.LoadedAssembly;
            //};
            //AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            //{
            //    Assembly assembly = null;
            //    _assemblies.TryGetValue(e.Name, out assembly);
            //    return assembly;
            //};
        }


        public static Assembly CompileCode(string code)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            //ICodeCompiler compiler = provider.CreateCompiler();
            var compiler = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compilerparams = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = false

            };

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    string location = assembly.Location;
                    if (!string.IsNullOrEmpty(location))
                    {
                        compilerparams.ReferencedAssemblies.Add(location);
                    }
                }
                catch (NotSupportedException)
                {
                    // this happens for dynamic assemblies, so just ignore it. 
                }
            }

            CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
                foreach (CompilerError error in results.Errors)
                {
                    errors.Append($"Line {error.Line},{error.Column}\t: {error.ErrorText}\n");
                }
                throw new Exception(errors.ToString());
            }
            else
            {
                AppDomain.CurrentDomain.Load(results.CompiledAssembly.GetName());
                return results.CompiledAssembly;
            }
        }
    }
}