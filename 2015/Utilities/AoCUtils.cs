using System.Diagnostics;

namespace Utilities
{
    public static class AoCUtils
    {
        static string stars = $"****************************************************************************{Environment.NewLine}";

        public static string[] GetInput(Stopwatch stopwatch, int day, int puzzle, string title)
        {
            Console.WriteLine($"{stars}Advent of Code 2015 Day {day} Puzzle {puzzle}: {title}");
            string[] lines = File.ReadAllLines("Input.txt");
            stopwatch.Restart();
            return lines;
        }

        public static void WriteOutput(Stopwatch stopwatch, string output)
        {
            stopwatch.Stop();
            Console.WriteLine($"Calculated answer is: {output}.");
            Console.WriteLine($"The answer took {stopwatch.Elapsed.TotalSeconds} seconds to calculate.");
            Console.WriteLine(stars);
        }
    }
}