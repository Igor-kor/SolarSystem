using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SolarSystemSimulation;

namespace SolarSystem
{
    internal interface InterfaceObject
    {
        public void Update(float elapsedTime);
        public void DrawMesh();
        public Vector3 Position { get; set; }
        public Matrix4 GetModelMatrix();
        public void DrawOrbit();
    }
}
