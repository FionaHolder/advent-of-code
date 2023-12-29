using System.Diagnostics;
using Utilities;

namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 5: If You Give A Seed A Fertilizer ---
                You take the boat and find the gardener right where you were told he would be: managing a giant "garden" that looks more to you like a farm.

                "A water source? Island Island is the water source!" You point out that Snow Island isn't receiving any water.

                "Oh, we had to stop the water because we ran out of sand to filter it with! Can't make snow with dirty water. Don't worry, I'm sure we'll get more sand soon; we only turned off the water a few days... weeks... oh no." His face sinks into a look of horrified realization.

                "I've been so busy making sure everyone here has food that I completely forgot to check why we stopped getting more sand! There's a ferry leaving soon that is headed over in that direction - it's much faster than your boat. Could you please go check it out?"

                You barely have time to agree to this request when he brings up another. "While you wait for the ferry, maybe you can help us with our food production problem. The latest Island Island Almanac just arrived and we're having trouble making sense of it."

                The almanac (your puzzle input) lists all of the seeds that need to be planted. It also lists what type of soil to use with each kind of seed, what type of fertilizer to use with each kind of soil, what type of water to use with each kind of fertilizer, and so on. Every type of seed, soil, fertilizer and so on is identified with a number, but numbers are reused by each category - that is, soil 123 and fertilizer 123 aren't necessarily related to each other.

                For example:

                seeds: 79 14 55 13

                seed-to-soil map:
                50 98 2
                52 50 48

                soil-to-fertilizer map:
                0 15 37
                37 52 2
                39 0 15

                fertilizer-to-water map:
                49 53 8
                0 11 42
                42 0 7
                57 7 4

                water-to-light map:
                88 18 7
                18 25 70

                light-to-temperature map:
                45 77 23
                81 45 19
                68 64 13

                temperature-to-humidity map:
                0 69 1
                1 0 69

                humidity-to-location map:
                60 56 37
                56 93 4
                The almanac starts by listing which seeds need to be planted: seeds 79, 14, 55, and 13.

                The rest of the almanac contains a list of maps which describe how to convert numbers from a source category into numbers in a destination category. That is, the section that starts with seed-to-soil map: describes how to convert a seed number (the source) to a soil number (the destination). This lets the gardener and his team know which soil to use with which seeds, which water to use with which fertilizer, and so on.

                Rather than list every source number and its corresponding destination number one by one, the maps describe entire ranges of numbers that can be converted. Each line within a map contains three numbers: the destination range start, the source range start, and the range length.

                Consider again the example seed-to-soil map:

                50 98 2
                52 50 48
                The first line has a destination range start of 50, a source range start of 98, and a range length of 2. This line means that the source range starts at 98 and contains two values: 98 and 99. The destination range is the same length, but it starts at 50, so its two values are 50 and 51. With this information, you know that seed number 98 corresponds to soil number 50 and that seed number 99 corresponds to soil number 51.

                The second line means that the source range starts at 50 and contains 48 values: 50, 51, ..., 96, 97. This corresponds to a destination range starting at 52 and also containing 48 values: 52, 53, ..., 98, 99. So, seed number 53 corresponds to soil number 55.

                Any source numbers that aren't mapped correspond to the same destination number. So, seed number 10 corresponds to soil number 10.

                So, the entire list of seed numbers and their corresponding soil numbers looks like this:

                seed  soil
                0     0
                1     1
                ...   ...
                48    48
                49    49
                50    52
                51    53
                ...   ...
                96    98
                97    99
                98    50
                99    51
                With this map, you can look up the soil number required for each initial seed number:

                Seed number 79 corresponds to soil number 81.
                Seed number 14 corresponds to soil number 14.
                Seed number 55 corresponds to soil number 57.
                Seed number 13 corresponds to soil number 13.
                The gardener and his team want to get started as soon as possible, so they'd like to know the closest location that needs a seed. Using these maps, find the lowest location number that corresponds to any of the initial seeds. To do this, you'll need to convert each seed number through other categories until you can find its corresponding location number. In this example, the corresponding types are:

                Seed 79, soil 81, fertilizer 81, water 81, light 74, temperature 78, humidity 78, location 82.
                Seed 14, soil 14, fertilizer 53, water 49, light 42, temperature 42, humidity 43, location 43.
                Seed 55, soil 57, fertilizer 57, water 53, light 46, temperature 82, humidity 82, location 86.
                Seed 13, soil 13, fertilizer 52, water 41, light 34, temperature 34, humidity 35, location 35.
                So, the lowest location number in this example is 35.

                What is the lowest location number that corresponds to any of the initial seed numbers?
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 5, puzzle: 1, title: "If You Give A Seed A Fertilizer");

            List<long> seedsToPlant = rows[0].Split(':', StringSplitOptions.TrimEntries)[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToList().ConvertAll(x => Int64.Parse(x));

            int seedSoilIndex = -1, soilFertiliserIndex = -1, fertiliserWaterIndex = -1, waterLightIndex = -1
                , lightTemperatureIndex = -1, temperatureHumidityIndex = -1, humidityLocationIndex = -1;

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == "seed-to-soil map:")
                    seedSoilIndex = i + 1;
                else if (rows[i] == "soil-to-fertilizer map:")
                    soilFertiliserIndex = i + 1;
                else if (rows[i] == "fertilizer-to-water map:")
                    fertiliserWaterIndex = i + 1;
                else if (rows[i] == "water-to-light map:")
                    waterLightIndex = i + 1;
                else if (rows[i] == "light-to-temperature map:")
                    lightTemperatureIndex = i + 1;
                else if (rows[i] == "temperature-to-humidity map:")
                    temperatureHumidityIndex = i + 1;
                else if (rows[i] == "humidity-to-location map:")
                    humidityLocationIndex = i + 1;
            }

            long minLocation = long.MaxValue;

            foreach (long seed in seedsToPlant)
            {
                long mappedValue = seed;
                mappedValue = FindMapForValue(rows.Skip(seedSoilIndex).Take(soilFertiliserIndex - seedSoilIndex - 2), mappedValue);
                mappedValue = FindMapForValue(rows.Skip(soilFertiliserIndex).Take(fertiliserWaterIndex - soilFertiliserIndex - 2), mappedValue);
                mappedValue = FindMapForValue(rows.Skip(fertiliserWaterIndex).Take(waterLightIndex - fertiliserWaterIndex - 2), mappedValue);
                mappedValue = FindMapForValue(rows.Skip(waterLightIndex).Take(lightTemperatureIndex - waterLightIndex - 2), mappedValue);
                mappedValue = FindMapForValue(rows.Skip(lightTemperatureIndex).Take(temperatureHumidityIndex - lightTemperatureIndex - 2), mappedValue);
                mappedValue = FindMapForValue(rows.Skip(temperatureHumidityIndex).Take(humidityLocationIndex - temperatureHumidityIndex - 2), mappedValue);
                mappedValue = FindMapForValue(rows.Skip(humidityLocationIndex).Take(rows.Length - humidityLocationIndex), mappedValue);

                minLocation = Math.Min(minLocation, mappedValue);
            }

            AoCUtils.WriteOutput(stopwatch, minLocation.ToString());

            /*
             * --- Part Two ---
                Everyone will starve if you only plant such a small number of seeds. Re-reading the almanac, it looks like the seeds: line actually describes ranges of seed numbers.

                The values on the initial seeds: line come in pairs. Within each pair, the first value is the start of the range and the second value is the length of the range. So, in the first line of the example above:

                seeds: 79 14 55 13
                This line describes two ranges of seed numbers to be planted in the garden. The first range starts with seed number 79 and contains 14 values: 79, 80, ..., 91, 92. The second range starts with seed number 55 and contains 13 values: 55, 56, ..., 66, 67.

                Now, rather than considering four seed numbers, you need to consider a total of 27 seed numbers.

                In the above example, the lowest location number can be obtained from seed number 82, which corresponds to soil 84, fertilizer 84, water 84, light 77, temperature 45, humidity 46, and location 46. So, the lowest location number is 46.

                Consider all of the initial seed numbers listed in the ranges on the first line of the almanac. What is the lowest location number that corresponds to any of the initial seed numbers?
             */

            rows = AoCUtils.GetInput(stopwatch, day: 5, puzzle: 2, title: "If You Give A Seed A Fertilizer");
            minLocation = long.MaxValue;

            var inputRows = new Dictionary<int, List<Node>>();
            inputRows.Add(1, GetNodesFromRows(rows.Skip(seedSoilIndex).Take(soilFertiliserIndex - seedSoilIndex - 2), depth: 1).OrderBy(x => x.SourceFrom).ToList());
            inputRows.Add(2, GetNodesFromRows(rows.Skip(soilFertiliserIndex).Take(fertiliserWaterIndex - soilFertiliserIndex - 2), depth: 2).OrderBy(x => x.SourceFrom).ToList());
            inputRows.Add(3, GetNodesFromRows(rows.Skip(fertiliserWaterIndex).Take(waterLightIndex - fertiliserWaterIndex - 2), depth: 3).OrderBy(x => x.SourceFrom).ToList());
            inputRows.Add(4, GetNodesFromRows(rows.Skip(waterLightIndex).Take(lightTemperatureIndex - waterLightIndex - 2), depth: 4).OrderBy(x => x.SourceFrom).ToList());
            inputRows.Add(5, GetNodesFromRows(rows.Skip(lightTemperatureIndex).Take(temperatureHumidityIndex - lightTemperatureIndex - 2), depth: 5).OrderBy(x => x.SourceFrom).ToList());
            inputRows.Add(6, GetNodesFromRows(rows.Skip(temperatureHumidityIndex).Take(humidityLocationIndex - temperatureHumidityIndex - 2), depth: 6).OrderBy(x => x.SourceFrom).ToList());
            inputRows.Add(7, GetNodesFromRows(rows.Skip(humidityLocationIndex).Take(rows.Length - humidityLocationIndex), depth: 7).OrderBy(x => x.SourceFrom).ToList());

            var seeds = new List<Seed>();
            long seedFrom = -1;

            foreach (long seed in seedsToPlant)
            {
                if (seedFrom == -1)
                {
                    seedFrom = seed;
                }
                else
                {
                    var newSeed = new Seed()
                    {
                        From = seedFrom,
                        To = seedFrom + seed
                    };
                    seedFrom = -1;

                    newSeed.Children = GetChildren(newSeed.From, newSeed.To, depth: 1, inputRows[1]);

                    foreach(Node a in newSeed.Children)
                    {
                        a.Children = GetChildren(a.DestFrom, a.DestTo, depth: 2, inputRows[2]);

                        foreach (Node b in a.Children)
                        {
                            b.Children = GetChildren(b.DestFrom, b.DestTo, depth: 3, inputRows[3]);

                            foreach (Node c in b.Children)
                            {
                                c.Children = GetChildren(c.DestFrom, c.DestTo, depth: 4, inputRows[4]);

                                foreach (Node d in c.Children)
                                {
                                    d.Children = GetChildren(d.DestFrom, d.DestTo, depth: 5, inputRows[5]);

                                    foreach (Node e in d.Children)
                                    {
                                        e.Children = GetChildren(e.DestFrom, e.DestTo, depth: 6, inputRows[6]);

                                        foreach (Node f in e.Children)
                                        {
                                            f.Children = GetChildren(f.DestFrom, f.DestTo, depth: 7, inputRows[7]);

                                            foreach(Node location in f.Children)
                                            {
                                                minLocation = Math.Min(minLocation, location.DestFrom);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    seeds.Add(newSeed);
                }
            }

            AoCUtils.WriteOutput(stopwatch, minLocation.ToString());

            static List<Node> GetChildren(long from, long to, int depth, List<Node> potentialChildren)
            {
                var children = new List<Node>();
                long num = from;
                while (num <= to)
                {
                    var child = potentialChildren.FirstOrDefault(x => x.SourceTo >= num && x.SourceFrom <= num);
                    if (child != null)
                    {
                        children.Add(new Node()
                        {
                            Depth = depth,
                            SourceFrom = Math.Max(num, child.SourceFrom),
                            SourceTo = Math.Min(to, child.SourceTo),
                            DestFrom = Math.Max(num, child.SourceFrom) + child.Difference,
                            DestTo = Math.Min(to, child.SourceTo) + child.Difference
                        });
                        num = Math.Min(to, child.SourceTo) + 1;
                    }
                    else
                    {
                        children.Add(new Node()
                        {
                            Depth = depth,
                            SourceFrom = num,
                            SourceTo = to,
                            DestFrom = num,
                            DestTo = to
                        });
                        num = to + 1;
                    }
                }

                return children;
            }
        }

        private class Seed
        {
            public long From { get; set; }

            public long To { get; set; }

            public List<Node> Children { get; set; } = new List<Node>();
            public override string ToString()
            {
                return $"Seed {From} - {To}";
            }

        }

        private class Node
        {
            public List<Node> Children { get; set; } = new List<Node>();

            public int Depth { get; set; }
            public long SourceFrom { get; set; }
            public long SourceTo { get; set; }
            public long DestFrom { get; set; }
            public long DestTo { get; set; }
            public long Difference { get { return DestFrom - SourceFrom; } }

            public override string ToString()
            {
                return $"Depth {Depth}: Map {SourceFrom} - {SourceTo} onto {DestFrom} - {DestTo}";
            }

        }

        private static IEnumerable<Node> GetNodesFromRows(IEnumerable<string> rows, int depth)
        {
            foreach (var row in rows)
            {
                string[] split = row.Split(' ');

                yield return new Node()
                {
                    Depth = depth,
                    SourceFrom = Int64.Parse(split[1]),
                    SourceTo = Int64.Parse(split[1]) + Int64.Parse(split[2]) - 1,
                    DestFrom = Int64.Parse(split[0]),
                    DestTo = Int64.Parse(split[0]) + Int64.Parse(split[2]) - 1
                };
            }
        }

        private static long FindMapForValue(IEnumerable<string> rows, long value)
        {
            foreach (var row in rows)
            {
                string[] split = row.Split(' ');
                long destRangeStart = Int64.Parse(split[0]);
                long sourceRangeStart = Int64.Parse(split[1]);
                long rangeLength = Int64.Parse(split[2]);

                if (sourceRangeStart <= value && sourceRangeStart + rangeLength > value) 
                {
                    value = value + (destRangeStart - sourceRangeStart);
                    break;
                }
            }

            return value;
        }
    }
}