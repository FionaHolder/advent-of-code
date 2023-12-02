using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using Utilities;

namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 4: The Ideal Stocking Stuffer ---
                Santa needs help mining some AdventCoins (very similar to bitcoins) to use as gifts for all the economically forward-thinking little girls and boys.

                To do this, he needs to find MD5 hashes which, in hexadecimal, start with at least five zeroes. The input to the MD5 hash is some secret key (your puzzle input, given below) followed by a number in decimal. To mine AdventCoins, you must find Santa the lowest positive number (no leading zeroes: 1, 2, 3, ...) that produces such a hash.

                For example:

                If your secret key is abcdef, the answer is 609043, because the MD5 hash of abcdef609043 starts with five zeroes (000001dbbfa...), and it is the lowest such number to do so.
                If your secret key is pqrstuv, the lowest number it combines with to make an MD5 hash starting with five zeroes is 1048970; that is, the MD5 hash of pqrstuv1048970 looks like 000006136ef....
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 4, puzzle: 1, title: "The Ideal Stocking Stuffer");
            bool found = false;
            int number = 0;
            MD5 md5 = MD5.Create();
            var encoding = new UTF8Encoding();

            while (!found)
            {
                number++;

                byte[] hash = md5.ComputeHash(encoding.GetBytes(rows[0] + number.ToString()));

                if (hash[0] == 0 && hash[1] == 0 && (hash[2] == 0 || hash[2].ToString().Length == 1))
                {
                    found = true;
                }
            }

            AoCUtils.WriteOutput(stopwatch, number.ToString());

            /*
             * --- Part Two ---
                Now find one that starts with six zeroes.
             */

            rows = AoCUtils.GetInput(stopwatch, day: 4, puzzle: 2, title: "The Ideal Stocking Stuffer");
            found = false;
            number = 0;

            while (!found)
            {
                number++;

                byte[] hash = md5.ComputeHash(encoding.GetBytes(rows[0] + number.ToString()));

                if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                {
                    found = true;
                }
            }

            AoCUtils.WriteOutput(stopwatch, number.ToString());
        }
    }
}