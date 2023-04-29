using System;
using System.Collections.Generic;

public class Program
{
    private static void Main(string[] args)
    {
        string input = "aababababbabaaabaabbabbbabbababbbaababa";
        string template = "ana";
        
        Table t = new Table(template);
        t.PrintMatrix(template);
        // KMP kmp = new KMP();
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
        // PrintMatrix(template);
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
