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
        List<float> texCoordstemp = new List<float>();
        List<float> texCoords = new List<float>();

        // Массив индексов
        List<int> indices = new List<int>();

        List<int> vertexIndices = new List<int>();
        List<int> texCoordIndices = new List<int>();
        List<int> normalIndices = new List<int>();

        public ObjParser(string filename)
        {
            Console.WriteLine("Load Obj " + filename);
            lines = File.ReadAllLines(filename);
            FileParse();
        }

        public Mesh GetMech( float semiMajorAxis, float eccentricity, float orbitalPeriod)
        {
            Mesh mesh = new Mesh(vertices, normals, texCoords, indices, texture, semiMajorAxis, eccentricity, orbitalPeriod);

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
                 //   texCoordstemp.Add(float.Parse(elements[1], culture));
                 //   texCoordstemp.Add(float.Parse(elements[2], culture));

                    texCoords.Add(float.Parse(elements[1], culture));
                    texCoords.Add(float.Parse(elements[2], culture));
                }
                else if (line.StartsWith("f "))
                {
                    // Разбиение строки на элементы и добавление в массив индексов
                    string[] elements = line.Split(' ');
                  
                    for (int i = 1; i < elements.Length; i++)
                    {
                        var vertexData = elements[i].Split('/');
                        vertexIndices.Add(int.Parse(vertexData[0]) - 1); // Индекс вершины
                       // texCoordIndices.Add(int.Parse(vertexData[1]) - 1); // Индекс текстурной координаты
                    //    texCoords.Add(texCoordstemp.ElementAt(int.Parse(vertexData[1]) - 1));
                   //     texCoords.Add(texCoordstemp.ElementAt(int.Parse(vertexData[1]) ));
                        normalIndices.Add(int.Parse(vertexData[2]) - 1); // Индекс нормали
                    }
                    // Создание списка индексов (faces)
                   /* for (int i = 0; i < vertexIndices.Count; i++)
                    {
                        indices.Add(vertexIndices[i]);
                        indices.Add(texCoordIndices[i]);
                        indices.Add(normalIndices[i]);
                    }*/
                } else if (line.StartsWith("mtllib "))
                {
                    // Разбиение строки на элементы и добавление в массив индексов
                    string[] elements = line.Split(' ');
                    mtlParser mtlParser = new mtlParser("../../../blender/" + elements[1]);
                    texture = mtlParser.GettexturePath();
                }
            }

            indices = vertexIndices;
        }
    }
}
