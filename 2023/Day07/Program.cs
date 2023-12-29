using System.Diagnostics;
using Utilities;

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * --- Day 7: Camel Cards ---
                Your all-expenses-paid trip turns out to be a one-way, five-minute ride in an airship. (At least it's a cool airship!) It drops you off at the edge of a vast desert and descends back to Island Island.

                "Did you bring the parts?"

                You turn around to see an Elf completely covered in white clothing, wearing goggles, and riding a large camel.

                "Did you bring the parts?" she asks again, louder this time. You aren't sure what parts she's looking for; you're here to figure out why the sand stopped.

                "The parts! For the sand, yes! Come with me; I will show you." She beckons you onto the camel.

                After riding a bit across the sands of Desert Island, you can see what look like very large rocks covering half of the horizon. The Elf explains that the rocks are all along the part of Desert Island that is directly above Island Island, making it hard to even get there. Normally, they use big machines to move the rocks and filter the sand, but the machines have broken down because Desert Island recently stopped receiving the parts they need to fix the machines.

                You've already assumed it'll be your job to figure out why the parts stopped when she asks if you can help. You agree automatically.

                Because the journey will take a few days, she offers to teach you the game of Camel Cards. Camel Cards is sort of similar to poker except it's designed to be easier to play while riding a camel.

                In Camel Cards, you get a list of hands, and your goal is to order them based on the strength of each hand. A hand consists of five cards labeled one of A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2. The relative strength of each card follows this order, where A is the highest and 2 is the lowest.

                Every hand is exactly one type. From strongest to weakest, they are:

                Five of a kind, where all five cards have the same label: AAAAA
                Four of a kind, where four cards have the same label and one card has a different label: AA8AA
                Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
                Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
                Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
                One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
                High card, where all cards' labels are distinct: 23456
                Hands are primarily ordered based on type; for example, every full house is stronger than any three of a kind.

                If two hands have the same type, a second ordering rule takes effect. Start by comparing the first card in each hand. If these cards are different, the hand with the stronger first card is considered stronger. If the first card in each hand have the same label, however, then move on to considering the second card in each hand. If they differ, the hand with the higher second card wins; otherwise, continue with the third card in each hand, then the fourth, then the fifth.

                So, 33332 and 2AAAA are both four of a kind hands, but 33332 is stronger because its first card is stronger. Similarly, 77888 and 77788 are both a full house, but 77888 is stronger because its third card is stronger (and both hands have the same first and second card).

                To play Camel Cards, you are given a list of hands and their corresponding bid (your puzzle input). For example:

                32T3K 765
                T55J5 684
                KK677 28
                KTJJT 220
                QQQJA 483
                This example shows five hands; each hand is followed by its bid amount. Each hand wins an amount equal to its bid multiplied by its rank, where the weakest hand gets rank 1, the second-weakest hand gets rank 2, and so on up to the strongest hand. Because there are five hands in this example, the strongest hand will have rank 5 and its bid will be multiplied by 5.

                So, the first step is to put the hands in order of strength:

                32T3K is the only one pair and the other hands are all a stronger type, so it gets rank 1.
                KK677 and KTJJT are both two pair. Their first cards both have the same label, but the second card of KK677 is stronger (K vs T), so KTJJT gets rank 2 and KK677 gets rank 3.
                T55J5 and QQQJA are both three of a kind. QQQJA has a stronger first card, so it gets rank 5 and T55J5 gets rank 4.
                Now, you can determine the total winnings of this set of hands by adding up the result of multiplying each hand's bid with its rank (765 * 1 + 220 * 2 + 28 * 3 + 684 * 4 + 483 * 5). So the total winnings in this example are 6440.

                Find the rank of every hand in your set. What are the total winnings?
             */

            var stopwatch = new Stopwatch();
            string[] rows = AoCUtils.GetInput(stopwatch, day: 7, puzzle: 1, title: "Camel Cards");

            var rankedInputs = new List<Input>();

            foreach(string row in rows)
            {
                var split = row.Split(" ");
                var input = new Input()
                {
                    Hand = split[0],
                    Bid = Int32.Parse(split[1]),
                    Rank = 0
                };

                rankedInputs.Add(input);
            }

            rankedInputs.Sort((firstInput, secondInput) =>
                {
                    return firstInput.CompareTo(secondInput);
                }
            );

            for(int i = 0; i < rankedInputs.Count; i++)
            {
                rankedInputs[i].Rank = i + 1;
            }

            int result = rankedInputs.Sum(x => x.Rank * x.Bid);

            AoCUtils.WriteOutput(stopwatch, result.ToString());

            /*
             * --- Part Two ---
                To make things a little more interesting, the Elf introduces one additional rule. Now, J cards are jokers - wildcards that can act like whatever card would make the hand the strongest type possible.

                To balance this, J cards are now the weakest individual cards, weaker even than 2. The other cards stay in the same order: A, K, Q, T, 9, 8, 7, 6, 5, 4, 3, 2, J.

                J cards can pretend to be whatever card is best for the purpose of determining hand type; for example, QJJQ2 is now considered four of a kind. However, for the purpose of breaking ties between two hands of the same type, J is always treated as J, not the card it's pretending to be: JKKK2 is weaker than QQQQ2 because J is weaker than Q.

                Now, the above example goes very differently:

                32T3K 765
                T55J5 684
                KK677 28
                KTJJT 220
                QQQJA 483
                32T3K is still the only one pair; it doesn't contain any jokers, so its strength doesn't increase.
                KK677 is now the only two pair, making it the second-weakest hand.
                T55J5, KTJJT, and QQQJA are now all four of a kind! T55J5 gets rank 3, QQQJA gets rank 4, and KTJJT gets rank 5.
                With the new joker rule, the total winnings in this example are 5905.

                Using the new joker rule, find the rank of every hand in your set. What are the new total winnings?
             */

            rows = AoCUtils.GetInput(stopwatch, day: 7, puzzle: 2, title: "Camel Cards");

            var rankedJokerInputs = new List<JokerInput>();

            foreach (string row in rows)
            {
                var split = row.Split(" ");
                var input = new JokerInput()
                {
                    Hand = split[0],
                    Bid = Int32.Parse(split[1]),
                    Rank = 0
                };

                rankedJokerInputs.Add(input);
            }

            rankedJokerInputs.Sort((firstInput, secondInput) =>
            {
                return firstInput.CompareTo(secondInput);
            }
            );

            for (int i = 0; i < rankedJokerInputs.Count; i++)
            {
                rankedJokerInputs[i].Rank = i + 1;
            }

            int jokerResult = rankedJokerInputs.Sum(x => x.Rank * x.Bid);

            AoCUtils.WriteOutput(stopwatch, jokerResult.ToString());
        }

        private class Input : IComparable<Input>
        {
            public string Hand { get; set; }

            public List<int> NumericHand
            {
                get
                {
                    return Hand.ToCharArray().ToList().ConvertAll(c => GetNumberForCard(c));
                }
            }

            private int GetNumberForCard(char c)
            {
                switch(c)
                {
                    case 'A':
                        return 14;
                    case 'K':
                        return 13;
                    case 'Q':
                        return 12;
                    case 'J':
                        return 11;
                    case 'T':
                        return 10;
                    default:
                        return Int32.Parse(c.ToString());
                }
            }

            public int Bid { get; set; }
            public int Rank { get; set; }

            public int CompareTo(Input? other)
            {
                /* A value that indicates the relative order of the objects being compared. The return value has these meanings:
                Value	                Meaning
                Less than zero	        This instance precedes other in the sort order.
                Zero	                This instance occurs in the same position in the sort order as other.
                Greater than zero	    This instance follows other in the sort order.
                 */
                if (other == null) throw new ArgumentNullException();

                // Five of a kind, where all five cards have the same label: AAAAA
                if (OfAKind(this.NumericHand, 5))
                {
                    if (OfAKind(other.NumericHand, 5))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1; 
                }
                else if (OfAKind(other.NumericHand, 5))
                {
                    return -1;
                }

                // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
                if (OfAKind(this.NumericHand, 4))
                {
                    if (OfAKind(other.NumericHand, 4))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (OfAKind(other.NumericHand, 4))
                {
                    return -1;
                }

                // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
                if (FullHouse(this.NumericHand))
                {
                    if (FullHouse(other.NumericHand))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (FullHouse(other.NumericHand))
                {
                    return -1;
                }

                // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
                if (OfAKind(this.NumericHand, 3))
                {
                    if (OfAKind(other.NumericHand, 3))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (OfAKind(other.NumericHand, 3))
                {
                    return -1;
                }

                // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
                if (TwoPair(this.NumericHand))
                {
                    if (TwoPair(other.NumericHand))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (TwoPair(other.NumericHand))
                {
                    return -1;
                }

                // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
                if (OfAKind(this.NumericHand, 2))
                {
                    if (OfAKind(other.NumericHand, 2))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (OfAKind(other.NumericHand, 2))
                {
                    return -1;
                }

                return TieBreak(this.NumericHand, other.NumericHand);
            }

            public override string ToString()
            {
                return $"Rank {Rank} for Hand {Hand} with bid {Bid}";
            }
        }

        private class JokerInput : IComparable<JokerInput>
        {
            public string Hand { get; set; }

            public List<int> NumericHand
            {
                get
                {
                    return Hand.ToCharArray().ToList().ConvertAll(c => GetNumberForCard(c));
                }
            }

            private int GetNumberForCard(char c)
            {
                switch (c)
                {
                    case 'A':
                        return 14;
                    case 'K':
                        return 13;
                    case 'Q':
                        return 12;
                    case 'J':
                        return 1; // Joker
                    case 'T':
                        return 10;
                    default:
                        return Int32.Parse(c.ToString());
                }
            }

            public int Bid { get; set; }
            public int Rank { get; set; }

            public int CompareTo(JokerInput? other)
            {
                /* A value that indicates the relative order of the objects being compared. The return value has these meanings:
                Value	                Meaning
                Less than zero	        This instance precedes other in the sort order.
                Zero	                This instance occurs in the same position in the sort order as other.
                Greater than zero	    This instance follows other in the sort order.
                 */
                if (other == null) throw new ArgumentNullException();

                // Five of a kind, where all five cards have the same label: AAAAA
                if (JokerOfAKind(this.NumericHand, 5))
                {
                    if (JokerOfAKind(other.NumericHand, 5))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (JokerOfAKind(other.NumericHand, 5))
                {
                    return -1;
                }

                // Four of a kind, where four cards have the same label and one card has a different label: AA8AA
                if (JokerOfAKind(this.NumericHand, 4))
                {
                    if (JokerOfAKind(other.NumericHand, 4))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (JokerOfAKind(other.NumericHand, 4))
                {
                    return -1;
                }

                // Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
                if (JokerFullHouse(this.NumericHand))
                {
                    if (JokerFullHouse(other.NumericHand))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (JokerFullHouse(other.NumericHand))
                {
                    return -1;
                }

                // Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
                if (JokerOfAKind(this.NumericHand, 3))
                {
                    if (JokerOfAKind(other.NumericHand, 3))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (JokerOfAKind(other.NumericHand, 3))
                {
                    return -1;
                }

                // Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
                if (JokerTwoPair(this.NumericHand))
                {
                    if (JokerTwoPair(other.NumericHand))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (JokerTwoPair(other.NumericHand))
                {
                    return -1;
                }

                // One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
                if (JokerOfAKind(this.NumericHand, 2))
                {
                    if (JokerOfAKind(other.NumericHand, 2))
                    {
                        return TieBreak(this.NumericHand, other.NumericHand);
                    }

                    return 1;
                }
                else if (JokerOfAKind(other.NumericHand, 2))
                {
                    return -1;
                }

                return TieBreak(this.NumericHand, other.NumericHand);
            }

            private bool JokerOfAKind(List<int> numericHand, int number)
            {
                for (int i = 1; i <= 14; i++)
                {
                    List<int> tempHand = new List<int>()
                    {
                        numericHand[0] == 1 ? i : numericHand[0],
                        numericHand[1] == 1 ? i : numericHand[1],
                        numericHand[2] == 1 ? i : numericHand[2],
                        numericHand[3] == 1 ? i : numericHand[3],
                        numericHand[4] == 1 ? i : numericHand[4],
                    };

                    if (OfAKind(tempHand, number)) return true;
                }

                return false;
            }

            private bool JokerFullHouse(List<int> numericHand)
            {
                for (int i = 1; i <= 14; i++)
                {
                    List<int> tempHand = new List<int>()
                    {
                        numericHand[0] == 1 ? i : numericHand[0],
                        numericHand[1] == 1 ? i : numericHand[1],
                        numericHand[2] == 1 ? i : numericHand[2],
                        numericHand[3] == 1 ? i : numericHand[3],
                        numericHand[4] == 1 ? i : numericHand[4],
                    };

                    if (FullHouse(tempHand)) return true;
                }

                return false;
            }

            private bool JokerTwoPair(List<int> numericHand)
            {
                for (int i = 1; i <= 14; i++)
                {
                    List<int> tempHand = new List<int>()
                    {
                        numericHand[0] == 1 ? i : numericHand[0],
                        numericHand[1] == 1 ? i : numericHand[1],
                        numericHand[2] == 1 ? i : numericHand[2],
                        numericHand[3] == 1 ? i : numericHand[3],
                        numericHand[4] == 1 ? i : numericHand[4],
                    };

                    if (TwoPair(tempHand)) return true;
                }

                return false;
            }

            public override string ToString()
            {
                return $"Rank {Rank} for Hand {Hand} with bid {Bid}";
            }
        }


        private static int TieBreak(List<int> input, List<int> other)
        {
            for (int i = 0; i < 5; i++)
            {
                if (input[i] != other[i])
                {
                    return input[i].CompareTo(other[i]);
                }
            }

            return 0;
        }

        private static bool OfAKind(List<int> numericHand, int number)
        {
            var x = numericHand.GroupBy(x => x).
                OrderByDescending(x => x.Count());

            return x.First().Count() == number;
        }
        private static bool FullHouse(List<int> numericHand)
        {
            var x = numericHand.GroupBy(x => x).
                OrderByDescending(x => x.Count());

            return x.First().Count() == 3 && x.Last().Count() == 2;
        }

        private static bool TwoPair(List<int> numericHand)
        {
            var x = numericHand.GroupBy(x => x).
                OrderByDescending(x => x.Count());

            return x.First().Count() == 2 && x.ElementAt(1).Count() == 2;
        }

    }
}