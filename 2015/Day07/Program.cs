using System.Diagnostics;
using Utilities;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 7: Some Assembly Required ---
                This year, Santa brought little Bobby Tables a set of wires and bitwise logic gates! Unfortunately, little Bobby is a little under the recommended age range, and he needs help assembling the circuit.

                Each wire has an identifier (some lowercase letters) and can carry a 16-bit signal (a number from 0 to 65535). A signal is provided to each wire by a gate, another wire, or some specific value. Each wire can only get a signal from one source, but can provide its signal to multiple destinations. A gate provides no signal until all of its inputs have a signal.

                The included instructions booklet describes how to connect the parts together: x AND y -> z means to connect wires x and y to an AND gate, and then connect its output to wire z.

                For example:

                123 -> x means that the signal 123 is provided to wire x.
                x AND y -> z means that the bitwise AND of wire x and wire y is provided to wire z.
                p LSHIFT 2 -> q means that the value from wire p is left-shifted by 2 and then provided to wire q.
                NOT e -> f means that the bitwise complement of the value from wire e is provided to wire f.
                Other possible gates include OR (bitwise OR) and RSHIFT (right-shift). If, for some reason, you'd like to emulate the circuit instead, almost all programming languages (for example, C, JavaScript, or Python) provide operators for these gates.

                For example, here is a simple circuit:

                123 -> x
                456 -> y
                x AND y -> d
                x OR y -> e
                x LSHIFT 2 -> f
                y RSHIFT 2 -> g
                NOT x -> h
                NOT y -> i
                After it is run, these are the signals on the wires:

                d: 72
                e: 507
                f: 492
                g: 114
                h: 65412
                i: 65079
                x: 123
                y: 456
                In little Bobby's kit's instructions booklet (provided as your puzzle input), what signal is ultimately provided to wire a?
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 7, puzzle: 1, title: "Some Assembly Required");

            ushort signalA = Solve(rows);
            AoCUtils.WriteOutput(stopwatch, signalA.ToString());

            /*
             * --- Part Two ---
                Now, take the signal you got on wire a, override wire b to that signal, and reset the other wires (including wire a). What new signal is ultimately provided to wire a?
             */

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].EndsWith("-> b"))
                {
                    rows[i] = signalA + " -> b";
                }
            }

            signalA = Solve(rows);
            AoCUtils.WriteOutput(stopwatch, signalA.ToString());
        }

        static ushort Solve(string[] rows)
        {
            var wireSignals = new Dictionary<string, ushort>();
            List<string> unsolvedRows = new List<string>((string[])rows.Clone());

            while (unsolvedRows.Count > 0)
            {
                for (int i = unsolvedRows.Count - 1; i >= 0; i--)
                {
                    string row = unsolvedRows[i];
                    string[] split1 = row.Split("->", StringSplitOptions.TrimEntries);

                    if (row.Contains("AND"))
                    {
                        string[] andSplit = split1[0].Split("AND", StringSplitOptions.TrimEntries);
                        if (wireSignals.ContainsKey(andSplit[0]) && wireSignals.ContainsKey(andSplit[1]))
                        {
                            ushort wire1 = wireSignals[andSplit[0]];
                            ushort wire2 = wireSignals[andSplit[1]];

                            wireSignals[split1[1]] = (ushort)(wire1 & wire2);
                            unsolvedRows.RemoveAt(i);
                        }
                        else if (ushort.TryParse(andSplit[0], out ushort value) && wireSignals.ContainsKey(andSplit[1]))
                        {
                            ushort wire = wireSignals[andSplit[1]];

                            wireSignals[split1[1]] = (ushort)(value & wire);
                            unsolvedRows.RemoveAt(i);
                        }
                    }
                    else if (row.Contains("OR"))
                    {
                        string[] orSplit = split1[0].Split("OR", StringSplitOptions.TrimEntries);
                        if (wireSignals.ContainsKey(orSplit[0]) && wireSignals.ContainsKey(orSplit[1]))
                        {
                            ushort wire1 = wireSignals[orSplit[0]];
                            ushort wire2 = wireSignals[orSplit[1]];

                            wireSignals[split1[1]] = (ushort)(wire1 | wire2);
                            unsolvedRows.RemoveAt(i);
                        }
                    }
                    else if (row.Contains("NOT"))
                    {
                        string[] notSplit = split1[0].Split("NOT", StringSplitOptions.TrimEntries);
                        if (wireSignals.ContainsKey(notSplit[1]))
                        {
                            ushort wire = wireSignals[notSplit[1]];
                            wireSignals[split1[1]] = (ushort)(~wire);
                            unsolvedRows.RemoveAt(i);
                        }
                    }
                    else if (row.Contains("LSHIFT"))
                    {
                        string[] shiftSplit = split1[0].Split("LSHIFT", StringSplitOptions.TrimEntries);
                        if (wireSignals.ContainsKey(shiftSplit[0]))
                        {
                            ushort wire = wireSignals[shiftSplit[0]];
                            short shift = short.Parse(shiftSplit[1]);

                            wireSignals[split1[1]] = (ushort)(wire << shift);
                            unsolvedRows.RemoveAt(i);
                        }
                    }
                    else if (row.Contains("RSHIFT"))
                    {
                        string[] shiftSplit = split1[0].Split("RSHIFT", StringSplitOptions.TrimEntries);
                        if (wireSignals.ContainsKey(shiftSplit[0]))
                        {
                            ushort wire = wireSignals[shiftSplit[0]];
                            short shift = short.Parse(shiftSplit[1]);

                            wireSignals[split1[1]] = (ushort)(wire >> shift);
                            unsolvedRows.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (ushort.TryParse(split1[0], out ushort value))
                        {
                            wireSignals[split1[1]] = ushort.Parse(split1[0]);
                            unsolvedRows.RemoveAt(i);
                        }
                        else if (wireSignals.ContainsKey(split1[0]))
                        {
                            wireSignals[split1[1]] = wireSignals[split1[0]];
                            unsolvedRows.RemoveAt(i);
                        }
                    }
                }
            }

            return wireSignals["a"];
        }

    }
}