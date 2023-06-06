using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace SolarSystem
{
    internal class SkyBox
    {
        public Mesh Mesh { get; set; }

        public SkyBox(Mesh mesh)
        {
            Mesh = mesh;
        }

        public void DrawMehs()
        {
            Mesh.DrawMesh();
        }
    }
}
