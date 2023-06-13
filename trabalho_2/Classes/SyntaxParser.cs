using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Classes
{

    public class SyntaxParser
    {
        public bool Error;

        public List<int> pilha;
        public List<Token> simbolo;
        public List<Token> entrada;

        public string[,] tabelaSLR;

        public SyntaxParser(List<Token> tokens)
        {
            pilha = new List<int>();
            simbolo = new List<Token>();
            entrada = new List<Token>();
            Error = false;

            pilha.Add(0);
            entrada = tokens;
            Token tk = new Token
            {
                Value = "$"
            };
            entrada.Add(tk);

            tabelaSLR = new string[9, 8]
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

            Parser();
        }

        private void Parser()
        {
            Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
            Console.WriteLine("| PILHA              | SÍMBOLO            | ENTRADA            | AÇÃO               |");
            Console.WriteLine("-------------------------------------------------------------------------------------");

            string c;
            bool loop = true;
            while(loop)
            {
                c = tabelaSLR[pilha.Last(), IndexOf(entrada.First())];

                switch (c)
                {
                    case "s2":
                        Shift(2);
                        break;

                    case "s4":
                        Shift(4);
                        break;

                    case "s6":
                        Shift(6);
                        break;

                    case "s7":
                        Shift(7);
                        break;

                    case "r1":
                        Reduce(1);
                        break;

                    case "r2":
                        Reduce(2);
                        break;

                    case "r3":
                        Reduce(3);
                        break;

                    case "r4":
                        Reduce(4);
                        break;

                    case "ok":
                        PrintTable("OK");
                        Console.WriteLine("Aceita!");
                        loop = false;
                        break;

                    default:
                        PrintTable("NULL");
                        Console.WriteLine("Não aceita!");
                        loop = false;
                        Error = true;
                        break;
                }
            }
        }

        private void Shift(int s)
        {
            PrintTable("s" + s);
            pilha.Add(s);
            simbolo.Add(entrada.First());
            entrada.RemoveAt(0);
        }

        private void Reduce(int r)
        {
            PrintTable("r" + r);

            if(r == 1)
            {
                // Substitui SIMBOLO por pela redução
                simbolo.RemoveAt(simbolo.Count - 1);
                simbolo.RemoveAt(simbolo.Count - 1);
                simbolo.RemoveAt(simbolo.Count - 1);
                simbolo.RemoveAt(simbolo.Count - 1);

                Token tk = new Token
                {
                    Value = "S"

                };

                simbolo.Add(tk);

                // Remove o estado da pilha
                pilha.RemoveAt(pilha.Count - 1);
                pilha.RemoveAt(pilha.Count - 1);
                pilha.RemoveAt(pilha.Count - 1);
                pilha.RemoveAt(pilha.Count - 1);

                // O estado atual, ao ler o Simbolo da redução, leva a qual estado?
                string e = tabelaSLR[pilha.Last(), IndexOf(simbolo.Last())];

                // Adiciona o novo estado na pilha
                pilha.Add(int.Parse(e));

            }
            else if(r == 2)
            {
                // Substitui SIMBOLO por pela redução
                simbolo.RemoveAt(simbolo.Count - 1);

                Token tk = new Token
                {
                    Value = "S"

                };

                simbolo.Add(tk);

                // Remove o estado da pilha
                pilha.RemoveAt(pilha.Count - 1);

                // O estado atual, ao ler o Simbolo da redução, leva a qual estado?
                string e = tabelaSLR[pilha.Last(), IndexOf(simbolo.Last())];

                // Adiciona o novo estado na pilha
                pilha.Add(int.Parse(e));
            }
            else if (r == 3)
            {
                // Substitui SIMBOLO por pela redução
                simbolo.RemoveAt(simbolo.Count - 1);

                Token tk = new Token
                {
                    Value = "E"

                };

                simbolo.Add(tk);

                // Remove o estado da pilha
                pilha.RemoveAt(pilha.Count - 1);

                // O estado atual, ao ler o Simbolo da redução, leva a qual estado?
                string e = tabelaSLR[pilha.Last(), IndexOf(simbolo.Last())];

                // Adiciona o novo estado na pilha
                pilha.Add(int.Parse(e));
            }
            else if (r == 4)
            {
                // Substitui SIMBOLO por pela redução
                simbolo.RemoveAt(simbolo.Count - 1);

                Token tk = new Token
                {
                    Value = "C"

                };

                simbolo.Add(tk);

                // Remove o estado da pilha
                pilha.RemoveAt(pilha.Count - 1);

                // O estado atual, ao ler o Simbolo da redução, leva a qual estado?
                string e = tabelaSLR[pilha.Last(), IndexOf(simbolo.Last())];

                // Adiciona o novo estado na pilha
                pilha.Add(int.Parse(e));
            }
        }

        private int IndexOf(Token tk)
        {
            string v = tk.Value;
            switch(v)
            {
                case "if":
                    return 0;
                case "then":
                    return 1;
                case "a":
                    return 2;
                case "b":
                    return 3;
                case "$":
                    return 4;
                case "S":
                    return 5;
                case "E":
                    return 6;
                default:
                    return 7;
            }
        }

        private void PrintTable(string acao)
        {
            int spaceCount;

            //Imprime pilha
            spaceCount = 0;
            Console.Write("| ");
            foreach (int l in pilha)
            {
                Console.Write(l + " ");
                spaceCount += 2;
            }
            for (int i = spaceCount; i < 19; i++)
                Console.Write(" ");

            //Imprime Símbolo
            spaceCount = 0;
            Console.Write("| ");
            foreach (Token tk in simbolo)
            {
                Console.Write(tk.Value + " ");
                spaceCount += tk.Value.Length + 1;
            }
            for (int i = spaceCount; i < 19; i++)
                Console.Write(" ");

            //Imprime Entrada
            spaceCount = 0;
            Console.Write("| ");
            foreach (Token tk in entrada)
            {
                Console.Write(tk.Value + " ");
                spaceCount += tk.Value.Length + 1;
            }
            for (int i = spaceCount; i < 19; i++)
                Console.Write(" ");

            //Imprime Ação
            spaceCount = 0;
            Console.Write("| ");
            Console.Write(acao);
            spaceCount += acao.Length + 1;
            for (int i = spaceCount; i < 20; i++)
                Console.Write(" ");
            Console.WriteLine("|");
            Console.WriteLine("-------------------------------------------------------------------------------------");


        }
    }
}
