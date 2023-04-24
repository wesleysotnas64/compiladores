using System;
using System.Collections.Generic;

/*
    Identifica padrões em uma cadeia de caracteres
    Algoritmo O(n*m).
    Para cada índice n da entrada, verifica os m índices do padrão.
*/

public class Token
{
    public int initial;
    public int final;
}

public static class Global{
    public static List<Token> tokens = new List<Token>();
}

public class Program
{
    private static void Main(string[] args)
    {
        string input = "anaaninhaananaana--ana-anaaa-anannana";
        string template = "ana";
        IdentifiesString(input, template);
        Console.WriteLine(input);
        Utils.PrintHighlighted(input);
    }

    public static void IdentifiesString(string input, string template)
    {
        int k;
        int j;
        for(int i = 0; i < input.Length-(template.Length-1); i++)
        {
            k = i;
            j = 0;
            while(true)
            {
                if(input[k] == template[j])
                {
                    k++;
                    j++;
                }
                else
                    break;
                
                if(j == template.Length)
                {
                    Utils.AddToken(i, k-1);
                    i+=template.Length;
                    break;
                }
            }
        }
    }
}

public static class Utils
{
    public static void AddToken(int initial, int final)
    {

        Token tk = new Token
        {
            initial = initial,
            final = final
        };

        Global.tokens.Add(tk);
        // Console.WriteLine("Token salvo!");
    }

    public static void PrintHighlighted(string input)
    {
        for(int i = 0; i < input.Length; i++)
        {
            if(IsTokenInterval(i, input))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.DarkGray;
            
            Console.Write(input[i]);
            Console.ResetColor();
        }
        Console.WriteLine("");
    }

    public static bool IsTokenInterval(int i, string input)
    {
        foreach(Token tk in Global.tokens)
        {
            if(i >= tk.initial && i <= tk.final)
            {
                return true;
            }
        }

        return false;
    }
}