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

        Node() 
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
}
