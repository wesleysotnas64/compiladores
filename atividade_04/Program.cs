using System;
using System.Collections.Generic;

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
        string input = "aaaanannnnananaaanaana";
        string template = "ana";
        
        Table t = new Table(template);
        t.PrintMatrix(template);
        IdentifiesString(input, template, t);
        Console.WriteLine(input);
        Utils.PrintHighlighted(input);
    }

    private static void IdentifiesString(string input, string template, Table t)
    {
        int state = 0;
        for(int i = 0; i < input.Length; i++)
        {
            state = t.Transition[t.IndexAlphabet(input[i]),state];

            if(state == template.Length)
            {
                // Console.WriteLine(i-(state-1));
                Utils.AddToken(i-(state-1), i);
                state = 0;
            }
        }
    }
}

public class Table
{
    public List<char> Alphabet;
    public int[,] Transition;

    public Table(string template)
    {
        Alphabet = new List<char>();
        DetectAlphabet(template);
        Transition = new int[Alphabet.Count, template.Length];
        Transitions(template);
    }

    private void DetectAlphabet(string template)
    {
        foreach(char chr in template)
        {
            if(Alphabet.IndexOf(chr) == -1)
            {   
                Alphabet.Add(chr);
            }
        }
    }

    private void Transitions(string template)
    {
        int alfaIndex;

        alfaIndex = Alphabet.IndexOf(template[0]);
        if(alfaIndex != -1)
        {
            Transition[alfaIndex,0] = 1;
        }

        int returnState = 0;
        for(int state = 1; state < template.Length; state++)
        {
            for(int alfa = 0; alfa < Alphabet.Count; alfa++)
            {
                Transition[alfa,state] = Transition[alfa,returnState];
            }
            
            alfaIndex = Alphabet.IndexOf(template[state]);
            Transition[alfaIndex, state] = state+1;
            returnState = Transition[alfaIndex, returnState];
        }
    }

    public int IndexAlphabet(char chr)
    {
        return Alphabet.IndexOf(chr);
    }

    public void PrintMatrix(string template)
    {
        int l = Alphabet.Count;
        int c = template.Length;

        Console.Write("  ");
        for(int i = 0; i < c; i++)
            Console.Write(" "+i+" ");
        Console.WriteLine("");

        Console.Write("  ");
        foreach(char chr in template)
        {
            Console.Write(" "+chr+" ");
        }
        Console.WriteLine("");
        

        for(int i = 0; i < l; i++)
        {
            Console.Write(Alphabet[i]+" ");
            for(int j = 0; j < c; j++)
            {
                Console.Write("["+Transition[i,j]+"]");
            }
            Console.WriteLine("");
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
        PrintInitiTemplate(input);
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

    private static void PrintInitiTemplate(string input)
    {
        for(int i = 0; i < input.Length; i++)
        {
            if(IsInitInterval(i, input))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("^");
                Console.ResetColor();
            }
            else
            {
                Console.Write(" ");
            }
        }
        Console.WriteLine("");
    }

    public static bool IsInitInterval(int i, string input)
    {
        foreach(Token tk in Global.tokens)
        {
            if(i == tk.initial)
            {
                return true;
            }
        }
        return false;
    }
}