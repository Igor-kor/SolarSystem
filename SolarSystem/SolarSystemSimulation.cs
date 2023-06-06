using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SolarSystem;
using System;
using System.Drawing;
using static OpenTK.Graphics.OpenGL.GL;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ErrorCode = OpenTK.Graphics.OpenGL.ErrorCode;
using Sun = SolarSystem.Sun;

namespace SolarSystemSimulation
{
    public class SolarSystemSimulation : GameWindow
    {

        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private double _time;
        private Shader _shader;
        private Shader _shader2;
        private SkyBox skyBox;
        private Sun sun;

        private List<InterfaceObject> ObjectsList;
        public SolarSystemSimulation(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            ObjectsList = new List<InterfaceObject>();

            _shader = new Shader("../../../Shaders/texture.vert", "../../../Shaders/texture.frag");
            _shader2 = new Shader("../../../Shaders/orbit.vert", "../../../Shaders/orbit.frag");
            _shader.Use();
            LoadObj();
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color.Black);
         
            _camera = new Camera(new Vector3(0, 0, 10), Size.X / (float)Size.Y);
            CursorState = CursorState.Grabbed;
        }

        void LoadObj()
        {
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/mercury.obj").GetMesh(), 0.3871f*3f, 0.2056f, 87.97f, 0.330f, 47.87f, 0.03f, 58.64f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/venus.obj").GetMesh(), 0.7233f * 3f, 0.0068f, 224.7f, 4.87f, 35.02f, 177.36f, -243.02f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/earth.obj").GetMesh(), 1.0f * 3f, 0.0167f, 365.25f, 5.97f, 29.78f, 23.44f, 24.0f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/mars.obj").GetMesh(), 1.5237f * 3f, 0.0934f, 686.98f, 0.642f, 24.07f, 25.19f, 24.62f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/jupiter.obj").GetMesh(), 5.2026f * 3f, 0.0484f, 4332.82f, 1898.0f, 13.07f, 3.12f, 9.93f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/saturn.obj").GetMesh(), 9.5388f * 3f, 0.0542f, 10755.7f, 568.0f, 9.69f, 26.73f, 10.66f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/uranus.obj").GetMesh(), 19.1914f * 3f, 0.0472f, 30687.15f, 86.8f, 6.81f, 97.77f, -17.24f));
            ObjectsList.Add(new Planet(new ObjParser("../../../blender/neptune.obj").GetMesh(), 30.0611f * 3f, 0.0086f, 60190.03f, 102.0f, 5.43f, 28.32f, 16.11f));
            skyBox = new SkyBox(new ObjParser("../../../blender/skybox.obj").GetMesh());
            sun = new Sun(new ObjParser("../../../blender/sun.obj").GetMesh());
        }

        void UpdateObj()
        {
            foreach (var mesh in ObjectsList)
            {
                mesh.Update((float)_time*10);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _time += 1.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            var model = Matrix4.Identity;

            // Очищаем ошибки OpenGL перед отрисовкой
            GL.GetError();

            _shader.Use();
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _shader.SetMatrix4("model", model);
            skyBox.DrawMehs();
            sun.DrawMesh();
            foreach (var objectel in ObjectsList)
            {
                _shader.Use();
                _shader.SetMatrix4("view", _camera.GetViewMatrix());
                _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
                _shader.SetMatrix4("model", objectel.GetModelMatrix());
                objectel.DrawMesh();
                _shader2.Use();
                _shader2.SetMatrix4("view", _camera.GetViewMatrix());
                _shader2.SetMatrix4("projection", _camera.GetProjectionMatrix());
                _shader2.SetMatrix4("model", model);
                objectel.DrawOrbit();
            }

            // Проверяем наличие ошибок OpenGL после отрисовки
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL ошибка: {error}");
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            UpdateObj();

            if (!IsFocused) // check to see if the window is focused
            {
                return;
            }

            KeyboardState input = KeyboardState;

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            var mouse = MouseState;

            if (_firstMove) 
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
              
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }

        
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}

