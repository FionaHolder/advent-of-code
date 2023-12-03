using System.Diagnostics;
using System.Text.RegularExpressions;
using Utilities;

namespace Day03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 3: Gear Ratios ---
                You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.

                It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.

                "Aaah!"

                You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone! The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.

                The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.

                The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)

                Here is an example engine schematic:

                467..114..
                ...*......
                ..35..633.
                ......#...
                617*......
                .....+.58.
                ..592.....
                ......755.
                ...$.*....
                .664.598..
                In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.

                Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 3, puzzle: 1, title: "Gear Ratios");
            int total = 0;
            char[] nonSymbols = { '.','0','1','2','3','4','5','6','7','8','9' };
            var coordsWithSymbol = new List<Tuple<int, int>>(); 

            for (int i = 0; i < rows.Length; i++)
            {
                for(int j = 0; j < rows[i].Length; j++)
                {
                    if (!nonSymbols.Contains(rows[i][j]))
                    {
                        coordsWithSymbol.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            var numbersRegex = new Regex("\\d+");
            int rowLength = rows.Any() ? rows.First().Length : 0;
            int numberOfRows = rows.Length;

            for (int i = 0; i < rows.Length; i++)
            {
                foreach (Match match in numbersRegex.Matches(rows[i]))
                {
                    bool adjacentSymbol = false;

                    for (int j = match.Index; j < match.Index + match.Length; j++)
                    {
                        if 
                            ((i > 0 && coordsWithSymbol.Contains(new Tuple<int, int>(i - 1, j))) // above
                            || (i > 0 && j < rowLength && coordsWithSymbol.Contains(new Tuple<int, int>(i - 1, j + 1))) // above-right
                            || (j < rowLength && coordsWithSymbol.Contains(new Tuple<int, int>(i, j + 1))) // right
                            || (i < numberOfRows && j < rowLength && coordsWithSymbol.Contains(new Tuple<int, int>(i + 1, j + 1))) // below-right
                            || (i < numberOfRows && coordsWithSymbol.Contains(new Tuple<int, int>(i + 1, j))) // below
                            || (i < numberOfRows && j > 0 && coordsWithSymbol.Contains(new Tuple<int, int>(i + 1, j - 1))) // below-left
                            || (j > 0 && coordsWithSymbol.Contains(new Tuple<int, int>(i, j - 1))) // left
                            || (i > 0 && j > 0 && coordsWithSymbol.Contains(new Tuple<int, int>(i - 1, j - 1)))) // above-left
                        {
                            adjacentSymbol = true; 
                            break;
                        }
                    }

                    if (adjacentSymbol)
                    {
                        total += Int32.Parse(match.Value);
                    }
                }

            }

            AoCUtils.WriteOutput(stopwatch, total.ToString());

            /*
             * --- Part Two ---
                The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the closest gondola, finally ready to ascend to the water source.

                You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone labeled "help", so you pick it up and the engineer answers.

                Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You exit the gondola.

                The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.

                This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear needs to be replaced.

                Consider the same engine schematic again:

                467..114..
                ...*......
                ..35..633.
                ......#...
                617*......
                .....+.58.
                ..592.....
                ......755.
                ...$.*....
                .664.598..
                In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.

                What is the sum of all of the gear ratios in your engine schematic?
             */

            rows = AoCUtils.GetInput(stopwatch, day: 3, puzzle: 2, title: "Gear Ratios");
            total = 0;
            var gears = new List<Gear>();

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                {
                    if (rows[i][j] == '*')
                    {
                        gears.Add(new Gear() { X = i, Y = j, AdjacentNumbers = new List<int>() });
                    }
                }
            }

            for (int i = 0; i < rows.Length; i++)
            {
                foreach (Match match in numbersRegex.Matches(rows[i]))
                {
                    var gearsByNumber = new List<Gear>();    

                    for (int j = match.Index; j < match.Index + match.Length; j++)
                    {
                        if (i > 0 && gears.Any(g => g.X == i-1 && g.Y == j)) // above
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i - 1 && g.Y == j));
                        }
                        if (i > 0 && j < rowLength && gears.Any(g => g.X == i - 1 && g.Y == j + 1)) // above-right
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i - 1 && g.Y == j + 1));
                        }
                        if (j < rowLength && gears.Any(g => g.X == i && g.Y == j + 1)) // right
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i && g.Y == j + 1));
                        }
                        if (i < numberOfRows && j < rowLength && gears.Any(g => g.X == i + 1 && g.Y == j + 1)) // below-right
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i + 1 && g.Y == j + 1));
                        }
                        if (i < numberOfRows && gears.Any(g => g.X == i + 1 && g.Y == j)) // below
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i + 1 && g.Y == j));
                        }
                        if (i < numberOfRows && j > 0 && gears.Any(g => g.X == i + 1 && g.Y == j - 1)) // below-left
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i + 1 && g.Y == j - 1));
                        }
                        if (j > 0 && gears.Any(g => g.X == i && g.Y == j - 1)) // left
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i && g.Y == j - 1));
                        }
                        if (i > 0 && j > 0 && gears.Any(g => g.X == i - 1 && g.Y == j - 1)) // above-left
                        {
                            gearsByNumber.Add(gears.First(g => g.X == i - 1 && g.Y == j - 1));
                        }
                    }

                    gearsByNumber = gearsByNumber.Distinct().ToList();

                    foreach (var gear in gearsByNumber)
                    {
                        gear.AdjacentNumbers.Add(Int32.Parse(match.Value));
                    }
                }

            }

            foreach(var gear in gears)
            {
                if (gear.AdjacentNumbers.Count == 2)
                {
                    total += (gear.AdjacentNumbers[0] * gear.AdjacentNumbers[1]);
                }
            }

            AoCUtils.WriteOutput(stopwatch, total.ToString());
        }

        private class Gear
        {
            public int X { get; set; }
            public int Y { get; set; }
            public List<int> AdjacentNumbers { get; set; }
        }
    }
}