using System;
using System.Collections.Generic;

/*
S -> cAd
A -> abA | c
*/

public class Program
{
    private static void Main(string[] args)
    {
        
        string input = "cababcd";
        Console.WriteLine(S(input));
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