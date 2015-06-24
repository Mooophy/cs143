using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon
{
    public class Parser
    {
        #region Fields
        private Lexer _lexer;
        private Token _look;
        public Env Top;
        public int Used;
        #endregion

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
        public void Declaration()
        {
            while(_look.TagValue == Tag.BASIC)
            {
                var type = this.Type();
                var tok = _look;
                this.Match(Tag.ID);
                this.Match(';');

                var id = new Id(tok as Word, type, this.Used);
                this.Top.Add(tok, id);
                this.Used += type.Width;
            }
        }

        public Dragon.Type Type()
        {
            var type = _look as Dragon.Type;    //expect _look.tag == Tag.Basic
            this.Match(Tag.BASIC);

            return _look.TagValue != '[' ? type : this.Dimension(type);
        }

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

        //Stmts()

        //Stmt()

        //public Stmt Assign()
        //{
        //    Stmt stmt;
        //    var tok = _look;

        //    this.Match(Tag.ID);
        //    var id = Top.Get(tok);
        //    if (id == null)
        //        this.Error(tok.ToString() + " undeclared");

        //    if(_look.TagValue == '=')
        //    {
        //        this.Move();
        //        stmt = new Set(id,)
        //    }
        //}     need method Bool() to continue

        public Expr Bool()
        {
            Expr expr = this.Join();
            while(_look.TagValue == Tag.OR)
            {
                var tok = _look;
                this.Move();
                expr = new Or(tok, expr, this.Join());
            }
            return expr;
        }

        public Expr Join()
        {
            Expr expr = this.Equality();
            while(_look.TagValue == Tag.AND)
            {
                var tok = _look;
                this.Move();
                expr = new And(tok, expr, this.Equality());
            }
            return expr;
        }

        public Expr Equality()
        {
            Expr expr = this.Rel();
            while(_look.TagValue == Tag.EQ || _look.TagValue == Tag.NE)
            {
                var tok = _look;
                this.Move();
                expr = new Rel(tok, expr, this.Rel());
            }
            return expr;
        }

        public Expr Rel()
        {
            Expr expr = this.Expr();
            if('<' == _look.TagValue || Tag.LE == _look.TagValue || Tag.GE == _look.TagValue || '>' == _look.TagValue)
            {
                Token tok = _look;
                this.Move();
                return new Rel(tok, expr, this.Expr());
            }
            return expr;
        }

        public Expr Expr()
        {
            Expr expr = this.Term();
            while(_look.TagValue == '+' || _look.TagValue == '-')
            {
                Token tok = _look;
                this.Move();
                expr = new Arith(tok, expr, this.Term());
            }
            return expr;
        }
    }
}
