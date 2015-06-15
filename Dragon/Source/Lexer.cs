using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Source
{
    public class Tag
    {
        public readonly static int
            AND     =   256,
            BASIC   =   257,
            BREAK   =   258,
            DO      =   259,
            ELSE    =   260,
            EQ      =   261,
            FALSE   =   262,
            GE      =   263,
            ID      =   264,
            IF      =   265,
            INDEX   =   266,
            LE      =   267,
            MINUS   =   268,
            NE      =   269,
            NUM     =   270,
            OR      =   271,
            REAL    =   272,
            TEMP    =   273,
            TRUE    =   274,
            WHILE   =   275;
    }

    public class Token
    {
        public readonly int TagValue;
        public Token(int t) { this.TagValue = t; }
        public override string ToString() { return "" + this.TagValue; }
    }

    public class Num : Token
    {
        public readonly int Value;
        public Num(int v): base(Tag.NUM) { this.Value = v; }
        public override string ToString() { return "" + this.Value; }
    }

    public class Word : Token
    {
        public readonly string Lexeme;
        public Word(string s, int t) : base(t) { this.Lexeme = s; }
        public override string ToString() { return this.Lexeme; }
        public readonly static Word
            and     =   new Word("&&", Tag.AND),
            or      =   new Word("||", Tag.OR),
            eq      =   new Word("==", Tag.EQ),
            ne      =   new Word("!=", Tag.NE),
            le      =   new Word("<=", Tag.LE),
            ge      =   new Word(">=", Tag.GE),
            minus   =   new Word("minus", Tag.MINUS),
            True    =   new Word("true", Tag.TRUE),
            False   =   new Word("false", Tag.FALSE),
            temp    =   new Word("t", Tag.TEMP);
    }

    public class Real : Token
    {
        public readonly float Value;
        public Real(float v) : base(Tag.REAL) { this.Value = v; }
        public override string ToString() { return "" + this.Value; }
    }

    public class Lexer
    {
        //char _peek;
        //HashSet<Word> _words;
        //public long Line { get; private set; }   

    }
}
