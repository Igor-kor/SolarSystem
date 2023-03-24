using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Reflection;

namespace SolarSystemSimulation
{
    public class Mesh
    {
        private int vao;
        private int vbo;
        private int ebo;
        private int vertexCount;
        private int indexCount;
       // private Shader _shader;

        public void Create(float[] vertices, int[] indices)
        {
           /* _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
           */
            //_shader.SetInt("texture0", 0);
            //_shader.SetInt("texture1", 1);

            // Generate the VAO, VBO, and EBO
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out ebo);

            // Bind the VAO
            GL.BindVertexArray(vao);

            // Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            // Bind the EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.DynamicDraw);

            // Set the vertex attributes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Unbind the VAO
            GL.BindVertexArray(0);

            vertexCount = vertices.Length / 3;
            indexCount = indices.Length;
        }

        public void Render(/*Camera camera*/)
        {
            // Bind the VAO
            GL.BindVertexArray(vao);
            //_shader.SetMatrix4("model", vao);
            //_shader.SetMatrix4("view", camera.GetViewMatrix());
            //_shader.SetMatrix4("projection", camera.GetProjectionMatrix());
            // Draw the mesh
            GL.DrawElements(BeginMode.LineStrip, indexCount, DrawElementsType.UnsignedInt, 0);

            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
        }
        public static Mesh CreateSphere(float radius, int slices, int stacks)
        {
            Mesh mesh = new Mesh();
            List<float> vertices = new List<float>();
            List<int> indices = new List<int>();

            for (int j = 0; j <= stacks; j++)
            {
                float theta = j * MathF.PI / stacks; ;
                float sinTheta = MathF.Sin(theta);
                float cosTheta = MathF.Cos(theta);

                for (int i = 0; i <= slices; i++)
                {
                    float phi = i * 2 * MathF.PI / slices;
                    float sinPhi = MathF.Sin(phi);
                    float cosPhi = MathF.Cos(phi);

                    Vector3 vertex = new Vector3(cosPhi * sinTheta, cosTheta, sinPhi * sinTheta) * radius;
                    vertices.Add( vertex.X);
                    vertices.Add( vertex.Y);
                    vertices.Add( vertex.Z);
                }
            }

            for (int j = 0; j < stacks; j++)
            {
                for (int i = 0; i < slices; i++)
                {
                    int a = j * (slices + 1) + i;
                    int b = a + 1;
                    int c = (j + 1) * (slices + 1) + i;
                    int d = c + 1;

                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(d);

                    indices.Add(a);
                    indices.Add(d);
                    indices.Add(c);
                }
            }

            mesh.Create(vertices.ToArray(), indices.ToArray());

            return mesh;
        }
    }
}

