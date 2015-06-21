using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon
{
    public class Stmt : Node
    {
        public Stmt() { }
        public static Stmt Null = new Stmt();

        public virtual void Gen(int beginning, int after) { }
        public int After = 0;
        public static Stmt Enclosing = Stmt.Null;
    }


    public class If : Stmt
    {
        public Expr Expr;
        public Stmt Stmt;

        public If(Expr expr, Stmt stmt)
        {
            this.Expr = expr;
            this.Stmt = stmt;
            if(this.Expr.Type != Type.Bool)
                this.Expr.Error("boolean required in if");
        }

        public override void Gen (int beginning, int after)
        {
            int lable = this.NewLable();    //label for the code of "for"
            this.Expr.Jumping(0, after);    //fall through on true, goto "after" on false
            this.EmitLabel(lable);
            this.Stmt.Gen(lable, after);
        }
    }


    public class Else : Stmt
    {
        public Expr Expr;
        public Stmt Stmt1, Stmt2;

        public Else(Expr expr, Stmt stmt1, Stmt stmt2)
        {
            this.Expr = expr;
            this.Stmt1 = stmt1;
            this.Stmt2 = stmt2;
            if (this.Expr.Type != Dragon.Type.Bool)
                this.Expr.Error("boolean required in if");
        }

        public override void Gen(int beginning, int after)
        {
            int label1 = this.NewLable();
            int lable2 = this.NewLable();
            this.Expr.Jumping(0, lable2);

            this.EmitLabel(label1);
            this.Stmt1.Gen(label1, after);
            this.Emit("goto L" + after);

            this.EmitLabel(lable2);
            this.Stmt2.Gen(lable2, after);
        }
    }


    public class While : Stmt
    {
        public Expr Expr;
        public Stmt Stmt;

        public While()
        {
            this.Expr = null;
            this.Stmt = null;
        }

        public void Init(Expr expr, Stmt stmt)
        {
            this.Expr = expr;
            this.Stmt = stmt;
            if (this.Expr.Type != Type.Bool)
                this.Expr.Error("boolean requried in while");
        }

        public override void Gen(int beginning, int after)
        {
            this.After = after;             //save after
            this.Expr.Jumping(0, after);
            int label = this.NewLable();    //label for stmt
            this.EmitLabel(label);
            this.Stmt.Gen(label, beginning);
            this.Emit("goto L " + beginning);
        }
    }

    public class Do : Stmt
    {
        public Expr Expr;
        public Stmt Stmt;

        public Do()
        {
            this.Expr = null;
            this.Stmt = null;
        }

        public void Init(Stmt stmt, Expr expr)
        {
            this.Expr = expr;
            this.Stmt = stmt;
            if (this.Expr.Type != Type.Bool)
                this.Expr.Error("boolean requried in do");
        }

        public override void Gen(int beginning, int after)
        {
            this.After = after;
            int label = this.NewLable();
            this.Stmt.Gen(beginning, label);
            this.EmitLabel(label);
            this.Expr.Jumping(beginning, 0);
        }
    }
}
