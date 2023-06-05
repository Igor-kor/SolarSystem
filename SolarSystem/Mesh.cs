using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;
using SolarSystemSimulation;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Mathematics;

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
            Console.WriteLine(vertices.Count);
            Console.WriteLine(normals.Count);
            Console.WriteLine(texCoords.Count);
            Console.WriteLine(indices.Count);
            // Создание VBO для вершинных данных
            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
         

            // Создание VBO для нормалей
            GL.GenBuffers(1, out nbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Count * sizeof(float), normals.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);


            // Создание VBO для текстурных координат
            GL.GenBuffers(1, out tbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

            // Освобождение привязок VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Создание EBO для индексов
            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);

            // Освобождение привязок
            GL.BindVertexArray(0);
        }

        public int LoadTexture(string filePath)
        {

            textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            using (Image image = Image.FromFile(filePath))
            {
                Bitmap bitmap = new Bitmap(image);
                BitmapData data = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                );

                GL.TexImage2D(
                    TextureTarget.Texture2D,
                     0,
                     PixelInternalFormat.Rgba,
                     data.Width,
                     data.Height,
                     0,
                     OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                     PixelType.UnsignedByte,
                     data.Scan0
                 );

                bitmap.UnlockBits(data);
                bitmap.Dispose();
            }

            // Установка параметров фильтрации и повторения текстуры
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            return textureId;
        }


        public void BindTexture()
        {
            // Установка текстуры (если используется)
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.ActiveTexture(TextureUnit.Texture0);
        }

        public void DrawMesh()
        {
           
            // Связывание VAO
            GL.BindVertexArray(vao);
            BindTexture();
            // Рисование модели
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
          
            GL.BindVertexArray(0);
        }


      
        public void Update(float elapsedTime)
        {
           //
        }

     
    }
}
