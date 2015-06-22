using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon
{
    public class Parser
    {
        private Lexer _lexer;
        private Token _look;
        public Env Top;
        public int Used;

        public Parser(Lexer lex)
        {
            this._lexer = lex;
            this.Move();
            this.Top = null;
            this.Used = 0;
        }

        public void Move ()
        {
            _look = _lexer.scan();
        }

        public void Error(string msg)
        {
            //note The book here is lex.line, but Line is static member, so this might be a bug.
            throw new Exception("near line " + Lexer.Line + ": " + msg);
        }

        public void Match(int tag)
        {
            if (_look.TagValue == tag) this.Move();
            else this.Error("syntax error");
        }
        
        //..

        public Dragon.Type Dimension(Dragon.Type type)
        {
            this.Match('[');
            Token tok = _look;
            this.Match(Tag.NUM);
            this.Match(']');

            if (_look.TagValue == '[')
                type = this.Dimension(type);

            return new Array(((Num)tok).Value, type);
        }
    }
}
