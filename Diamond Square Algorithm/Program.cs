using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Diamond_Square_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get n from user.In this implementation we will be creating 2^n+1 sized maps.
            Console.Write("Please provide the size of the image.Note that the formula 2^n+1 will be used and you will be providing the n,so be careful.\n");
            Console.Write("Size:");
            int n = Convert.ToInt32(Console.ReadLine());


            //Max image size will be 2^13+1 = 8193 x 8193
            if (n > 13)
            {
                Console.Write("Size is not acceptable.Defaulted to 13.\n");
            }

            //We will have better results using odd sized maps.
            int size = (int)Math.Pow(2, n) + 1;

            //Create map
            int[,] map = new int[size, size];

            //Initialize the four corners of the map
            Random rng = new Random();

            map[0, 0] = rng.Next(0, 65);
            map[size - 1, 0] = rng.Next(0, 65);
            map[0, size - 1] = rng.Next(0, 65);
            map[size - 1, size - 1] = rng.Next(0, 65);

            //This is used to create variety
            int rand, lower = 1, upper = 4;

            int step = size - 1;
            int half;

            while (step > 1)
            {
                half = step / 2;

                //Diamond
                for (int y = half; y < size - 1; y += step)
                {
                    for (int x = half; x < size - 1; x += step)
                    {
                        //Get the average of the four corners
                        map[x, y] = (map[x - half, y + half] + map[x + half, y - half] + map[x + half, y + half] + map[x - half, y - half]) / 4 + rng.Next(1, 11);
                    }
                }

                //Square
                int xs;
                int up, down, left, right;
                bool even = true;
                for (int y = 0; y < size; y += half)
                {
                    xs = (even) ? half : 0;
                    for (int x = xs; x < size; x += step)
                    {
                        //Get the average of those edges that have a value
                        up = (y + half < size) ? map[x, y + half] : 0;
                        down = (y - half >= 0) ? map[x, y - half] : 0;
                        right = (x + half < size) ? map[x + half, y] : 0;
                        left = (x - half >= 0) ? map[x - half, y] : 0;


                        //If an edge does not have a value,reduce the neighbors by 1 as the tile only has 3 neighbors
                        int neighbors = 4;
                        if (up == 0 || down == 0 || right == 0 || left == 0)
                        {
                            neighbors = 3;
                        }
                        //Calculate
                        map[x, y] = (up + down + right + left) / neighbors + rng.Next(1, 11);

                    }
                    even = !even;
                }
                step /= 2;
                
                //Ugly code,please move along
                bool cont = true;               
                if (lower == upper + 1)
                {
                    lower -= 1;
                    upper += 1;
                    cont = false;
                }
                else if (cont == true)
                {
                    lower += 1;
                    upper -= 1;
                }
            }

            Bitmap bmp = new Bitmap(size, size);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int val = (int)(map[x, y] / 100.0 * 255);
                    //Sometimes due to the Random function this can go over 255 which is undesirable
                    if (val > 255)
                    {
                        val = 255;
                    }
                    //Paint the pixel
                    bmp.SetPixel(x, y, Color.FromArgb(255, val, val, val));
                }
            }
            //Save and start photo viewer to check out the image
            string dir = Directory.GetCurrentDirectory() + "\\map.png";
            bmp.Save(dir);
            Process.Start(dir);
        }
    }

}
