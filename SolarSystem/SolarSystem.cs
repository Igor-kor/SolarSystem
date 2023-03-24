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
            bodies.Add(new CelestialBody(new Vector3(0, 0, 0), 5, Color.Yellow)); // солнце
            bodies.Add(new CelestialBody(new Vector3(10, 0, 0), 1, Color.Blue)); // земля
            bodies.Add(new CelestialBody(new Vector3(20, 0, 0), 0.5f, Color.Red)); // марс
            /*bodies.Add(new CelestialBody(new Vector3(-15, 0, 0), 2, Color.White)); // юпитер
            bodies.Add(new CelestialBody(new Vector3(-25, 0, 0), 1.5f, Color.Orange)); // Сатурн
            bodies.Add(new CelestialBody(new Vector3(-35, 0, 0), 1, Color.Green)); // Уран
            bodies.Add(new CelestialBody(new Vector3(-45, 0, 0), 1, Color.SkyBlue)); // Нептун
            bodies.Add(new CelestialBody(new Vector3(-55, 0, 0), 0.5f, Color.LightGray)); // Плутон*/
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

