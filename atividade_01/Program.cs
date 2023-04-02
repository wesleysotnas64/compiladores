using System;
using System.Collections.Generic;

public static class Global
{
    public static string alphabet = "abcdefghijklmnopqrstuvwxyz";
    public static string digits = "123456789";
    public static char zero = '0';
    public static char hyphen = '-';
    public static char point = '.';
    public static List<Token> tokens = new List<Token>();
}

enum State
{
    q0 = 0,
    q1 = 1,
    q2 = 2
}

enum IntType
{
    negative = 0,
    positive = 1,
    notInt = 2
}

public class Token
{
    public bool negative;
    public int init;
    public int final;
}

public class Program
{
    public static void Main()
    {
        string input = "ab-1c10ff54joa65-9--58mjd--6-";
        GetIntegers(input);
    }

    public static void GetIntegers(string input)
    {
        Console.WriteLine("Fita de entrada: "+input);

        State state = State.q0;

        int countInt = 0;

        int initRead = 0;
        int finalRead = 0;

        

        for(int i = 0; i < input.Length; i++)
        {
            //q0 > q0
            if(state == State.q0 && InAlphabet(input[i]))
            {
                initRead = i;
            }

            //q0 > q1
            else if(state == State.q0 && InDigits(input[i]))
            {
                initRead = i;
                state = State.q1;
            }

            //q1 > q1
            else if(state == State.q1 && (InDigits(input[i]) || (input[i] == Global.zero)))
            {
                finalRead = i;

                if (i+1 == input.Length)
                {
                    countInt++;
                    SaveToken(initRead, finalRead, input);
                    PrintInterval(initRead, finalRead, input);
                }
            }

            //q1 > q0
            else if(state == State.q1 && InAlphabet(input[i]))
            {
                finalRead = i-1;
                state = State.q0;
                countInt++;
                SaveToken(initRead, finalRead, input);
                PrintInterval(initRead, finalRead, input);
            }

            //q1 > q2
            else if(state == State.q1 && input[i] == Global.hyphen)
            {
                state = State.q2;
                finalRead = i-1;
                countInt++;
                SaveToken(initRead, finalRead, input);
                PrintInterval(initRead, finalRead, input);
                initRead = i;
            }

            //q0 > q2
            else if(state == State.q0 && input[i] == Global.hyphen)
            {
                state = State.q2;
                initRead = i;
            }

            //q2 > q1
            else if(state == State.q2 && InDigits(input[i]))
            {
                state = State.q1;
                finalRead = i;

                if (i+1 == input.Length)
                {
                    countInt++;
                    SaveToken(initRead, finalRead, input);
                    PrintInterval(initRead, finalRead, input);
                }
            }

            //q2 > q2
            else if(state == State.q2 && input[i] == Global.hyphen)
            {
                initRead = i;
            }
        }

        PrintFooter(countInt, input);
    }

    public static bool InDigits(char chr)
    {
        for(int i = 0; i < Global.digits.Length; i++)
        {
            if(chr == Global.digits[i]) return true;
        }

        return false;
    }

    public static bool InAlphabet(char chr)
    {
        for(int i = 0; i < Global.alphabet.Length; i++)
        {
            if(chr == Global.alphabet[i]) return true;
        }

        return false;
    }

    public static void PrintInterval(int init, int final, string input)
    {
        for(int i = init; i < final+1; i++)
            Console.Write(input[i]);
        
        Console.WriteLine();
    }

    public static void PrintFooter(int countInt, string input)
    {
        for(int i = 0; i < input.Length; i++)
            Console.Write("-");
        Console.WriteLine();

        PrintInputColor(input);

        Console.WriteLine("Int: " + countInt);
    }

    public static void PrintInputColor(string input)
    {
        IntType intType;
        for(int i = 0; i < input.Length; i++)
        {
            IsTokenInterval(i, input);
            
            Console.Write(input[i]);
            Console.ResetColor();
        }
    }

    public static void IsTokenInterval(int i, string input)
    {
        foreach(Token token in Global.tokens)
        {
            if(i >= token.init && i <= token.final)
            {
                if(token.negative)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;
                
                continue;
            }
        }
    }

    public static void SaveToken(int init, int final, string input)
    {
        Token token = new Token();
        token.init  = init;
        token.final = final;
        if(input[init] == Global.hyphen)
            token.negative = true;
        
        Global.tokens.Add(token);
    }
}