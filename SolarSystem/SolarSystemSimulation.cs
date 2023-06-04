﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SolarSystem;
using System;
using System.Drawing;
using static OpenTK.Graphics.OpenGL.GL;

namespace SolarSystemSimulation
{
    public class SolarSystemSimulation : GameWindow
    {
        private SolarSystem solarSystem;

        // We need an instance of the new camera class so it can manage the view and projection matrix code.
        // We also need a boolean set to true to detect whether or not the mouse has been moved for the first time.
        // Finally, we add the last position of the mouse so we can calculate the mouse offset easily.
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private double _time;
        private Shader _shader;

        private List<Mesh> _meshes;
        public SolarSystemSimulation(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            _meshes = new List<Mesh>();
            LoadObj();
            GL.Enable(EnableCap.DepthTest);

          /*  GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.TextureGenS);
            GL.Enable(EnableCap.TextureGenT);*/


            GL.ClearColor(System.Drawing.Color.Black);
            // We initialize the camera so that it is 3 units back from where the rectangle is.
            // We also give it the proper aspect ratio.
            //_camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            _camera = new Camera(new Vector3(0, 0, 10), Size.X / (float)Size.Y);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;

            

            _shader = new Shader("../../../Shaders/texture.vert", "../../../Shaders/texture.frag");
            _shader.Use();
            
            //***********************************************************************
            // Set the position uniform
            int positionLocation = _shader.GetAttribLocation("position");
            int textureSamplerLocation = _shader.GetAttribLocation("textureSampler");
            GL.EnableVertexAttribArray(positionLocation);
            GL.Uniform3(positionLocation, new Vector3(0,0,0));
            GL.Uniform1(textureSamplerLocation, 0);
            //  int colorLocation = _shader.GetAttribLocation("Scolor");
            //   Vector4 colorVector = new Vector4(1,1,1,1);

            //   GL.EnableVertexAttribArray(colorLocation);
            //    GL.Uniform4(colorLocation, colorVector);

            solarSystem = new SolarSystem();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vertexLocation);
            //*************************************************************************

            /*var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);  */
            

        }

        void LoadObj()
        {
         /*   Mesh Sun = new ObjParser("../../../blender/sun.obj").GetMech(0.0f, 0.0f, 0.0f);
            _meshes.Add(Sun);   */
            Mesh mercury = new ObjParser("../../../blender/sun.obj").GetMech(0.3871f, 0.2056f, 87.97f);
            _meshes.Add(mercury);
            Mesh venus = new ObjParser("../../../blender/sun.obj").GetMech(0.7233f, 0.0068f, 224.7f); // Венера
            _meshes.Add(venus);
            Mesh earth = new ObjParser("../../../blender/sun.obj").GetMech(1.0f, 0.0167f, 365.25f);
            _meshes.Add(earth);
            Mesh mars = new ObjParser("../../../blender/sun.obj").GetMech(1.5237f, 0.0934f, 686.98f); // Марс
            _meshes.Add(mars);
            Mesh jupiter = new ObjParser("../../../blender/sun.obj").GetMech(5.2026f, 0.0484f, 4332.82f); // Юпитер
            _meshes.Add(jupiter);
            Mesh saturn = new ObjParser("../../../blender/sun.obj").GetMech(9.5388f, 0.0542f, 10755.7f); // Сатурн
            _meshes.Add(saturn);
            Mesh uranus = new ObjParser("../../../blender/sun.obj").GetMech(19.1914f, 0.0472f, 30687.15f); // Уран
            _meshes.Add(uranus);
            Mesh neptune = new ObjParser("../../../blender/sun.obj").GetMech(30.0611f, 0.0086f, 60190.03f); // Нептун
            _meshes.Add(neptune);

            // _meshes.Add(new ObjParser("../../../blender/test.obj").GetMech());
        }

        void UpdateObj()
        {
            foreach (var mesh in _meshes)
            {
                mesh.Update((float)_time* 10);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            var model = Matrix4.Identity;
            _shader.Use();
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            solarSystem.Draw();
            foreach (var mesh in _meshes)
            {
                _shader.SetVector3("position", mesh.Position);
                //_shader.SetVector4("Scolor", new Vector4(1,1,1,1));
                mesh.DrawMesh(_shader);
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

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}

