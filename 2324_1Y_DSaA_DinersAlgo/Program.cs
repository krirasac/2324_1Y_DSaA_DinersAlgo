using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace _2324_1Y_DSaA_DinersAlgo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            byte[] diners = new byte[] { 0, 0, 0, 0, 0 };
            // Diner status 0 = waiting, 1 = eating/processing, 2 = resting/output
            bool[] forks = new bool[] { false, false, false, false, false };
            // Resource or forks status false = available, true = being used
            byte[] timers = new byte[] { 0, 0, 0, 0, 0 };
            // Just keeps track of the timers. By default it will be 0

            bool activation = false;
            int cycleCount = 0, count = 0;
            ConsoleColor[] statusColors = new ConsoleColor[]
            {ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Red};

            using (StreamReader cycle = new StreamReader("cycle.txt"))
            { 
                string line = cycle.ReadLine();
                count = int.Parse(line);
            }

            using (StreamWriter info = new StreamWriter("situation.txt"))
            {
                while (cycleCount < count)
                {
                    cycleCount++;
                    Console.ResetColor();
                    Console.WriteLine($"Cycle {cycleCount}...");
                    for (int x = 0; x < diners.Length; x++)
                    {
                        if (diners[x] == 0) // waiting
                        {
                            activation = false;
                            // check if the resources are available
                            if (!forks[x])
                            {
                                if (x - 1 < 0)
                                {
                                    // this will only trigger for diner 0
                                    if (!forks[forks.Length - 1])
                                    {
                                        diners[x] = 1;
                                        forks[x] = true;
                                        forks[forks.Length - 1] = true;
                                        timers[x] = (byte)rnd.Next(1, 5);
                                        activation = true;
                                    }
                                }
                                else
                                {
                                    // this will trigger for every other diner
                                    if (!forks[x - 1])
                                    {
                                        diners[x] = 1;
                                        forks[x] = true;
                                        forks[x - 1] = true;
                                        timers[x] = (byte)rnd.Next(1, 5);
                                        activation = true;
                                    }
                                }
                            }

                            Console.ForegroundColor = statusColors[diners[x]];
                            if (activation)
                            {
                                Console.WriteLine($"Diner {x} is now eating for {timers[x]} cycles.");
                                info.WriteLine($"Diner {x} is now eating for {timers[x]} cycles.");
                            }
                            else
                            {
                                Console.WriteLine($"Diner {x} failed to activate.");
                                info.WriteLine($"Diner {x} failed to activate.");
                            }
                        }
                        else if (diners[x] == 1)
                        {
                            // diner is eating
                            timers[x]--;
                            if (timers[x] == 0)
                            {
                                diners[x] = 2;
                                timers[x] = (byte)rnd.Next(1, 5);
                                Console.ForegroundColor = statusColors[diners[x]];
                                Console.WriteLine($"Diner {x} is now resting for {timers[x]} cycles.");
                                info.WriteLine($"Diner {x} is now resting for {timers[x]} cycles.");
                                // release the resources
                                forks[x] = false;
                                if (x - 1 < 0)
                                    forks[forks.Length - 1] = false;
                                else
                                    forks[x - 1] = false;
                            }
                            else
                            {
                                Console.ForegroundColor = statusColors[diners[x]];
                                Console.WriteLine($"Diner {x} still has {timers[x]} cycles left to eat.");
                                info.WriteLine($"Diner {x} still has {timers[x]} cycles left to eat.");
                            }
                        }
                        else
                        {
                            // this means the diner is resting
                            timers[x]--;
                            if (timers[x] == 0)
                            {
                                diners[x] = 0;
                                timers[x] = 0;
                                Console.ForegroundColor = statusColors[diners[x]];
                                Console.WriteLine($"Diner {x} is now waiting to be activated");
                                info.WriteLine($"Diner {x} is now waiting to be activated");
                            }
                            else
                            {
                                Console.ForegroundColor = statusColors[diners[x]];
                                Console.WriteLine($"Diner {x} still has {timers[x]} cycles left to rest.");
                                info.WriteLine($"Diner {x} still has {timers[x]} cycles left to rest.");
                            }
                        }
                    }

                    Console.WriteLine();
                    //Console.ReadKey();
                    Thread.Sleep(500);
                }
            }
        }
    }
}
