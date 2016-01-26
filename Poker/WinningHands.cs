namespace Poker
{
    using System.Collections.Generic;
    using System.Linq;

    public class WinningHands
    {
        public static void rStraightFlush(
            ref double current, 
            ref double power, 
            int[] st1, 
            int[] st2, 
            int[] st3, 
            int[] st4,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        power = (st1.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        power = (st1.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        power = (st2.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        power = st2.Max() / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        power = (st3.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        power = (st3.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        power = (st4.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        power = (st4.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public static void rFourOfAKind(
            ref double current, 
            ref double power, 
            int[] straight,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        current = 7;
                        power = (straight[j] / 4) * 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 7 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0 && straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        power = 13 * 4 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 7 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        
        public static void rFullHouse(
            ref double current, 
            ref double power, 
            ref bool done, 
            int[] straight,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                type = power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                power = 13 * 2 + current * 100;
                                winningHands.Add(new Type() { Power = power, Current = 6 });
                                sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                power = fh.Max() / 4 * 2 + current * 100;
                                winningHands.Add(new Type() { Power = power, Current = 6 });
                                sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (current != 6)
                {
                    power = type;
                }
            }
        }

        public static void rFlush(
            ref double current, 
            ref double power, 
            ref bool vf, 
            int[] straight,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                var f1 = straight.Where(o => o % 4 == 0).ToArray();
                var f2 = straight.Where(o => o % 4 == 1).ToArray();
                var f3 = straight.Where(o => o % 4 == 2).ToArray();
                var f4 = straight.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f1[0] % 4)
                    {
                        if (reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f1.Max() / 4 && reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                // different cards in hand
                if (f1.Length == 4) 
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f1[0] % 4)
                    {
                        if (reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (reserve[i] % 4 == f1[0] % 4 && reserve[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f1[0] % 4 && reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f1.Min() / 4 && reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        power = f1.Max() + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f2[0] % 4)
                    {
                        if (reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f2.Max() / 4 && reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)//different cards in hand
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f2[0] % 4)
                    {
                        if (reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (reserve[i] % 4 == f2[0] % 4 && reserve[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f2[0] % 4 && reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f2.Min() / 4 && reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        power = f2.Max() + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f3[0] % 4)
                    {
                        if (reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f3.Max() / 4 && reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                ////different cards in hand
                if (f3.Length == 4)
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f3[0] % 4)
                    {
                        if (reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (reserve[i] % 4 == f3[0] % 4 && reserve[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f3[0] % 4 && reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f3.Min() / 4 && reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        power = f3.Max() + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (reserve[i] % 4 == reserve[i + 1] % 4 && reserve[i] % 4 == f4[0] % 4)
                    {
                        if (reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[i] / 4 < f4.Max() / 4 && reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                //different cards in hand
                if (f4.Length == 4)
                {
                    if (reserve[i] % 4 != reserve[i + 1] % 4 && reserve[i] % 4 == f4[0] % 4)
                    {
                        if (reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[i + 1] % 4 != reserve[i] % 4 && reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (reserve[i] % 4 == f4[0] % 4 && reserve[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[i + 1] % 4 == f4[0] % 4 && reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[i] / 4 < f4.Min() / 4 && reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        power = f4.Max() + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                ////ace
                if (f1.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reserve[i] / 4 == 0 && reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[i + 1] / 4 == 0 && reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public static void rStraight(
            ref double current, 
            ref double power, 
            int[] straight,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                var op = straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            power = op.Max() + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 4 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            power = op[j + 4] + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 4 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = power, Current = 4 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        
        public static void rThreeOfAKind(
            ref double current, 
            ref double power, 
            int[] straight,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            power = 13 * 3 + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 3 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 3 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        public static void rTwoPair(
            ref double current, 
            ref double power,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (reserve[i] / 4 != reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }

                            if (tc - k >= 12)
                            {
                                if (reserve[i] / 4 == reserve[tc] / 4 && reserve[i + 1] / 4 == reserve[tc - k] / 4 ||
                                    reserve[i + 1] / 4 == reserve[tc] / 4 && reserve[i] / 4 == reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (reserve[i + 1] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (reserve[i] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 != 0 && reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (reserve[i] / 4) * 2 + (reserve[i + 1] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void rPairTwoPair(
            ref double current, 
            ref double power,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (reserve[tc] / 4 == reserve[tc - k] / 4)
                            {
                                if (reserve[tc] / 4 != reserve[i] / 4 && reserve[tc] / 4 != reserve[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (reserve[i] / 4) * 2 + 13 * 4 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (reserve[tc] / 4) * 2 + (reserve[i + 1] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (reserve[tc] / 4) * 2 + (reserve[i] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reserve[i] / 4 > reserve[i + 1] / 4)
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + reserve[i] / 4 + current * 100;
                                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = reserve[tc] / 4 + reserve[i] / 4 + current * 100;
                                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + reserve[i + 1] + current * 100;
                                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = reserve[tc] / 4 + reserve[i + 1] / 4 + current * 100;
                                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public static void rPairFromHand(
            ref double current, 
            ref double power, 
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (reserve[i] / 4 == reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reserve[i] / 4 == 0)
                        {
                            current = 1;
                            power = 13 * 4 + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 1 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            power = (reserve[i + 1] / 4) * 4 + current * 100;
                            winningHands.Add(new Type() { Power = power, Current = 1 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (reserve[i + 1] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[i + 1] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + reserve[i] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (reserve[i + 1] / 4) * 4 + reserve[i] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (reserve[i] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[i] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + reserve[i + 1] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (reserve[tc] / 4) * 4 + reserve[i + 1] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }
        
        public static void rHighCard(
            ref double current, 
            ref double power,
            List<Type> winningHands, 
            Type sorted, 
            ref double type, 
            ref int i, 
            int[] reserve)
        {
            if (current == -1)
            {
                if (reserve[i] / 4 > reserve[i + 1] / 4)
                {
                    current = -1;
                    power = reserve[i] / 4;
                    winningHands.Add(new Type() { Power = power, Current = -1 });
                    sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    power = reserve[i + 1] / 4;
                    winningHands.Add(new Type() { Power = power, Current = -1 });
                    sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (reserve[i] / 4 == 0 || reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    power = 13;
                    winningHands.Add(new Type() { Power = power, Current = -1 });
                    sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }
    }
}
