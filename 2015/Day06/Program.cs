﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using Utilities;

namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 6: Probably a Fire Hazard ---
                Because your neighbors keep defeating you in the holiday house decorating contest year after year, you've decided to deploy one million lights in a 1000x1000 grid.

                Furthermore, because you've been especially nice this year, Santa has mailed you instructions on how to display the ideal lighting configuration.

                Lights in your grid are numbered from 0 to 999 in each direction; the lights at each corner are at 0,0, 0,999, 999,999, and 999,0. The instructions include whether to turn on, turn off, or toggle various inclusive ranges given as coordinate pairs. Each coordinate pair represents opposite corners of a rectangle, inclusive; a coordinate pair like 0,0 through 2,2 therefore refers to 9 lights in a 3x3 square. The lights all start turned off.

                To defeat your neighbors this year, all you have to do is set up your lights by doing the instructions Santa sent you in order.

                For example:

                turn on 0,0 through 999,999 would turn on (or leave on) every light.
                toggle 0,0 through 999,0 would toggle the first line of 1000 lights, turning off the ones that were on, and turning on the ones that were off.
                turn off 499,499 through 500,500 would turn off (or leave off) the middle four lights.
                After following the instructions, how many lights are lit?
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 6, puzzle: 1, title: "Probably a Fire Hazard");

            var litLights = new bool[1000,1000];
            int lit = 0; 
            Regex regex = new Regex("\\D+(\\d+),(\\d+)\\D+(\\d+),(\\d+)");

            foreach (string row in rows) 
            { 
                var coordinates = regex.Match(row).Groups;
                int fromX = Int32.Parse(coordinates[1].Value);
                int fromY = Int32.Parse(coordinates[2].Value);
                int toX = Int32.Parse(coordinates[3].Value);
                int toY = Int32.Parse(coordinates[4].Value);

                for (int i = fromX; i <= toX; i++)
                {
                    for (int j = fromY; j <= toY; j++)
                    {
                        bool currentlyLit = litLights[i,j];

                        if ((row.Contains("turn on") && !currentlyLit)
                            || (row.Contains("toggle") && !currentlyLit))
                        {
                            litLights[i, j] = true;
                            lit++;
                        }

                        if ((row.Contains("turn off") && currentlyLit)
                            || (row.Contains("toggle") && currentlyLit))
                        {
                            litLights[i, j] = false;
                            lit--;
                        }
                    }
                }
            }

            AoCUtils.WriteOutput(stopwatch, lit.ToString());

            /*
             * --- Part Two ---
                You just finish implementing your winning light pattern when you realize you mistranslated Santa's message from Ancient Nordic Elvish.

                The light grid you bought actually has individual brightness controls; each light can have a brightness of zero or more. The lights all start at zero.

                The phrase turn on actually means that you should increase the brightness of those lights by 1.

                The phrase turn off actually means that you should decrease the brightness of those lights by 1, to a minimum of zero.

                The phrase toggle actually means that you should increase the brightness of those lights by 2.

                What is the total brightness of all lights combined after following Santa's instructions?

                For example:

                turn on 0,0 through 0,0 would increase the total brightness by 1.
                toggle 0,0 through 999,999 would increase the total brightness by 2000000.
             */

            rows = AoCUtils.GetInput(stopwatch, day: 6, puzzle: 1, title: "Probably a Fire Hazard");

            var lightBrightness = new int[1000, 1000];
            int total = 0;

            foreach (string row in rows)
            {
                var coordinates = regex.Match(row).Groups;
                int fromX = Int32.Parse(coordinates[1].Value);
                int fromY = Int32.Parse(coordinates[2].Value);
                int toX = Int32.Parse(coordinates[3].Value);
                int toY = Int32.Parse(coordinates[4].Value);

                for (int i = fromX; i <= toX; i++)
                {
                    for (int j = fromY; j <= toY; j++)
                    {
                        int currentBrightness = lightBrightness[i, j];

                        if (row.Contains("turn on"))
                        {
                            lightBrightness[i, j]++;
                            total++;
                        }

                        if (row.Contains("turn off") && lightBrightness[i, j] > 0)
                        {
                            lightBrightness[i, j]--;
                            total--;
                        }

                        if (row.Contains("toggle"))
                        {
                            lightBrightness[i, j] += 2;
                            total += 2;
                        }
                    }
                }
            }

            AoCUtils.WriteOutput(stopwatch, total.ToString());
        }
    }
}