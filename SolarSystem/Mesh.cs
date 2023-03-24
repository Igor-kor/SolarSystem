using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Reflection;

namespace SolarSystemSimulation
{
    public class Mesh
    {
        List<float> vertices = new List<float>();
        List<int> indices = new List<int>();
        private int vao;
        private int vbo;
        private int ebo;

        private int vertexCount;
        private int indexCount;
        Vector3 position;
        Color color;
        Vector4 colorVector;
        static Shader shaders;

        public Mesh()
        {
            if (shaders == null)
            {
                shaders = new Shader("Shaders/mypositiion.vert", "Shaders/color.frag");
                Mesh.shaders.Use();
            }
         
        }
        //public void Create(float[] vertices, int[] indices)
        //{
        //    // Generate the VAO, VBO, and EBO
        //    GL.GenVertexArrays(1, out vao);
        //    GL.GenBuffers(1, out vbo);
        //    GL.GenBuffers(1, out ebo);

        //    // Bind the VAO
        //    GL.BindVertexArray(vao);

        //    // Bind the VBO
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        //    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

        //    // Bind the EBO
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.DynamicDraw);

        //    // Set the vertex attributes
        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        //    GL.EnableVertexAttribArray(0);

        //    // Unbind the VAO
        //    GL.BindVertexArray(0);

        //    vertexCount = vertices.Length / 3;
        //    indexCount = indices.Length;
        //}

        public void Create(float[] vertices, int[] indices)
        {
            // Generate the VAO, VBO, and EBO
            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out ebo);
            // Bind the VAO
            GL.BindVertexArray(vao);

            // Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Bind the EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            // Set the vertex attributes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Set the position uniform
            int positionLocation = GL.GetAttribLocation(shaders.Handle,"Pposition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.Uniform3(positionLocation, position);

            // Set the color uniform
            int colorLocation = GL.GetAttribLocation(shaders.Handle, "Scolor");
            colorVector = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A/255f);
            GL.EnableVertexAttribArray(colorLocation);
            GL.Uniform4(colorLocation, colorVector);
            //GL.VertexAttribPointer(colorLocation, 4, VertexAttribPointerType.Float, false, 0, 0);
          
            // Unbind the VAO
            GL.BindVertexArray(0);

            vertexCount = vertices.Length / 3;
            indexCount = indices.Length;
        }

        public void Render()
        {
            // Bind the VAO
            GL.BindVertexArray(vao);
            shaders.SetVector3("Pposition", position);
            shaders.SetVector4("Scolor", colorVector);
            // Draw the mesh
            GL.DrawElements(BeginMode.Polygon, indexCount, DrawElementsType.UnsignedInt, 0);

            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
        }
        public void CreateSphere(float radius, int slices, int stacks, Vector3 _position, Color _color)
        {
            position = _position;
            color = _color;
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
                    vertices.Add(vertex.X);
                    vertices.Add(vertex.Y);
                    vertices.Add(vertex.Z);
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

            Create(vertices.ToArray(), indices.ToArray());

        }

        //public void setPosition(Vector3 position)
        //{
        //    /* for (int j = 0; j <= vertices.Count-1; j+=3)
        //     {
        //         for (int i = 0; i < 3; i++)
        //         {
        //             vertices[j+i]=vertices[j + i] + position.X;
        //             vertices[j+i]=vertices[j + i] + position.X;
        //             vertices[j+i]=vertices[j + i] + position.X;
        //         }
        //     }
        //     Create(vertices.ToArray(), indices.ToArray());*/
        //    GL.BindVertexArray(vao);

        //    GL.EnableVertexAttribArray(0);
        //    GL.VertexAttribFormat(0, 3, VertexAttribType.UnsignedByte, false,0);
        //    GL.VertexAttribBinding(0, 0);
        //    GL.EnableVertexAttribArray(1);
        //    GL.VertexAttribFormat(1, 3, VertexAttribType.UnsignedByte, false,3);
        //    GL.VertexAttribBinding(1, 0);
        //    GL.EnableVertexAttribArray(2);
        //    GL.VertexAttribFormat(2, 4, VertexAttribType.UnsignedByte, true,6);
        //    GL.VertexAttribBinding(2, 0);

        //    GL.BindVertexArray(0);
        //}
    }

    public struct SmallBlockVertex
    {
        public byte PositionX;
        public byte PositionY;
        public byte PositionZ;
        public byte ColorR;
        public byte ColorG;
        public byte ColorB;
        public byte TextureX;
        public byte TextureY;
    }
}

