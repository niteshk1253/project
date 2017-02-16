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
			// initially, hide the characters in the image
            State st = State.H_process;
			
			// to hold the character index which is hidden
            int cIndex = 0;
			
			// to hold the character value which is converted to integer
            int cValue = 0;
			
			// to hold the index of the currently processed color element R/G/B
			long px_Index = 0;
            
			// to hold the number of trailing zeros when process is finishing
			int zeros = 0;
			
			// to hold pixel elements
            int R = 0, G = 0, B = 0;
			
			// passing through rows
            for (int i = 0; i < b.Height; i++)
            {
				// passing through each row
                for (int j = 0; j < b.Width; j++)
                {
					//to hold pixel which is currently being processed
                    Color px = b.GetPixel(j, i);
					
					//to clear LSB from each pixel elements
                    R = px.R - px.R % 2;
                    G = px.G - px.G % 2;
                    B = px.B - px.B % 2;
					
					// for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
						// check if new 8 bits has been processed
                        if (px_Index % 8 == 0)
                        {
							// check if the whole process has finished
                            // we can say that it's finished when 8 zeros are added
                            if (st == State.Z_padding && zeros == 8)
                            {
								// apply the last pixel on the image
                                // even if only a part of its elements have been affected
                                if ((px_Index - 1) % 3 < 2)
                                {
                                    b.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
								
								// return the bitmap with the text hidden in
                                return b;
                            }
							
							// check if all characters has been hidden
                            if (cIndex >= text.Length)
                            {
                                // start adding zeros to mark the end of the text
								st = State.Z_padding;
                            }
                            else
                            {
								// move to the next character and process again
                                cValue = text[cIndex++];
                            }
                        }
						
						// check which pixel element has the turn to hide a bit in its LSB
                        switch (px_Index % 3)
                        {
                            case 0:
                                {
                                    if (st == State.H_process)
                                    {
									   /* the rightmost bit in the character will be (charValue % 2)
										  to put this value instead of the LSB of the pixel element
									      just add it to it
									      recall that the LSB of the pixel element had been cleared
									      before this operation
									   */
                                        R += cValue % 2;
										
										/*removes the added rightmost bit of the character
                                          such that next time we can reach the next one*/
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
							// increment the value of zeros until it is 8
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
			
			// holds the text that will be extracted from the image
            string messageExt = String.Empty;
			
			// passing through rows
            for (int i = 0; i < b.Height; i++)
            {
				// passing through each row
                for (int j = 0; j < b.Width; j++)
                {
                    Color px = b.GetPixel(j, i);
					
					// for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        switch (color_Index % 3)
                        {
                            case 0:
                                {
									/*get the LSB from the pixel element (will be pixel.R % 2)
                                      then add one bit to the right of the current character
                                      this can be done by (charValue = charValue * 2)
                                      replace the added bit (which value is by default 0) with
                                      the LSB of the pixel element, simply by addition*/
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
						
						// if 8 bits has been added, then add the current character to the result text
                        if (color_Index % 8 == 0)
                        {
							// reverse? of course, since each time the process happens on the right (for simplicity)
                            cValue = bitRvrse(cValue);
							
							// can only be 0 if it is the stop character (the 8 zeros)
                            if (cValue == 0)
                            {
                                return messageExt;
                            }
							
							// convert the character value from int to char
                            char c = (char)cValue;
							
							// add the current character to the result text
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
