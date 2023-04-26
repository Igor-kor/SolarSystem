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
        private MeshSphere mesh = new MeshSphere();
        //Shader _shader;

        public CelestialBody(Vector3 position, float radius, Color color, float speed)
        {
            Position = position;
            Radius = radius;
            Color = color;
            mesh.CreateSphere(Radius, 32, 32, position, color, speed);
        }

        public void Draw()
        {
            GL.PushMatrix();

            //GL.Translate(Position);
            mesh.Render();
            GL.PopMatrix();
        }
    }
}

