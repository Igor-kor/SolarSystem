using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK.Graphics.OpenGL;
using static OpenTK.Graphics.OpenGL.GL;
using SolarSystemSimulation;

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

        private int vao;

        private int vbo;
        private int nbo;
        private int tbo;
        private int ibo;

        //  private int ebo;

        public Mesh(List<float> vertices, List<float> normals, List<float> texCoords, List<int> indices)
        {
            this.vertices = vertices;
            this.normals = normals;
            this.texCoords = texCoords;
            this.indices = indices;
            BindParseData();
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
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * sizeof(float), texCoords.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);

            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        void BindTexture()
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
            /* int positionLocation = GL.GetAttribLocation(shader.Handle, "aPosition");
             GL.EnableVertexAttribArray(positionLocation);
             GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 0, 0);*/
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
            // Связывание буфера текстурных координат с атрибутом текстурных координат
            //   GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            /*  GL.EnableVertexAttribArray(textureCoordLocation);
              GL.VertexAttribPointer(textureCoordLocation, 2, VertexAttribPointerType.Float, false, 0, 0);*/
            // Console.WriteLine(indices.Count);
            // Рисование модели
            // GL.DrawElements(BeginMode.TriangleStripAdjacency, indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.DrawElements(BeginMode.Lines, indices.Count, DrawElementsType.UnsignedInt, 0);

            // Отключение атрибутов и связываний
            /* GL.DisableVertexAttribArray(positionLocation);
             GL.DisableVertexAttribArray(textureCoordLocation);*/
            //  GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

        }
    }
}
