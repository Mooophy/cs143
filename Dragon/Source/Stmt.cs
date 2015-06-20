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
}
