using OpenTK.Mathematics;
using System.Drawing;

namespace SolarSystemSimulation
{
    public class SolarSystem
    {
        private List<CelestialBody> bodies;
        public SolarSystem()
        {
            bodies = new List<CelestialBody>();
            // добавление планет и звезд в систему
          //  bodies.Add(new CelestialBody(new Vector3(0, 0, 0), 5, Color.Yellow,0.0f)); // солнце
          //  bodies.Add(new CelestialBody(new Vector3(10 * 1, 0, 0), 1, Color.Blue, 1.0f / (0.365f * 1.0f))); // земля
          //  bodies.Add(new CelestialBody(new Vector3(10 * 1.52f, 0, 0), 0.5f, Color.Red, 1.0f / (0.3686f * 1.0f))); // марс
          //  bodies.Add(new CelestialBody(new Vector3(10 * 5.2f, 0, 0), 2, Color.White, 1.0f / (0.365f * 11.8f))); // юпитер
          //  bodies.Add(new CelestialBody(new Vector3(10 * 9.54f, 0, 0), 1.5f, Color.Orange, 1.0f / (0.365f * 29.46f))); // Сатурн
         //   bodies.Add(new CelestialBody(new Vector3(10 * 19.22f, 0, 0), 1, Color.Green, 1.0f / (0.365f * 84.02f))); // Уран
         //   bodies.Add(new CelestialBody(new Vector3(10 * 30.06f, 0, 0), 1, Color.SkyBlue, 1.0f / (0.365f * 165.78f))); // Нептун
         //   bodies.Add(new CelestialBody(new Vector3(10 * 39.2f, 0, 0), 0.5f, Color.LightGray, 1.0f / (0.365f * 248.09f))); // Плутон*/
        }

        public void Draw()
        {
            foreach (var body in bodies)
            {
                body.Draw();
            }
        }
    }
}

