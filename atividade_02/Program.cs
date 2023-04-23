using System;
using System.Collections.Generic;

/*
    O seguinte autômato identifica os padrões de números inteiros e reais.
    A imagem que representa o autômato está na pasta img/Automato_INT_FLOAT.png
*/


public static class Global{
    public static string digits = "0123456789";
    public static string operators = "+-";
    public static char dot = '.';
    public static List<Token> intTokens = new List<Token>();
    public static List<Token> floatTokens = new List<Token>();
    public static List<Token> tokens = new List<Token>();
}

public enum TokenType
{
    NULL = 0,
    INT = 1,
    FLOAT = 2
}

public class Token
{
    public TokenType type;
    public int initial;
    public int final;
}


public class Program
{
    private static void Main(string[] args)
    {
        string input = "10==27string100.456float+99++3.a-2.27.2..0.+1string0.5";
        IdentfyNumbers(input);
        Console.WriteLine(input);
        Utils.PrintHighlighted(input);
        // Utils.PrintTokens(input);
    }

    private static void IdentfyNumbers(string input)
    {
        int state = 0;
        int initial = 0;
        int final = 0;

        int i = 0;
        int length = input.Length;
        char chr;
        while(i < length)
        {
            chr = input[i];
            //transition
            switch(state)
            {
                case 0:
                    initial = i;
                    if(Utils.InOperators(chr))
                    {
                        state = 1;
                    }
                    else if(Utils.InDigits(chr))
                    {
                        state = 2;
                    }
                    break;
                
                case 1:
                    if(Utils.InDigits(chr))
                    {
                        state = 2;
                    }
                    else
                    {
                        state = 0;
                        i--;
                    }
                    break;
                
                case 2:
                    if(Utils.IsDot(chr))
                    {
                        state = 4;
                    }
                    else if(!Utils.InDigits(chr))
                    {
                        state = 3;
                        i--;
                    }
                    break;
                
                case 4:
                    if(Utils.InDigits(chr))
                    {
                        state = 5;
                    }
                    else
                    {
                        i -= 2;
                        state = 3;
                    }
                    break;

                case 5:
                    if(!Utils.InDigits(chr))
                    {
                        state = 6;
                        i--;
                    }
                    break;

                default:
                    break;
                
            }

            if(i+1 == length)
            {
                switch(state)
                {
                    case 2:
                        state = 3;
                        break;
                    
                    case 4:
                        state = 3;
                        i--;
                        break;

                    case 5:
                        state = 6;
                        break;
                }
            }

            switch(state)
            {
                case 3:
                    final = i;
                    Utils.AddToken(TokenType.INT, initial, final);
                    state = 0;
                    break;
                
                case 6:
                    final = i;
                    Utils.AddToken(TokenType.FLOAT, initial, final);
                    state = 0;
                    break;
            }

            i++;
        }
    }

}

public static class Utils
{
    public static bool InDigits(char chr)
    {
        foreach(char digit in Global.digits)
        {
            if(digit == chr)
                return true;
        }

        return false;
    }

    public static bool InOperators(char chr)
    {
        foreach(char op in Global.operators)
        {
            if(op == chr)
                return true;
        }

        return false;
    }

    public static bool IsDot(char chr)
    {
        if(chr == Global.dot)
            return true;
        
        return false;
    }

    public static void AddToken(TokenType tokenType, int initial, int final)
    {

        Token tk = new Token
        {
            type = tokenType,
            initial = initial,
            final = final
        };

        Global.tokens.Add(tk);
    }

    public static void PrintTokens(string input)
    {
        foreach(Token tk in Global.tokens)
        {
            for(int i = tk.initial; i <= tk.final; i++)
            {
                Console.Write(input[i]);
            }
            Console.WriteLine("");
        }
    }

    public static void PrintHighlighted(string input)
    {
        TokenType tkType;
        for(int i = 0; i < input.Length; i++)
        {
            tkType = IsTokenInterval(i, input);
            switch (tkType)
            {
                case TokenType.INT:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case TokenType.FLOAT:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.Write(input[i]);
            Console.ResetColor();
        }
        Console.WriteLine("");
    }

    public static TokenType IsTokenInterval(int i, string input)
    {
        foreach(Token tk in Global.tokens)
        {
            if(i >= tk.initial && i <= tk.final)
            {
                return tk.type;
            }
        }

        return TokenType.NULL;
    }
}