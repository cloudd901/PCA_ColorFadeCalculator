//==========================================================
//Created by Derrick Ducote - admin@pcaffinity.com -
//==========================================================

namespace PCAFFINITY
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class ColorFadeCalculator
    {
        // accuracy 1-255 (how many steps from one color to the next)

        public ColorFadeCalculator()
        {
        }

        public ColorFadeCalculator(Color fromColor, Color toColor)
            : this(fromColor, toColor, 60, 1, false)
        {
        }

        public ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60)
            : this(fromColor, toColor, targetSteps, 1, false)
        {
        }

        public ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60, bool transparentFading = false)
            : this(fromColor, toColor, targetSteps, 1, transparentFading)
        {
        }

        public ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60, int accuracy = 1)
            : this(fromColor, toColor, targetSteps, accuracy, false)
        {
        }

        private ColorFadeCalculator(Color fromColor, Color toColor, int targetSteps = 60, int accuracy = 1, bool transparentFading = false)
        {
            Steps = new List<Color>();
            DoWork(fromColor, toColor, targetSteps, accuracy, transparentFading);
        }

        public List<Color> Steps { get; private set; }

        public static Color GetMidColor(Color color1, Color color2)
        {
            int r = (color1.R + color2.R) / 2;
            int g = (color1.G + color2.G) / 2;
            int b = (color1.B + color2.B) / 2;

            return Color.FromArgb(r, g, b);
        }

        public void DoWork(Color fromColor, Color toColor, int targetSteps, int accuracy, bool transparentFading)
        {
            //FromARGB fixes some color issues with WinForms
            if (fromColor == default || fromColor.IsEmpty || fromColor == Color.Transparent)
            {
                fromColor = Color.FromArgb(0, 0, 0, 0);
            }

            if (toColor == default || toColor.IsEmpty || toColor == Color.Transparent)
            {
                toColor = Color.FromArgb(0, 0, 0, 0);
            }

            if (accuracy > 255 || accuracy < 1)
            {
                throw new ArgumentOutOfRangeException("Accuracy must be between 1 and 255.");
            }

            Color labelStepColor = fromColor;

            if (transparentFading)
            {
                do
                {
                    labelStepColor = FadeColor(labelStepColor, toColor, 1, true);
                    Steps.Add(labelStepColor);
                } while (labelStepColor.A != toColor.A);
            }
            else
            {
                do
                {
                    labelStepColor = FadeColor(labelStepColor, toColor, accuracy, false);
                    Steps.Add(labelStepColor);
                } while (labelStepColor.R != toColor.R || labelStepColor.G != toColor.G || labelStepColor.B != toColor.B);
            }

            if (targetSteps > 1)
            {
                if (Steps.Count > targetSteps)
                {
                    int div = Steps.Count / targetSteps;
                    List<Color> tempList = new List<Color>();

                    for (int i = 0; i < Steps.Count; i += div)
                    {
                        tempList.Add(Steps[i]);
                    }

                    Steps = new List<Color>(tempList);

                    if (Steps.Count != targetSteps)
                    {
                        int cnt = Steps.Count - targetSteps;
                        div = Steps.Count / cnt;
                        tempList.Clear();

                        for (int i = 1; i < Steps.Count; i += div)
                        {
                            Steps.RemoveAt(i);
                        }
                    }
                }
                else if (Steps.Count < targetSteps)
                {
                    int cnt = targetSteps - Steps.Count;
                    int div = Steps.Count / cnt;
                    int place = 1;
                    for (int i = 1; i < cnt; i++)
                    {
                        if (place == Steps.Count)
                        {
                            place--;
                        }

                        Steps.Insert(place, Steps[place]);
                        place += div;
                    }
                }
            }

            if (Steps.Count >= targetSteps)
            {
                Steps.RemoveAt(Steps.Count - 1);
            }

            Steps.Add(toColor);

            // If extra steps are required then they are randomly added
            // Usually not required
            int temp = targetSteps;
            while (Steps.Count < targetSteps)
            {
                Random r = new Random();
                int i = r.Next(1, Steps.Count - 1);
                if (i == temp || i - 1 == temp)
                {
                    continue;
                }

                temp = i;
                Steps.Insert(i, Steps[i]);
            }

            while (Steps.Count > targetSteps)
            {
                Random r = new Random();
                int i = r.Next(1, Steps.Count - 1);
                if (i == temp || i - 1 == temp)
                {
                    continue;
                }

                temp = i;
                Steps.RemoveAt(i);
            }

            //Ensure start and end colors are correct
            Steps[0] = fromColor;
            Steps[Steps.Count - 1] = toColor;
        }

        private static Color FadeColor(Color startColor, Color targetColor, int steps, bool fadeAlpha)
        {
            int A = startColor.A;
            int R = startColor.R;
            int G = startColor.G;
            int B = startColor.B;
            int AX = targetColor.A;
            int RX = targetColor.R;
            int GX = targetColor.G;
            int BX = targetColor.B;

            if (R != RX)
            {
                if (R < RX && R + steps < RX)
                {
                    R += steps;
                }
                else if (R > RX && R - steps > RX)
                {
                    R -= steps;
                }
                else
                {
                    R = RX;
                }
            }

            if (G != GX)
            {
                if (G < GX && G + steps < GX)
                {
                    G += steps;
                }
                else if (G > GX && G - steps > GX)
                {
                    G -= steps;
                }
                else
                {
                    G = GX;
                }
            }

            if (B != BX)
            {
                if (B < BX && B + steps < BX)
                {
                    B += steps;
                }
                else if (B > BX && B - steps > BX)
                {
                    B -= steps;
                }
                else
                {
                    B = BX;
                }
            }

            if (fadeAlpha && A != AX)
            {
                if (A < AX && A + steps < AX)
                {
                    A += steps;
                }
                else if (A > AX && A - steps > AX)
                {
                    A -= steps;
                }
                else
                {
                    A = AX;
                }
            }

            return Color.FromArgb(A, R, G, B);
        }
    }
}