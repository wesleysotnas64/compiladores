using Compiladores.Classes;
using System;
using System.Diagnostics;

namespace Compiladores 
{

    public static class Global
    {
        public static List<int>   pilha   = new List<int>();
        public static List<Token> simbolo = new List<Token>();
        public static List<Token> entrada = new List<Token>();
        public static List<Token> tokens  = new List<Token>();

        public static string[,] tabelaSLR = new string[9, 8]
        {
            { "s2", ""  , ""  , "s4", ""  ,    "1", "" , "3" },
            { ""  , ""  , ""  , ""  , "ok",    "" , "" , ""  },
            { ""  , ""  , "s6", ""  , ""  ,    "" , "5", ""  },
            { ""  , ""  , ""  , ""  , "r2",    "" , "" , ""  },
            { ""  , ""  , ""  , ""  , "r4",    "" , "" , ""  },
            { ""  , "s7", ""  , ""  , ""  ,    "" , "" , ""  },
            { ""  , "r3", ""  , ""  , ""  ,    "" , "" , ""  },
            { ""  , ""  , ""  , "s4", ""  ,    "" , "" , "8" },
            { ""  , ""  , ""  , ""  , "r1",    "" , "" , ""  }
        };
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            //input = "b";
            //input = "if a then b";

            //input = "a";
            //input = "if b";
            //input = "if a then #";
            //input = "if a then a";
            //input = "if if a then b";
            LexicalAnalyzer la = new LexicalAnalyzer(input);
            if(la.Error == false)
            {
                Utils.PrintHighlighted(la.List);
                SyntaxParser sp = new SyntaxParser(la.List);
            }
        }
    }
}