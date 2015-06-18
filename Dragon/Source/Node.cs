//
//  corresponds to Inter package in dragon book
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dragon
{
    public class Node
    {
        int _lexLine;
        static int _labels = 0;

        public Node() //private on book
        { 
            _lexLine = Lexer.Line; 
        }

        void Error(string msg)
        {
            throw new Exception("near line " + _lexLine + ": " + msg);
        }

        public int NewLable()
        {
            return ++Node._labels;
        }

        public void EmitLabel(int i)
        {
            Console.Write("L" + i + ":");
        }

        public void Emit(string s)
        {
            Console.Write("\t" + s);
        }
    }


    public class Expr : Node
    {
        public Token Op { get; set; }
        public Type Type { get; set; }

        public Expr(Token tok, Type type) //private on book
        {
            this.Op = tok;
            this.Type = type;
        }

        public Expr Gen()
        {
            return this;
        }

        public Expr Reduce()
        {
            return this;
        }

        public void Jumping(int t, int f)
        {
            this.EmitJumps(this.ToString(), t, f);
        }

        public void EmitJumps(string test, int lineForTrue, int lineForFalse)
        {
            if(lineForTrue != 0 && lineForFalse != 0)
            {
                this.Emit("if " + test + " goto L" + lineForTrue); 
                this.Emit("goto L" + lineForFalse);
            }
            else if(lineForTrue != 0)
            {
                this.Emit("if " + test + " goto L" + lineForTrue);
            }
            else if(lineForFalse != 0)
            {
                this.Emit("iffalse " + test + " goto L" + lineForFalse);
            }
            else
            {
                ;
            }
        }

        public override string ToString()
        {
            return this.Op.ToString();
        }
    }
}
