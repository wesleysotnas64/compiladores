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
        
        string input = "id10";
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
    public static string multiplyingOperator = "*/";
    public static string relational = "";
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
    RelationalOperator = 5
}

public class Token
{
    public TokenType type;
    public string value;
}

public static class LexicalAnalyzer
{
    public static void PreSelect(string input)
    {
        int i = 0;
        while(i < input.Length)
        {
            if(Utils.CharIsIn(Global.letters, input[i]))
            {
                i = IdentifierAutomaton(input, i);
            }
            // else if()
            // {

            // }
        }
    }

    public static int IdentifierAutomaton(string input, int i)
    {
        int initial = i;
        int q = 0;
        while(true)
        {
            if(q == 0 && Utils.CharIsIn(Global.letters, input[i]))
            {
                q = 1;
                i++;
            }
            else if(q == 1 && (Utils.CharIsIn(Global.letters, input[i]) || Utils.CharIsIn(Global.digits, input[i])))
            {
                i++;
            }
            else{
                q = 2;
            }

            if(i+1 > input.Length)
                q = 2;

            if(q == 2)
            {
                Utils.AddToken(TokenType.Identifier, Utils.GetSubString(input, initial, i));
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
        foreach(Token tk in Global.tokens)
        {
            Console.WriteLine(tk.type);
            Console.WriteLine(tk.value);
            Console.WriteLine("");
        }
    }
}