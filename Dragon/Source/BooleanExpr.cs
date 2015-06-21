using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon
{
    public class Constant : Expr
    {
        public Constant (Token tok, Type type)
            : base(tok,type)
        { }

        public Constant(int i)
            : base(new Num(i), Dragon.Type.Int)
        { }

        public static readonly Constant
            True = new Constant(Word.True, Dragon.Type.Bool),
            False = new Constant(Word.False, Dragon.Type.Bool);

        public override void Jumping(int t, int f)
        {
            if (this == Constant.True && t != 0)
                this.Emit("goto L" + t);
            else if(this == Constant.False && f != 0)
                this.Emit("goto L" + f);
        }
    }
}
