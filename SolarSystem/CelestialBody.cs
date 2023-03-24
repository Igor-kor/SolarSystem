using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;

namespace SolarSystemSimulation
{
    public class CelestialBody
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }
        private Mesh mesh;

        public CelestialBody(Vector3 position, float radius, Color color)
        {
            Position = position;
            Radius = radius;
            Color = color;
            mesh = Mesh.CreateSphere(Radius, 32, 32);
        }

        public void Draw()
        {
            GL.PushMatrix();

            GL.Translate(Position);

            GL.Color3(Color);
            //gluSphere(IntPtr.Zero, Radius, 32, 32);
            mesh.Render();

            GL.PopMatrix();
        }
    }
}

