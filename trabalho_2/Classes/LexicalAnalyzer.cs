using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Classes
{
    public class LexicalAnalyzer
    {
        public List<Token> List;
        public bool Error;
        private int index;
        private string Input;

        public LexicalAnalyzer(string input)
        {
            index = 0;
            Error = false;
            List = new List<Token>();
            Input = Utils.RemoveSpaces(input);
            PreSelect();
        }

        public void PreSelect()
        {

            while(index < Input.Length)
            {
                if (Input[index] == 'i')
                {
                    IfAutomaton();
                }
                else if (Input[index] == 't')
                {
                    ThenAutomaton();
                }
                else if (Input[index] == 'a')
                {
                    Token tk = new Token
                    {
                        Value = "a"

                    };
                    List.Add(tk);

                    index += 1;
                }
                else if (Input[index] == 'b')
                {
                    Token tk = new Token
                    {
                        Value = "b"

                    };
                    List.Add(tk);

                    index += 1;
                }
                else
                {
                    Console.WriteLine("Erro léxico! Símbolo fora da gramática.");
                    Error = true;
                    break;
                }
            }

        }

        private void IfAutomaton()
        {
            if (Input[index] == 'i' && Input[index+1] == 'f')
            {
                Token tk = new Token
                {
                    Value = "if"

                };
                List.Add(tk);

                index += 2;
            }
        }

        private void ThenAutomaton()
        {
            if (Input[index]   == 't' &&
                Input[index+1] == 'h' &&
                Input[index+2] == 'e' &&
                Input[index+3] == 'n')
            {
                Token tk = new Token
                {
                    Value = "then"

                };
                List.Add(tk);

                index += 4;
            }
        }

    }
}
