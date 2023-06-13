using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Classes
{
    public static class Utils
    {
        public static string RemoveSpaces(string input)
        {
            string str = "";

            foreach(char c in input)
            {
                if (c == ' ')
                {
                    str += "";
                }
                else
                {
                    str += c;
                }
            }
            return str;
        }
    
        public static void PrintHighlighted(List<Token> tokens)
        {
            foreach (Token token in tokens)
            {
                if(token.Value == "if" || token.Value == "then")
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write(token.Value + " ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(" ");
        }
    }
}
