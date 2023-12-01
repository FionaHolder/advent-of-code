using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using Utilities;

namespace Day01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* --- Day 1: Trebuchet?! ---
             * 
            Something is wrong with global snow production, and you've been selected to take a look. The Elves have even given you a map; on it, they've used stars to mark the top fifty locations that are likely to be having problems.

            You've been doing this long enough to know that to restore snow operations, you need to check all fifty stars by December 25th.

            Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

            You try to ask why they can't just use a weather machine ("not powerful enough") and where they're even sending you ("the sky") and why your map looks mostly blank ("you sure ask a lot of questions") and hang on did you just say the sky ("of course, where do you think snow comes from") when you realize that the Elves are already loading you into a trebuchet ("please hold still, we need to strap you in").

            As they're making the final adjustments, they discover that their calibration document (your puzzle input) has been amended by a very young Elf who was apparently just excited to show off her art skills. Consequently, the Elves are having trouble reading the values on the document.

            The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.

            For example:

            1abc2
            pqr3stu8vwx
            a1b2c3d4e5f
            treb7uchet
            In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.

            Consider your entire calibration document. What is the sum of all of the calibration values?
            */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 1, puzzle: 1, title: "Trebuchet?!");  
            int total = 0;

            foreach (string row in rows)
            {
                string firstNumber = row.First(x => int.TryParse(x.ToString(), out int i)).ToString();
                string lastNumber = row.Last(x => int.TryParse(x.ToString(), out int i)).ToString();

                total += Int32.Parse(firstNumber + lastNumber);
            }

            AoCUtils.WriteOutput(stopwatch, total.ToString());

            /*
             * --- Part Two ---
                Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".

                Equipped with this new information, you now need to find the real first and last digit on each line. For example:

                two1nine
                eightwothree
                abcone2threexyz
                xtwone3four
                4nineeightseven2
                zoneight234
                7pqrstsixteen
                In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.

                What is the sum of all of the calibration values?
             */

            rows = AoCUtils.GetInput(stopwatch, day: 1, puzzle: 2, title: "Trebuchet?!");
            total = 0;

            foreach (string row in rows)
            {
                var stringIndexes = new List<NumberLocation>();

                for (int i = 1; i < 10; i++)
                {
                    int firstIndex = row.IndexOf(i.ToString());
                    int lastIndex = row.LastIndexOf(i.ToString());

                    if (firstIndex >= 0)
                    {
                        stringIndexes.Add(new NumberLocation() { Number = i.ToString(), FirstIndex = firstIndex, LastIndex = lastIndex });
                    }
                }

                foreach(string num in new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" })
                {
                    int firstIndex = row.IndexOf(num);
                    int lastIndex = row.LastIndexOf(num);

                    if (firstIndex >= 0)
                    {
                        stringIndexes.Add(new NumberLocation() { Number = num, FirstIndex = firstIndex, LastIndex = lastIndex });
                    }
                }

                string firstNumber = stringIndexes.OrderBy(x => x.FirstIndex).First().NumberInteger;
                string lastNumber = stringIndexes.OrderBy(x => x.LastIndex).Last().NumberInteger;


                total += Int32.Parse(firstNumber +lastNumber);
            }

            AoCUtils.WriteOutput(stopwatch, total.ToString());

        }

        private class NumberLocation
        {
            public string Number { get; set;}

            public string NumberInteger
            {
                get
                {
                    switch (Number)
                    {
                        case "one": return "1";
                        case "two": return "2";
                        case "three": return "3";
                        case "four": return "4";
                        case "five": return "5";
                        case "six": return "6";
                        case "seven": return "7";
                        case "eight": return "8";
                        case "nine": return "9";
                        default: return Number;
                    }
                }
            }

            public int FirstIndex { get; set;}
            public int LastIndex { get; set;}  
        }
    }
}