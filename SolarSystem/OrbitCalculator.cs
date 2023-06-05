using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem
{
    public class OrbitCalculator
    {
        // Рассчитать параметры орбиты на основе массы и расстояния до Солнца
        public static void CalculateOrbitParameters(float planetMass, float distanceToSun, out float semiMajorAxis, out float eccentricity, out float orbitalPeriod)
        {
            // Константы гравитационной постоянной и массы Солнца
            const float G = 6.67430e-11f; // Гравитационная постоянная (м^3 * кг^-1 * с^-2)
            const float solarMass = 1.989e30f; // Масса Солнца (кг)

            // Расчет большой полуоси
            semiMajorAxis = (float)Math.Sqrt(distanceToSun * distanceToSun * ((G * solarMass) / (4.0f * (float)Math.PI * (float)Math.PI)));

            // Расчет эксцентриситета
            eccentricity = (float)Math.Sqrt(1.0f - (4.0f * (float)Math.PI * (float)Math.PI * planetMass * semiMajorAxis) / (G * solarMass * solarMass * distanceToSun));

            // Расчет орбитального периода
            orbitalPeriod = (float)Math.Sqrt((4.0f * (float)Math.PI * (float)Math.PI * semiMajorAxis * semiMajorAxis * semiMajorAxis) / (G * (solarMass + planetMass)));
        }
    }
}
