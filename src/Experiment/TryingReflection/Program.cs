using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Refl = System.Reflection;
using Emit = System.Reflection.Emit;
using IO = System.IO;

namespace TryingReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "hello.exe";
            var asm_name = new Refl.AssemblyName(IO.Path.GetFileNameWithoutExtension(name));//no extension for assembly name
            var asml = System.AppDomain.CurrentDomain.DefineDynamicAssembly(asm_name, Emit.AssemblyBuilderAccess.Save);
            var modl = asml.DefineDynamicModule(name);//extension is needed for module name
            var prog = modl.DefineType("Program");

            Emit.MethodBuilder main = prog.DefineMethod("Main", Refl.MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);
            var generator = main.GetILGenerator();
            generator.Emit(Emit.OpCodes.Ldstr, "hello~ I'm Yue");
            generator.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] { typeof(string) }));//?

            generator.Emit(Emit.OpCodes.Ret);
            prog.CreateType();
            modl.CreateGlobalFunctions();
            asml.SetEntryPoint(main);
            asml.Save(name);
        }
    }
}
