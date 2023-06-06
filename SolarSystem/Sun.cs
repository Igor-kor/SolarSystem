using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem
{
    internal class Sun
    {
        public Mesh Mesh { get; set; }

        public Sun(Mesh mesh)
        {
            Mesh = mesh;
        }

        public void DrawMesh()
        {
            Mesh.DrawMesh();
        }
    }
}
