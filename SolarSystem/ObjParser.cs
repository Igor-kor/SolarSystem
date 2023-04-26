using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Globalization;

namespace SolarSystem
{
    internal class ObjParser
    {
        // Загрузка файла .obj
        string[] lines;
        string texture;

        // Массивы вершин, нормалей и текстурных координат
        List<float> vertices = new List<float>();
        List<float> normals = new List<float>();
        List<float> texCoords = new List<float>();

        // Массив индексов
        List<int> indices = new List<int>();

        public ObjParser(string filename)
        {
            Console.WriteLine("Load Obj " + filename);
            lines = File.ReadAllLines(filename);
            FileParse();
            // BindParseData();
        }

        public Mesh GetMech()
        {
            Mesh mesh = new Mesh(vertices, normals, texCoords, indices, texture);

            return mesh;
        }

        void FileParse()
        {
            var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            foreach (string line in lines)
            {
                if (line.StartsWith("v "))
                {
                    // Разбиение строки на элементы и добавление в массив вершин
                    string[] elements = line.Split(' ');
                    vertices.Add(float.Parse(elements[1], culture));
                    vertices.Add(float.Parse(elements[2], culture));
                    vertices.Add(float.Parse(elements[3], culture));
                }
                else if (line.StartsWith("vn "))
                {
                    // Разбиение строки на элементы и добавление в массив нормалей
                    string[] elements = line.Split(' ');
                    normals.Add(float.Parse(elements[1], culture));
                    normals.Add(float.Parse(elements[2], culture));
                    normals.Add(float.Parse(elements[3], culture));
                }
                else if (line.StartsWith("vt "))
                {
                    // Разбиение строки на элементы и добавление в массив текстурных координат
                    string[] elements = line.Split(' ');
                    texCoords.Add(float.Parse(elements[1], culture));
                    texCoords.Add(float.Parse(elements[2], culture));
                }
                else if (line.StartsWith("f "))
                {
                    // Разбиение строки на элементы и добавление в массив индексов
                    string[] elements = line.Split(' ');
                  
                    for (int i = 1; i < elements.Length; i++)
                    {
                        string[] faceElements = elements[i].Split('/');
                        for (var k = 1; k <= 3; k++)
                        {
                            var vertexData = elements[k].Split('/');
                            indices.Add(int.Parse(vertexData[0]) - 1);
                        }
                    }
                } else if (line.StartsWith("mtllib "))
                {
                    // Разбиение строки на элементы и добавление в массив индексов
                    string[] elements = line.Split(' ');
                    mtlParser mtlParser = new mtlParser("../../../blender/" + elements[1]);
                    texture = mtlParser.GettexturePath();
                }
            }
        }
    }
}
