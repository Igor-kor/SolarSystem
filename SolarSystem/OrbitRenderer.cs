using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static OpenTK.Graphics.OpenGL.GL;

namespace SolarSystem
{
    internal class OrbitRenderer
    {
        private Vector3[] orbitPoints;
        private int vbo;
        private int vao;
        private int ebo; // Индексный буфер
        private int[] indices;

        public OrbitRenderer(Vector3[] _orbitPoints, int[] _indices)
        {
            orbitPoints = _orbitPoints;
            indices = _indices;

            GL.GenVertexArrays(1, out vao);
            GL.GenBuffers(1, out vbo);
            GL.GenBuffers(1, out ebo);
            // Bind the VAO
            GL.BindVertexArray(vao);

            // Bind the VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, orbitPoints.Length * sizeof(float), orbitPoints, BufferUsageHint.DynamicDraw);

            // Bind the EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.DynamicDraw);

            // Set the vertex attributes
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        public void Render()
        {
            // Bind the VAO
            GL.BindVertexArray(vao);
            // Draw the mesh
            GL.DrawElements(BeginMode.Lines, indices.Length, DrawElementsType.UnsignedInt, 0);
            // Unbind the VAO
            GL.BindVertexArray(0);
        }
    }
}
