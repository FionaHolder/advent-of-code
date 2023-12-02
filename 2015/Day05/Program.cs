using System.Diagnostics;
using Utilities;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 5: Doesn't He Have Intern-Elves For This? ---
                Santa needs help figuring out which strings in his text file are naughty or nice.

                A nice string is one with all of the following properties:

                It contains at least three vowels (aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
                It contains at least one letter that appears twice in a row, like xx, abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
                It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
                For example:

                ugknbfddgicrmopn is nice because it has at least three vowels (u...i...o...), a double letter (...dd...), and none of the disallowed substrings.
                aaa is nice because it has at least three vowels and a double letter, even though the letters used by different rules overlap.
                jchzalrnumimnmhp is naughty because it has no double letter.
                haegwjzuvuyypxyu is naughty because it contains the string xy.
                dvszwmarrgswjxmb is naughty because it contains only one vowel.
                How many strings are nice?
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 5, puzzle: 1, title: "Doesn't He Have Intern-Elves For This?");
            int niceStrings = 0;

            foreach (string row in rows) 
            {
                bool atLeastThreeVowels = row.Count(x => new List<char>() { 'a', 'e', 'i', 'o', 'u' }.Contains(x)) >= 3;
                bool oneLetterTwice = false;

                for (int i = 0; i < row.Length; i++)
                {
                    if (i + 1 < row.Length && row[i] == row[i + 1])
                    {
                        oneLetterTwice = true;
                        break;
                    }
                }

                bool noForbiddenStrings = !row.Contains("ab") && !row.Contains("cd") && !row.Contains("pq") && !row.Contains("xy");

                if (atLeastThreeVowels && oneLetterTwice && noForbiddenStrings)
                    niceStrings++;
            }

            AoCUtils.WriteOutput(stopwatch, niceStrings.ToString());

            /*
             * --- Part Two ---
                Realizing the error of his ways, Santa has switched to a better model of determining whether a string is naughty or nice. None of the old rules apply, as they are all clearly ridiculous.

                Now, a nice string is one with all of the following properties:

                It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
                It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
                For example:

                qjhvhtzxzqqjkmpb is nice because is has a pair that appears twice (qj) and a letter that repeats with exactly one letter between them (zxz).
                xxyxx is nice because it has a pair that appears twice and a letter that repeats with one between, even though the letters used by each rule overlap.
                uurcxstgmygtbstg is naughty because it has a pair (tg) but no repeat with a single letter between them.
                ieodomkazucvgmuy is naughty because it has a repeating letter with one between (odo), but no pair that appears twice.
                How many strings are nice under these new rules?
             */

            rows = AoCUtils.GetInput(stopwatch, day: 5, puzzle: 2, title: "Doesn't He Have Intern-Elves For This?");
            niceStrings = 0;

            foreach (string row in rows)
            {
                bool pairOfLettersTwice = false;
                bool repeatedWithLetterBetween = false;

                for (int i = 0; i < row.Length; i++)
                {
                    if (i + 1 < row.Length)
                    {
                        string pair = row[i].ToString() + row[i + 1].ToString();
                        if (row.LastIndexOf(pair) > i+1)
                            pairOfLettersTwice = true;
                    }

                    if (i + 2 < row.Length && row[i] == row[i+2])
                    {
                        repeatedWithLetterBetween |= true;
                    }
                }

                if (pairOfLettersTwice && repeatedWithLetterBetween)
                    niceStrings++;
            }

            AoCUtils.WriteOutput(stopwatch, niceStrings.ToString());
        }
    }
}