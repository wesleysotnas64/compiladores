using System;
using System.Collections.Generic;

/* GRAMÁTICA
<assignment statement> ::= <identifier> := <expression>

          <expression> ::= <simple expression> |
                           <simple expression> <relational operator> <simple expression>

   <simple expression> ::= <sign> <term> ( <adding operator> <term> )*

                <term> ::= <factor> ( <multiplying operator> <factor> )*

              <factor> ::= <identifier> |
                           ( <expression> ) |
                           not <factor> |
                           <digit>+

 <relational operator> ::= = |
                           <> |
                           < |
                           <= |
                           >= |
                           > |
                           or |
                           and

     <adding operator> ::= + |
                           -

<multiplying operator> ::= * |
                           div

                <sign> ::= + |
                           - |
                           <empty>

          <identifier> ::= <letter> ( <letter> | <digit> )*

              <letter> ::= a | b | c | d | e | f | g | h | i | j | k | l | m | n | o |
                           p | q | r | s | t | u | v | w | x | y | z |
                           A | B | C | D | E | F | G | H | I | J | K | L | M | N | O |
                           P | Q | R | S | T | W | V | W | X | Y | Z

               <digit> ::= 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9
*/

public class Program
{
    private static void Main(string[] args)
    {
        
        string input = "999identifier10=<><<=>=>|&";
        LexicalAnalyzer.PreSelect(input);
        Utils.PrintTokens();
    }

    private static bool S(string input)
    {
        if(!Match('c', input[0]))
            return false;

        string subString = GetSubString(input, 1, input.Length-1);

        if(!A(subString))
            return false;

        if(!Match('d', input[input.Length-1]))
            return false;

        return true;
    }

    private static bool A(string input)
    {
        if(NextToken(input, 0) == 'a')
        {
            if(!Match('a', input[0]))
                return false;

            if(!Match('b', input[1]))
                return false;

            string subString = GetSubString(input, 2, input.Length);

            if(!A(subString))          
                return false;
            
            return true;
        }
        else if(NextToken(input, 0) == 'c')
        {
            if(!Match('c', input[0]))
                return false;
            
            return true;
        }
        else{
            return false; //Erro sintático
        }
    }

    private static bool Match(char tk, char current)
    {
        if(tk == current)
            return true;

        return false;
    }

    private static char NextToken(string input, int i)
    {
        return input[i];
    }

    private static string GetSubString(string input, int initial, int final)
    {
        string subString = "";
        for(int i = initial; i < final; i++)
            subString += input[i];

        return subString;
    }
}


public static class Global{
    public static string digits = "0123456789";
    public static string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string addingOperator = "+-";
    public static string sign = "+-";
    public static string multiplyingOperator = "*/";
    public static string relational = "=<>|&";
    public static char dot = '.';
    public static List<Token> tokens = new List<Token>();
}

public enum TokenType
{
    Null = 0,
    Identifier = 1,
    Sign = 2,
    MultiplyingOperator = 3,
    AddingOperator = 4,
    RelationalOperator = 5,
    Int = 6,
    Float = 7
}

public class Token
{
    public TokenType type;
    public string value = "";
}

public static class LexicalAnalyzer
{
    public static void PreSelect(string input)
    {
        int i = 0;
        while(i < input.Length)
        {
            Console.WriteLine("Tamanho: {0} | i: {1}", input.Length, i);

            if(Utils.CharIsIn(Global.letters, input[i]))
            {
                i = IdentifierAutomaton(input, i);
            }
            else if(Utils.CharIsIn(Global.digits, input[i]))
            {
                i = NumbersAutomaton(input, i);
            }
            else if(Utils.CharIsIn(Global.relational, input[i]))
            {
                i = RelationalAutomaton(input, i);
            }
            else
            {
                Console.WriteLine("ERRO LÉXICO! Lexema [{0}] posição[{1}] fora da tabela de símbolos.", input[i], i);
                Console.WriteLine(input);
                for(int j = 0; j < i+1; j++)
                {
                    if(j != i)
                        Console.Write(" ");
                    else
                        Console.WriteLine("^");
                }
                break;
            }

            Console.WriteLine("Tamanho: {0} | i: {1}", input.Length, i);
        }
    }

    public static int IdentifierAutomaton(string input, int i)
    {
        int initial = i;
        int q = 0;
        bool end = false;
        while(!end == true)
        {

            switch(q)
            {
                case 0:
                    if(Utils.CharIsIn(Global.letters, input[i]))
                    {
                        q = 1;
                        i++;
                    }
                    break;

                case 1:
                    if(Utils.CharIsIn(Global.letters, input[i]) || Utils.CharIsIn(Global.digits, input[i]))
                    {
                        i++;
                    }
                    else
                    {
                        q = 2;
                    }
                    break;
            }

            if(i >= input.Length)
            {
                switch(q)
                {
                    case 1:
                        q = 2;
                        break;
                }
            }

            switch (q)
            {
                case 2:
                    Utils.AddToken(TokenType.Identifier, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                default:
                    break;
            }
        }
        return i;
    }

    public static int NumbersAutomaton(string input, int i)
    {
        int q = 0;
        int initial = i;

        char chr;
        bool end = false;
        while(!end == true)
        {
            chr = input[i];
            //transition
            switch(q)
            {
                case 0:
                    if(Utils.CharIsIn(Global.sign, chr))
                    {
                        q = 1;
                        i++;
                    }
                    else if(Utils.CharIsIn(Global.digits, chr))
                    {
                        q = 2;
                        i++;
                    }
                    break;
                
                case 1:
                    if(Utils.CharIsIn(Global.digits, chr))
                    {
                        q = 2;
                        i++;
                    }
                    break;
                
                case 2:
                    if(Global.dot == chr)
                    {
                        q = 4;
                        i++;
                    }
                    else if(Utils.CharIsIn(Global.digits, chr))
                    {
                        i++;
                    }
                    else
                    {
                        q = 3;
                    }
                    break;
                
                case 4:
                    if(Utils.CharIsIn(Global.digits, chr))
                    {
                        q = 5;
                        i++;
                    }
                    else
                    {
                        i -= 1;
                        q = 3;
                    }
                    break;

                case 5:
                    if(Utils.CharIsIn(Global.digits, chr))
                    {
                        q = 5;
                        i++;
                    }
                    else
                    {
                        q = 6;
                    }
                    break;

                default:
                    break;
                
            }

            if(i >= input.Length)
            {
                switch(q)
                {
                    case 2:
                        q = 3;
                        break;
                    
                    case 4:
                        q = 3;
                        i -= 1;
                        break;

                    case 5:
                        q = 6;
                        break;
                }
            }

            if(q == 3)
            {
                Utils.AddToken(TokenType.Int, Utils.GetSubString(input, initial, i));
                end = true;
            }
            else if(q == 6)
            {
                Utils.AddToken(TokenType.Float, Utils.GetSubString(input, initial, i));
                end = true;
            }
        }
        return i;
    }

    public static int RelationalAutomaton(string input, int i)
    {
        int q = 0;
        int initial = i;

        bool end = false;
        while(!end == true)
        {
            switch(q)
            {
                case 0:
                    if(input[i] == '=')
                    {
                        i++;
                        q = 1;
                    }
                    else if(input[i] == '<')
                    {
                        i++;
                        q = 2;
                    }
                    else if(input[i] == '>')
                    {
                        i++;
                        q = 6;
                    }
                    else if(input[i] == '|')
                    {
                        i++;
                        q = 9;
                    }
                    else if(input[i] == '&')
                    {
                        i++;
                        q = 10;
                    }
                    break;
                
                case 2:
                    if(input[i] == '=')
                    {
                        i++;
                        q = 3;
                    }
                    else if(input[i] == '>')
                    {
                        i++;
                        q = 4;
                    }
                    else
                    {
                        q = 5;
                    }
                    break;

                case 6:
                    if(input[i] == '=')
                    {
                        i++;
                        q = 7;
                    }
                    else
                    {
                        q = 8;
                    }
                    break;

                default:
                    break;
            }

            if(i >= input.Length)
            {
                switch(q)
                {
                    case 2:
                        q = 5;
                        break;

                    case 6:
                        q = 8;
                        break;
                    
                    default:
                        break;
                }
            }

            switch(q)
            {
                case 1:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                case 3:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                case 4:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                case 5:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                case 7:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                case 8:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;
                
                case 9:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                case 10:
                    Utils.AddToken(TokenType.RelationalOperator, Utils.GetSubString(input, initial, i));
                    end = true;
                    break;

                default:
                    break;
            }
        }

        return i;
    }
}

public static class Utils
{
    public static bool CharIsIn(string str, char chr)
    {
        for(int i = 0; i < str.Length; i++)
            if(chr == str[i]) return true;

        return false;
    }

    public static void AddToken(TokenType tokenType, string value)
    {

        Token tk = new Token
        {
            type = tokenType,
            value = value
        };

        Global.tokens.Add(tk);
    }

    public static string GetSubString(string input, int initial, int final)
    {
        string subString = "";
        for(int i = initial; i < final; i++)
            subString += input[i];

        return subString;
    }

    public static void PrintTokens()
    {
        Console.WriteLine("--------------------");
        foreach(Token tk in Global.tokens)
        {
            Console.WriteLine("Type: {0} | Value: {1}", tk.type, tk.value);
            Console.WriteLine("--------------------");
        }
    }
}