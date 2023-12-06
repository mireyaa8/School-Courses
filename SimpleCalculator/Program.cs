﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var values = input.Split(' ');
            var stack = new Stack<string>(values.Reverse());
            while (stack.Count > 1)
            {
                int first = int.Parse(stack.Pop());
                string operator1 = stack.Pop();
                int second = int.Parse(stack.Pop());
                switch (operator1)

                {
                    case "+":
                    stack.Push((first + second).ToString());
                    break;
                    case "-":
                   stack.Push((first - second).ToString());
                    break;
                }
            }

            Console.WriteLine(stack.Pop());
        }
    }
}
