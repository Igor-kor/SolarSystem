using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace SolarSystemSimulation
{
    public class SolarSystemSimulation : GameWindow
    {
        private SolarSystem solarSystem;
        float speed = 1.5f;
        MatrixMode matrixMode = new MatrixMode();
        Vector3 position = new Vector3(0.0f, 0.0f, 3.0f);
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f),
             new Vector3(0.0f, 0.0f, 0.0f),
             new Vector3(0.0f, 1.0f, 0.0f));

        public SolarSystemSimulation(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            solarSystem = new SolarSystem();
            OnLoad();
        }

        protected void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.Aqua);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            view = Matrix4.LookAt(position, position + front, up);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.ClearColor(Color.Aqua);
            GL.MatrixMode(MatrixMode.Color);
            GL.LoadIdentity();
            GL.Translate(0, 0, -50);

            solarSystem.Draw();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            KeyboardState input = KeyboardState;

            //...

            if (input.IsKeyDown(Keys.W))
            {
                position += front * speed; //Forward 
            }

            if (input.IsKeyDown(Keys.S))
            {
                position -= front * speed; //Backwards
            }

            if (input.IsKeyDown(Keys.A))
            {
                position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed; //Left
            }

            if (input.IsKeyDown(Keys.D))
            {
                position += Vector3.Normalize(Vector3.Cross(front, up)) * speed; //Right
            }

            if (input.IsKeyDown(Keys.Space))
            {
                position += up * speed; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                position -= up * speed; //Down
            }
        }
    }
}

