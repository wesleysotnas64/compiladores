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
        // Testes Léxico
        // string test = "idade:=(19<limite#)&#(limite>15)#";
        // string test = "resultado:=((idade-2)+(2*(idade-3)))*((idade-10)/2.5)";
        // string test = "id:=12+(1.9*4-5)/3(v1<17.1|id<>1)";

        //Testes Sintaxe
        // string test = "id:=id";
        // string test = "id:=id<>id";
        // string test = "id:=id(+id(*id))";
        // string test = "id:=(id)";
        // string test = "id:=(id<=id)";
        // string test = "id:=19<=(20)";
        string test = "id:=(2023(-1998))|25";
        // string test = "id:=((11(-1))|(12))";

        string input = test;

        LexicalIndicator li = LexicalAnalyzer.PreSelect(input);
        Utils.PrintTokens();
        Utils.PrintHighlighted();
        SyntaxParser.Parser(Global.tokens);
    }
}

public class SyntaxParser
{

    public static void Parser(List<Token> list)
    {
        SyntaxIndicator si = new SyntaxIndicator
        {
            next = 0,
            error = false
        };

        si = AssignmentStatement(list, si.next);

        if(si.error == false)
        {
            Console.WriteLine("Aceita!");
        }
        else
        {
            Console.WriteLine("Não Aceita!");
        }
    }

    public static SyntaxIndicator AssignmentStatement(List<Token> list, int init)
    {
        SyntaxIndicator si = new SyntaxIndicator
        {
            next = init,
            error = false
        };

        if(NextToken(list, si.next) != TokenType.Identifier)
        {
            si.error = true;
            return si;
        }
        si.next++;

        if(NextToken(list, si.next) != TokenType.Assignment)
        {
            si.error = true;
            return si;
        }
        si.next++;

        si = Expression(list, si.next);
        if(si.error)
        {
            return si;
        }

        return si;
    }

    public static SyntaxIndicator Expression(List<Token> list, int init)
    {
        SyntaxIndicator si = new SyntaxIndicator
        {
            next = init,
            error = false
        };

        si = SimpleExpression(list, si.next);
        if(si.error)
        {
            return si;
        }

        if(NextToken(list, si.next) == TokenType.RelationalOperator)
        {
            if(NextToken(list, si.next) != TokenType.RelationalOperator)
            {
                si.error = true;
                return si;
            }
            si.next++;

            si = SimpleExpression(list, si.next);
            if(si.error)
            {
                return si;
            }
        }

        return si;
    }

    public static SyntaxIndicator SimpleExpression(List<Token> list, int init)
    {
        SyntaxIndicator si = new SyntaxIndicator
        {
            next = init,
            error = false
        };

        si = Term(list, si.next);
        if(si.error)
        {
            return si;
        }

        bool reading = true;
        while(reading)
        {
            if(si.next < list.Count)
            {
                if(NextToken(list, si.next) == TokenType.LParenthesis &&
                   NextToken(list, si.next+1) == TokenType.AddingOperator)
                {
                    if(NextToken(list, si.next) != TokenType.LParenthesis)
                    {
                        si.error = true;
                        return si;
                    }
                    si.next++;

                    if(NextToken(list, si.next) != TokenType.AddingOperator)
                    {
                        si.error = true;
                        return si;
                    }
                    si.next++;

                    si = Term(list, si.next);
                    if(si.error)
                    {
                        return si;
                    }

                    if(NextToken(list, si.next) != TokenType.RParenthesis)
                    {
                        si.error = true;
                        return si;
                    }
                    si.next++;

                    if(NextToken(list, si.next) != TokenType.LParenthesis &&
                       NextToken(list, si.next+1) != TokenType.AddingOperator)
                    {
                        reading = false;
                    }
                }
                else
                {
                    reading = false;
                }
            }
            else 
            {
                reading = false;
            }
        }

        return si;
    }

    public static SyntaxIndicator Term(List<Token> list, int init)
    {
        SyntaxIndicator si = new SyntaxIndicator
        {
            next = init,
            error = false
        };

        si = Factor(list, si.next);
        if(si.error)
        {
            return si;
        }

        bool reading = true;
        while(reading)
        {
            if(si.next < list.Count)
            {
                if(NextToken(list, si.next) == TokenType.LParenthesis &&
                   NextToken(list, si.next+1) == TokenType.MultiplyingOperator )
                {
                    if(NextToken(list, si.next) != TokenType.LParenthesis)
                    {
                        si.error = true;
                        return si;
                    }
                    si.next++;

                    if(NextToken(list, si.next) != TokenType.MultiplyingOperator)
                    {
                        si.error = true;
                        return si;
                    }
                    si.next++;

                    si = Factor(list, si.next);
                    if(si.error)
                    {
                        return si;
                    }

                    if(NextToken(list, si.next) != TokenType.RParenthesis)
                    {
                        si.error = true;
                        return si;
                    }
                    si.next++;

                    if(NextToken(list, si.next) != TokenType.LParenthesis &&
                       NextToken(list, si.next+1) != TokenType.MultiplyingOperator)
                    {
                        reading = false;
                    }
                }
                else
                {
                    reading = false;
                }
            }
            else
            {
                reading = false;
            }
        }

        return si;
    }

    public static SyntaxIndicator Factor(List<Token> list, int init)
    {
        SyntaxIndicator si = new SyntaxIndicator
        {
            next = init,
            error = false
        };

        if(si.next >= list.Count)
        {
            si.error = true;
            return si;
        }

        if(NextToken(list, si.next) == TokenType.Identifier)
        {
            if(NextToken(list, si.next) != TokenType.Identifier)
            {
                si.error = true;
                return si;
            }
            si.next += 1;
        }
        else if(NextToken(list, si.next) == TokenType.LParenthesis)
        {
            if(NextToken(list, si.next) != TokenType.LParenthesis)
            {
                si.error = true;
                return si;
            }
            si.next += 1;

            si = Expression(list, si.next);
            if(si.error)
            {
                return si;
            }

            if(NextToken(list, si.next) != TokenType.RParenthesis)
            {
                si.error = true;
                return si;
            }
            si.next += 1;
        }
        else if(NextToken(list, si.next) == TokenType.Int ||
                NextToken(list, si.next) == TokenType.Float)
        {
            if(NextToken(list, si.next) != TokenType.Int &&
               NextToken(list, si.next) != TokenType.Float)
            {
                si.error = true;
                return si;
            }
            si.next += 1;
        }
        else
        {
            si.error = true;
            return si;
        }

        return si;
    }

    public static TokenType NextToken(List<Token> list, int index)
    {
        if(index >= list.Count)
            return TokenType.Null;

        return list[index].type;
    }

    public static void PresentSyntaxError(List<Token> list, SyntaxIndicator si, string message = "")
    {
        Console.WriteLine(
            "ERRO SINTÁTICO! Token [{0}] posição [{1}]. \n{2}",
            list[si.next].value, list[si.next].initial, message
        );
        foreach(Token tk in list)
        {
            Console.Write(tk.value);
        }
        Console.WriteLine("");

        for(int j = 0; j < list[si.next].initial+1; j++)
        {
            Console.Write(" ");
        }
            Console.WriteLine("^");
    }
}

public class SyntaxIndicator
{
    public int next;
    public bool error;
}

public class LexicalIndicator
{
    public int prox;
    public bool error;
}

public static class LexicalAnalyzer
{
    public static LexicalIndicator PreSelect(string input)
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

        return li;
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

public static class Global
{
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
            final = f-1
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
            Console.WriteLine("Inic: {0} | Final: {1}", tk.initial, tk.final);
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