﻿namespace ReverseString
{
    internal class Program
    {
        static void Main(string[] args)
        {
          
            string input = Console.ReadLine();
            Stack<char> stack = new Stack<char>();
            foreach(var ch in input)
            {
                stack.Push(ch);
            }
            while(stack.Count != 0)
            {
                Console.WriteLine(stack.Pop());
            };

            Console.WriteLine();
        }

    }
}