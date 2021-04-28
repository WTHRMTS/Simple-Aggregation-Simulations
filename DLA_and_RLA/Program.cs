﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DLA_and_RLA
{
    class Program
    {
        static bool GetNeighbours(int x, int y, int[,] Lattice)
        // Tests if the walker's current position is adjacent to any filled points. If it is it returns true. If not, false.
        {
            if (Lattice[x + 1, y] == 1)
            {
                return true;
            }
            if (Lattice[x - 1, y] == 1)
            {
                return true;
            }
            if (Lattice[x, y + 1] == 1)
            {
                return true;
            }
            if (Lattice[x, y - 1] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool GetNeighboursReact(int x, int y, int[,] Lattice)
        // Same as above, checks to see if neighbours are filled, if so returns true but only 
        // with a probabilty of 10%. This is "Reaction-Limited Aggregation". Tune probability p to taste.
        // This could be an input variable to the function, but would add yet another parameter to walk().
        // Maybe making all this OO could streamline this?
        {
            Random rnd = new Random();
            double p = rnd.NextDouble();
            if (Lattice[x + 1, y] == 1 && p < 0.1)
            {
                return true;
            }
            if (Lattice[x - 1, y] == 1 && p < 0.1)
            {
                return true;
            }
            if (Lattice[x, y + 1] == 1 && p < 0.1)
            {
                return true;
            }
            if (Lattice[x, y - 1] == 1 && p < 0.1)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        static void Walk(int x, int y, int n, int min, int max, bool neighbours, int[,] Lattice, int maxSteps)
        // The walk function. First sets number of steps to 0. Then checks that walker's initial position is 
        // more than x% of the lattice length from the edge, where x is set by min and max defined in main().
        // Then chooses a random direction and increments the appropriate coordinate.
        // Then increments the step counter (nSteps), checks if new coordinate has any filled neighbours by
        // calling getNeighbours(), and if this is true it sets the current lattice site to filled,
        // and terminates the walk.
        {

            var nSteps = 0;
            while (nSteps < maxSteps)
            {
                // Check to see if x,y within min,max
                if (x < min || x > max)
                {
                    break;
                }
                if (y < min || y > max)
                {
                    break;
                }
                // Choose direction and increment/decrement x or y
                Random rnd = new Random();
                int index = rnd.Next(4);
                if (index == 0)
                {
                    x--;
                }
                if (index == 1)
                {
                    x++;
                }
                if (index == 2)
                {
                    y--;
                }
                if (index == 3)
                {
                    y++;
                }
                // Increment step counter and check if walker is on site neighbouring cluster. Set to filled if it is.
                nSteps++;
                neighbours = GetNeighbours(x, y, Lattice);
                if (neighbours == true)
                {
                    Lattice[x, y] = 1;
                    break;
                }  
            }
         
        }

        static double GetNewboundary(int[,] Lattice, int n)
        // Computes the radius of the moving boundary from the lattice seed at x = n/2, y = n/2
        // by computing the maximum distance from the seed to the edge of the cluster and adding 0.3 times
        // the length of the cluster. Using a static boundary results in many walkers hitting
        // their maxSteps without finding the seed/cluster which increases simulation time
        // dramatically. This may need refinement.
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Lattice[i, j] == 1)
                    {
                        indexList.Add((int)(Math.Abs(i - n / 2)));
                        indexList.Add((int)(Math.Abs(j - n / 2)));
                    }
                }
            
            }
            
            var radius = (double)indexList.Max() + 0.2 * n;

            return radius;

        }


        static int[] InitWalker(int n, double radius)
        // Returns random x, y coordinates on a moving boundary used for walker's initial coordinates.
        {
            var coords = new int[2];
            Random rnd = new Random();
            var theta = rnd.NextDouble()* 2 * Math.PI;
            var x = radius * Math.Cos(theta);
            var y = radius * Math.Sin(theta);
            coords[0] = (int)(y) + n / 2; // switched as indexing is row column, which is y,x when plotted. Makes no difference on square lattice though.
            coords[1] = (int)(x) + n / 2;

            return coords;

        }
        static void Main(string[] args)
        {
            // Initialize lattice length n, number of walkers and maximum number of steps,
            // create n*n lattice and plant seed at the center.*/
            var n = 100;
            var nWalkers = 5000;
            var maxSteps = 1000;

            var Lattice = new int[n, n];

            Lattice[n/2, n/2] = 1;

            // Set Cluster limits, so it doesn't hit the edge of the Lattice.
            
            var min = (int)(0.1 * n);
            var max = (int)(0.9 * n);

            // Loop which for each of the nWalkers: 
            // 1. Sets neighbours variable to false, 
            // 2. Computes the new value of the moving boundary to drop walkers on. 
            // 3. Gets initial walker coordinates on this boundary and
            // 4. Calls the walk function using those coords.
            for (int i = 0; i < nWalkers; i++)
            {
                var neighbours = false;
                var radius = GetNewboundary(Lattice, n);
                var coords = InitWalker(n, radius);
                Walk(coords[1], coords[0], n, min, max, neighbours, Lattice, maxSteps);
             

            }
            // Write Lattice Array to a text file for Plotting.
            using var sw = new StreamWriter("Cluster.txt");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sw.Write(Lattice[i, j] + " ");
                }
                sw.Write("\n");
            }

            sw.Flush();
            sw.Close();
        }    
    }



        
}
