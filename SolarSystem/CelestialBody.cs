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
        private Mesh mesh = new Mesh();
        //Shader _shader;

        public CelestialBody(Vector3 position, float radius, Color color)
        {
            Position = position;
            Radius = radius;
            Color = color;
            mesh.CreateSphere(Radius, 32, 32, position, color);
          /*  _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        */
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

