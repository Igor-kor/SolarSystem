using OpenTK;
using System;
using System.Runtime.InteropServices;

namespace SolarSystemSimulation
{

    public class Program
    {
        // Uncomment the following line to resolve.
         static void Main() {
            using (SolarSystemSimulation solarSystemSimulation = new SolarSystemSimulation(1366,900,"Solar System"))
            {
                solarSystemSimulation.Run();
            }
        }
    }
}

