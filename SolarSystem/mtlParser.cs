using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem
{
    internal class mtlParser
    {
        string[] lines;
        string texturePath;
        public mtlParser(string filename)
        {
            Console.WriteLine("Load mtl "+ filename); 
            lines = File.ReadAllLines( filename);
            FileParse();
        }

        void FileParse()
        {
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            foreach (string line in lines)
            {
                if (line.StartsWith("map_Kd "))
                {
                    string[] elements = line.Split(' ');
                    texturePath = elements[1];
                }
             
            }
        }

        public string GettexturePath() 
        { 
            return texturePath;
        }
    }
}
