using System;
using System.Drawing;

namespace Steganography
{
    class SteganographyHelper
    {
        public enum State
        {
            H_process,
            Z_padding
        };

        public static Bitmap msgHiding(string text, Bitmap b)
        {
            State st = State.H_process;
            int cIndex = 0;
            int cValue = 0;
            long px_Index = 0;
            int zeros = 0;
            int R = 0, G = 0, B = 0;
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    Color px = b.GetPixel(j, i);
                    R = px.R - px.R % 2;
                    G = px.G - px.G % 2;
                    B = px.B - px.B % 2;
                    for (int n = 0; n < 3; n++)
                    {
                        if (px_Index % 8 == 0)
                        {
                            if (st == State.Z_padding && zeros == 8)
                            {
                                if ((px_Index - 1) % 3 < 2)
                                {
                                    b.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return b;
                            }
                            if (cIndex >= text.Length)
                            {
                                st = State.Z_padding;
                            }
                            else
                            {
                                cValue = text[cIndex++];
                            }
                        }
                        switch (px_Index % 3)
                        {
                            case 0:
                                {
                                    if (st == State.H_process)
                                    {
                                        R += cValue % 2;
                                        cValue /= 2;
                                    }
                                } break;
                            case 1:
                                {
                                    if (st == State.H_process)
                                    {
                                        G += cValue % 2;

                                        cValue /= 2;
                                    }
                                } break;
                            case 2:
                                {
                                    if (st == State.H_process)
                                    {
                                        B += cValue % 2;

                                        cValue /= 2;
                                    }

                                    b.SetPixel(j, i, Color.FromArgb(R, G, B));
                                } break;
                        }

                        px_Index++;

                        if (st == State.Z_padding)
                        {
                            zeros++;
                        }
                    }
                }
            }

            return b;
        }

        public static string msgRetrieve(Bitmap b)
        {
            int color_Index = 0;
            int cValue = 0;
            string messageExt = String.Empty;
            for (int i = 0; i < b.Height; i++)
            {
                for (int j = 0; j < b.Width; j++)
                {
                    Color px = b.GetPixel(j, i);
                    for (int n = 0; n < 3; n++)
                    {
                        switch (color_Index % 3)
                        {
                            case 0:
                                {
                                    cValue = cValue * 2 + px.R % 2;
                                } break;
                            case 1:
                                {
                                    cValue = cValue * 2 + px.G % 2;
                                } break;
                            case 2:
                                {
                                    cValue = cValue * 2 + px.B % 2;
                                } break;
                        }

                        color_Index++;
                        if (color_Index % 8 == 0)
                        {
                            cValue = bitRvrse(cValue);
                            if (cValue == 0)
                            {
                                return messageExt;
                            }
                            char c = (char)cValue;
                            messageExt += c.ToString();
                        }
                    }
                }
            }

            return messageExt;
        }

        public static int bitRvrse(int j)
        {
            int ans = 0;

            for (int i = 0; i < 8; i++)
            {
                ans = ans * 2 + j % 2;

                j /= 2;
            }

            return ans;
        }
    }
}
