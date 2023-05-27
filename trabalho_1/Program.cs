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
                           /

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
        string test_01 = "idade:=(19<limite#)&#(limite>15)#";
        string test_02 = "resultado:=((idade-2)+(2*(idade-3)))*((idade-10)/2.5)";
        string test_03 = "id:=12+(1.9*4-5)/3(v1<17.1|id<>1)";

        string input = test_03;

        LexicalAnalyzer.PreSelect(input);
        Utils.PrintTokens();
        Utils.PrintHighlighted();
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

public class LexicalIndicator
{
    public int prox;
    public bool error;
}

public static class LexicalAnalyzer
{
    public static void PreSelect(string input)
    {
        LexicalIndicator li = new LexicalIndicator
        {
            prox = 0,
            error = false
        };

        while((li.prox < input.Length) && (li.error == false))
        {
            if(Utils.CharIsIn(Global.letters, input[li.prox]))
            {
                li = IdentifierAutomaton(input, li.prox);
            }
            else if(Utils.CharIsIn(Global.digits, input[li.prox]))
            {
                li = NumbersAutomaton(input, li.prox);
            }
            else if(Utils.CharIsIn(Global.relational, input[li.prox]))
            {
                li = RelationalAutomaton(input, li.prox);
            }
            else if(Utils.CharIsIn(Global.parenthesis, input[li.prox]))
            {
                li = ParenthesisAutomaton(input, li.prox);
            }
            else if(Utils.CharIsIn(Global.addingOperator, input[li.prox]) ||
                    Utils.CharIsIn(Global.multiplyingOperator, input[li.prox])
                    )
            {
                li = OperatorAutomaton(input, li.prox);
            }
            else if(input[li.prox] == ':')
            {
                li = AssignmentAutomaton(input, li.prox);
            }
            else
            {
                PresentLexicalError(input, li, "Símbolo não pertence a gramática.");
                li.prox++;
                // break;
            }

            if(li.error)
            {
                PresentLexicalError(input, li);
                break;
            }
        }
    }

    public static void PresentLexicalError(string input, LexicalIndicator li, string message = "")
    {
        Console.WriteLine("ERRO LÉXICO! Lexema [{0}] posição [{1}]. \n{2}", input[li.prox], li.prox, message);
        Console.WriteLine(input);
        for(int j = 0; j < li.prox+1; j++)
        {
            if(j != li.prox)
                Console.Write(" ");
            else
                Console.WriteLine("^");
        }
    }

    public static LexicalIndicator IdentifierAutomaton(string input, int i)
    {
        LexicalIndicator li = new LexicalIndicator();

        int initial = i;
        int q = 0;
        bool end = false;
        do
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
                    Utils.AddToken(TokenType.Identifier, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

            }
        } while(end == false);
        return li;
    }

    public static LexicalIndicator NumbersAutomaton(string input, int i)
    {
        LexicalIndicator li = new LexicalIndicator();

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
                Utils.AddToken(TokenType.Int, initial, i, input);
                li.prox = i;
                li.error = false;
                end = true;
            }
            else if(q == 6)
            {
                Utils.AddToken(TokenType.Float, initial, i, input);
                li.prox = i;
                li.error = false;
                end = true;
            }
        }
        return li;
    }

    public static LexicalIndicator RelationalAutomaton(string input, int i)
    {
        LexicalIndicator li = new LexicalIndicator();

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
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                case 3:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                case 4:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                case 5:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                case 7:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                case 8:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;
                
                case 9:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                case 10:
                    Utils.AddToken(TokenType.RelationalOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;

                default:
                    break;
            }
        }

        return li;
    }

    public static LexicalIndicator AssignmentAutomaton(string input, int i)
    {
        LexicalIndicator li = new LexicalIndicator();

        int initial = i;
        int q = 0;
        bool end = false;

        do
        {
            switch(q)
            {
                case 0:
                    if(input[i] == ':')
                    {
                        i++;
                        q = 1;
                    }
                    break;
                
                case 1:
                    if(input[i] == '=')
                    {
                        i++;
                        q = 2;
                    }
                    else
                    {
                        q = 3;
                    }
                    break;
            }

            if(i >= input.Length)
            {
                switch(q)
                {
                    case 1:
                        q = 3;
                        break;
                }
            }


            switch(q)
            {
                case 2:
                    Utils.AddToken(TokenType.Assignment, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;
                
                case 3:
                    li.prox = i;
                    li.error = true;
                    end = true;
                    break;
            }

        } while((end == false) && (li.error == false));
        return li;
    }

    public static LexicalIndicator ParenthesisAutomaton(string input, int i)
    {
        LexicalIndicator li = new LexicalIndicator();

        int initial = i;
        int q = 0;
        bool end = false;

        do
        {
            switch(q)
            {
                case 0:
                    if(input[i] == '(')
                    {
                        i++;
                        q = 1;
                    }
                    else if(input[i] == ')')
                    {
                        i++;
                        q = 2;
                    }
                    break;
                
            }

            switch(q)
            {
                case 1:
                    Utils.AddToken(TokenType.LParenthesis, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;
                
                case 2:
                    Utils.AddToken(TokenType.RParenthesis, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;
            }

        } while((end == false) && (li.error == false));
        return li;
    }

    public static LexicalIndicator OperatorAutomaton(string input, int i)
    {
        LexicalIndicator li = new LexicalIndicator();

        int initial = i;
        int q = 0;
        bool end = false;

        do
        {
            switch(q)
            {
                case 0:
                    if(input[i] == '+' || input[i] == '-')
                    {
                        i++;
                        q = 1;
                    }
                    else if(input[i] == '/' || input[i] == '*')
                    {
                        i++;
                        q = 2;
                    }
                    break;
                
            }

            switch(q)
            {
                case 1:
                    Utils.AddToken(TokenType.AddingOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;
                
                case 2:
                    Utils.AddToken(TokenType.MultiplyingOperator, initial, i, input);
                    li.prox = i;
                    li.error = false;
                    end = true;
                    break;
            }

        } while((end == false) && (li.error == false));
        return li;
    }
}

public static class Global{
    public static string digits = "0123456789";
    public static string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string sign = "+-";
    public static string addingOperator = "+-";
    public static string multiplyingOperator = "*/";
    public static string relational = "=<>|&";
    public static string parenthesis = "()";
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
    Float = 7,
    LParenthesis = 8,
    RParenthesis = 9,
    Assignment = 10
}

public class Token
{
    public TokenType type;
    public string value = "";
    public int initial = 0;
    public int final = 0;
}

public static class Utils
{
    public static bool CharIsIn(string str, char chr)
    {
        for(int i = 0; i < str.Length; i++)
            if(chr == str[i]) return true;

        return false;
    }

    public static void AddToken(TokenType tokenType, int i, int f, string input)
    {

        Token tk = new Token
        {
            type = tokenType,
            value = GetSubString(input, i, f),
            initial = i,
            final = f
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

    public static void PrintHighlighted()
    {
        foreach(Token tk in Global.tokens)
        {
            if(tk.type == TokenType.Identifier)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if(
                tk.type == TokenType.AddingOperator ||
                tk.type == TokenType.MultiplyingOperator ||
                tk.type == TokenType.RelationalOperator ||
                tk.type == TokenType.Assignment
            )
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(
                tk.type == TokenType.Int ||
                tk.type == TokenType.Float
            )
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if(
                tk.type == TokenType.LParenthesis ||
                tk.type == TokenType.RParenthesis
            )
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.Write(tk.value);
            Console.ResetColor();
        }
        Console.WriteLine("");
    }
}