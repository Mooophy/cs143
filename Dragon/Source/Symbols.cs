using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    public class Type : Word
    {
        public int Width;
        public Type(string s, int t, int w) : base(s, t) { this.Width = w; }
        public readonly static Type
            Int     =   new Type("int",     Tag.BASIC, 4),
            Float   =   new Type("float",   Tag.BASIC, 8),
            Char    =   new Type("char",    Tag.BASIC, 1),
            Bool    =   new Type("bool",    Tag.BASIC, 1);   
    }
}
