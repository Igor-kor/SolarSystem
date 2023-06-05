using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SolarSystem
{
    internal class OrbitRenderer
    {
        private Vector3[] _orbitPoints;
        private int _vbo;

        public OrbitRenderer(Vector3[] orbitPoints)
        {
            _orbitPoints = orbitPoints;

            // Создаем буфер вершин и загружаем в него точки орбиты
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _orbitPoints.Length * Vector3.SizeInBytes, _orbitPoints, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Render()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            // Рисуем линии между точками орбиты
            GL.DrawArrays(PrimitiveType.LineStrip, 0, _orbitPoints.Length);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}
