using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

/*
 * Чтобы использовать этот буфер цветов в OpenGL, 
 * вы можете вызвать метод Bind() в начале каждого кадра 
 * и Unbind() в конце каждого кадра, чтобы привязать и отвязать буфер цветов соответственно. 
 * Чтобы изменить размеры буфера цветов, вызовите метод Resize() с новыми шириной и высотой.
 */

namespace SolarSystem
{
    internal class ColorBuffer
    {
        private int bufferHandle;
        private int width;
        private int height;

        public ColorBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            // Создаем буфер цветов
            GL.GenTextures(1, out bufferHandle);
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle);

            // Устанавливаем параметры текстуры
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        }

        public void Bind()
        {
            // Привязываем буфер цветов
            GL.BindTexture(TextureTarget.Texture2D, bufferHandle);
        }

        public void Unbind()
        {
            // Отвязываем буфер цветов
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Resize(int width, int height)
        {
            // Изменяем размеры буфера цветов
            this.width = width;
            this.height = height;

            GL.BindTexture(TextureTarget.Texture2D, bufferHandle);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }
    }
}

