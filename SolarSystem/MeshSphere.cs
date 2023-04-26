using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Reflection;

namespace SolarSystemSimulation
{
    public class MeshSphere
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
        float speed;
        float radius;
        float orbitradius;
        Shader shaders;
        float iterationdegree = 0;

        public MeshSphere()
        {
            shaders = Shader.current;
          /*  shaders.Use();*/
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
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            // Bind the EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.DynamicDraw);

            // Set the vertex attributes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Set the position uniform
            int positionLocation = shaders.GetAttribLocation("position");
          
            GL.EnableVertexAttribArray(positionLocation);
            GL.Uniform3(positionLocation, position);
         
            int colorLocation = shaders.GetAttribLocation("Scolor");
            colorVector = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A/255f);
            
            GL.EnableVertexAttribArray(colorLocation);
            GL.Uniform4(colorLocation, colorVector);


            // Unbind the VAO
            GL.BindVertexArray(0);

            vertexCount = vertices.Length / 3;
            indexCount = indices.Length;
        }

        public void Render()
        {
            // shaders.Use();
            iterationdegree += speed;
            if (iterationdegree >= 360) iterationdegree = 0;
            position.X = 0 + (orbitradius * MathF.Cos(iterationdegree * MathF.PI / 180));
            position.Y = 0 + (orbitradius * MathF.Sin(iterationdegree * MathF.PI / 180));
            shaders.SetVector3("position", position );
            shaders.SetVector4("Scolor", colorVector);
            
            // Bind the VAO
            GL.BindVertexArray(vao);
            // Draw the mesh
            GL.DrawElements(BeginMode.TriangleStripAdjacency, indexCount, DrawElementsType.UnsignedInt, 0);
            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
        }
        public void CreateSphere(float _radius, int slices, int stacks, Vector3 _position, Color _color, float _speed)
        {
            speed = _speed;
            position = _position;
            color = _color;
            radius = _radius;
            orbitradius = _position.X;
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

    }
}

