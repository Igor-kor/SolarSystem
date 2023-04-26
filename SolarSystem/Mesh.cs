using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;
using SolarSystemSimulation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace SolarSystem
{
    internal class Mesh
    {
        // Массивы вершин, нормалей и текстурных координат
        List<float> vertices = new List<float>();
        List<float> normals = new List<float>();
        List<float> texCoords = new List<float>();

        // Массив индексов
        List<int> indices = new List<int>();

        int textureId = 0;
        string texture;

        private int vao;

        private int vbo;
        private int nbo;
        private int tbo;
        private int ibo;

        //  private int ebo;

        public Mesh(List<float> vertices, List<float> normals, List<float> texCoords, List<int> indices, string texture)
        {
            this.vertices = vertices;
            this.normals = normals;
            this.texCoords = texCoords;
            this.indices = indices;
            this.texture = texture;
            BindParseData();
            LoadTexture("../../../blender/" + texture);
            BindTexture();
        }

        void BindParseData()
        {
            GL.GenVertexArrays(1, out vao);
            // Bind the VAO
            GL.BindVertexArray(vao);

            // Создание VBO для вершинных данных
            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * 3 * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // Создание VBO для нормалей
            GL.GenBuffers(1, out nbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Count * sizeof(float), normals.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            // Создание VBO для текстурных координат
            GL.GenBuffers(1, out tbo);
            GL.BindBuffer(BufferTarget.TextureBuffer, tbo);
            GL.BufferData(BufferTarget.TextureBuffer, texCoords.Count * sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);
           // GL.BufferData(BufferTarget.TextureBuffer, (IntPtr)(sizeof(float) * 4), IntPtr.Zero, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
            //  GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            //  GL.EnableVertexAttribArray(2);


            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);

            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        public int LoadTexture(string filename)
        {
            Console.WriteLine("Load Texrure " + filename);
            GL.BindVertexArray(vao);
            using (var image = Image.Load<Rgba32>(filename))
            {
                var texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);

                var rowSpan = image.GetPixelRowSpan(0);
                var width = image.Width;
                var height = image.Height;

                var data = new byte[width * height * 4];

                for (var y = 0; y < height; y++)
                {
                    var pixelRow = System.Runtime.InteropServices.MemoryMarshal.Cast<Rgba32, byte>(rowSpan.Slice(y));
                    var destRow = data.AsSpan().Slice(y * width * 4, width * 4);
                    pixelRow.CopyTo(destRow);
                }

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                textureId = texture;
                return texture;
            }
               // Unbind the VAO
            GL.BindVertexArray(0);
        }


        public void BindTexture()
        {
            // Установка текстуры (если используется)
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }

        public void DrawMesh(Shader shader)
        {
            // Связывание VAO
            GL.BindVertexArray(vao);

            // Связывание буфера индексов
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            // Связывание буфера вершин с атрибутом позиции
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            int positionLocation = GL.GetAttribLocation(shader.Handle, "aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
            //   GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            //  GL.EnableVertexAttribArray(0);
            // Связывание буфера текстурных координат с атрибутом текстурных координат
         
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            int textureLocation = GL.GetUniformLocation(shader.Handle, "tex");
            GL.Uniform1(textureLocation, 0);


            // Рисование модели
            GL.DrawElements(BeginMode.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);

            // Отключение атрибутов и связываний
            GL.DisableVertexAttribArray(positionLocation);
           
            //  GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

        }
    }
}
