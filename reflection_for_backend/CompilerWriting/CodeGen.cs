using Collections = System.Collections.Generic;
using Reflect = System.Reflection;
using Emit = System.Reflection.Emit;
using IO = System.IO;

public sealed class CodeGen
{
    Emit.ILGenerator il = null;
    Collections.Dictionary<string, Emit.LocalBuilder> symbolTable;

    public CodeGen(Stmt stmt, string moduleName)
    {
        if (IO.Path.GetFileName(moduleName) != moduleName)
        {
            throw new System.Exception("can only output into current directory!");
        }

        Reflect.AssemblyName name = new Reflect.AssemblyName(IO.Path.GetFileNameWithoutExtension(moduleName));
        Emit.AssemblyBuilder asmb = System.AppDomain.CurrentDomain.DefineDynamicAssembly(name, Emit.AssemblyBuilderAccess.Save);
        Emit.ModuleBuilder modb = asmb.DefineDynamicModule(moduleName);
        Emit.TypeBuilder typeBuilder = modb.DefineType("Foo");

        Emit.MethodBuilder methb = typeBuilder.DefineMethod("Main", Reflect.MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);

        // CodeGenerator
        this.il = methb.GetILGenerator();
        this.symbolTable = new Collections.Dictionary<string, Emit.LocalBuilder>();

        // Go Compile!
        this.GenStmt(stmt);

        il.Emit(Emit.OpCodes.Ret);
        typeBuilder.CreateType();
        modb.CreateGlobalFunctions();
        asmb.SetEntryPoint(methb);
        asmb.Save(moduleName);
        this.symbolTable = null;
        this.il = null;
    }


	private void GenStmt(Stmt stmt)
	{
		if (stmt is Sequence)
		{
			Sequence seq = (Sequence)stmt;
			this.GenStmt(seq.First);
			this.GenStmt(seq.Second);
		}		
        
        else if (stmt is DeclareVar)
		{
			// declare a local
			DeclareVar declare = (DeclareVar)stmt;
			this.symbolTable[declare.Ident] = this.il.DeclareLocal(this.TypeOfExpr(declare.Expr));

			// set the initial value
			Assign assign = new Assign();
			assign.Ident = declare.Ident;
			assign.Expr = declare.Expr;
			this.GenStmt(assign);
		}        
        
        else if (stmt is Assign)
        {
	        Assign assign = (Assign)stmt;
	        this.GenExpr(assign.Expr, this.TypeOfExpr(assign.Expr));
	        this.Store(assign.Ident, this.TypeOfExpr(assign.Expr));
        }			
        else if (stmt is Print)
		{
			// the "print" statement is an alias for System.Console.WriteLine. 
			// it uses the string case
			this.GenExpr(((Print)stmt).Expr, typeof(string));
			this.il.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] { typeof(string) }));
		}	

		else if (stmt is ReadInt)
		{
			this.il.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("ReadLine", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new System.Type[] { }, null));
			this.il.Emit(Emit.OpCodes.Call, typeof(int).GetMethod("Parse", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new System.Type[] { typeof(string) }, null));
			this.Store(((ReadInt)stmt).Ident, typeof(int));
		}
		else if (stmt is ForLoop)
		{
			// example: 
			// for x = 0 to 100 do
			//   print "hello";
			// end;

			// x = 0
			ForLoop forLoop = (ForLoop)stmt;
			Assign assign = new Assign();
			assign.Ident = forLoop.Ident;
			assign.Expr = forLoop.From;
			this.GenStmt(assign);			
			// jump to the test
			Emit.Label test = this.il.DefineLabel();
			this.il.Emit(Emit.OpCodes.Br, test);

			// statements in the body of the for loop
			Emit.Label body = this.il.DefineLabel();
			this.il.MarkLabel(body);
			this.GenStmt(forLoop.Body);

			// to (increment the value of x)
			this.il.Emit(Emit.OpCodes.Ldloc, this.symbolTable[forLoop.Ident]);
			this.il.Emit(Emit.OpCodes.Ldc_I4, 1);
			this.il.Emit(Emit.OpCodes.Add);
			this.Store(forLoop.Ident, typeof(int));

			// **test** does x equal 100? (do the test)
			this.il.MarkLabel(test);
			this.il.Emit(Emit.OpCodes.Ldloc, this.symbolTable[forLoop.Ident]);
			this.GenExpr(forLoop.To, typeof(int));
			this.il.Emit(Emit.OpCodes.Blt, body);
		}
		else
		{
			throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
		}




	}    
    	
    private void Store(string name, System.Type type)
	{
		if (this.symbolTable.ContainsKey(name))
		{
			Emit.LocalBuilder locb = this.symbolTable[name];

			if (locb.LocalType == type)
			{
				this.il.Emit(Emit.OpCodes.Stloc, this.symbolTable[name]);
			}
			else
			{
				throw new System.Exception("'" + name + "' is of type " + locb.LocalType.Name + " but attempted to store value of type " + type.Name);
			}
		}
		else
		{
			throw new System.Exception("undeclared variable '" + name + "'");
		}
	}



    private void GenExpr(Expr expr, System.Type expectedType)
	{
		System.Type deliveredType;
		
        if (expr is StringLiteral)
		{
			deliveredType = typeof(string);
			this.il.Emit(Emit.OpCodes.Ldstr, ((StringLiteral)expr).Value);
		}
		else if (expr is IntLiteral)
		{
			deliveredType = typeof(int);
			this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)expr).Value);
		}        
        else if (expr is Variable)
		{
			string ident = ((Variable)expr).Ident;
			deliveredType = this.TypeOfExpr(expr);

			if (!this.symbolTable.ContainsKey(ident))
			{
				throw new System.Exception("undeclared variable '" + ident + "'");
			}

			this.il.Emit(Emit.OpCodes.Ldloc, this.symbolTable[ident]);
		}
		else
		{
			throw new System.Exception("don't know how to generate " + expr.GetType().Name);
		}

        if (deliveredType != expectedType)
        {
            if (deliveredType == typeof(int) &&
                expectedType == typeof(string))
            {
                this.il.Emit(Emit.OpCodes.Box, typeof(int));
                this.il.Emit(Emit.OpCodes.Callvirt, typeof(object).GetMethod("ToString"));
            }
            else
            {
                throw new System.Exception("can't coerce a " + deliveredType.Name + " to a " + expectedType.Name);
            }
        }

	}



    private System.Type TypeOfExpr(Expr expr)
	{
		if (expr is StringLiteral)
		{
			return typeof(string);
		}
		else if (expr is IntLiteral)
		{
			return typeof(int);
		}
		else if (expr is Variable)
		{
            Variable var = (Variable)expr;
		    if (this.symbolTable.ContainsKey(var.Ident))
		    {
			    Emit.LocalBuilder locb = this.symbolTable[var.Ident];
			    return locb.LocalType;
		    }
		    else
		    {
			    throw new System.Exception("undeclared variable '" + var.Ident + "'");
		    }
		}
		else
		{
			throw new System.Exception("don't know how to calculate the type of " + expr.GetType().Name);
		}
	}	
}
