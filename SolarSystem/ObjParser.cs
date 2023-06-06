using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Globalization;
using OpenTK.Mathematics;

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

        List<int> vertexIndices = new List<int>();
        List<int> texCoordIndices = new List<int>();
        List<int> normalIndices = new List<int>();

        List<float> newVertices = new List<float>();
        List<float> newNormals = new List<float>();
        List<float> newTexCoords = new List<float>();
        List<int> newIndices = new List<int>();

        public ObjParser(string filename)
        {
            Console.WriteLine("Load Obj " + filename);
            lines = File.ReadAllLines(filename);
            FileParse();
        }

        public Mesh GetMesh()
        {
            for (int i = 0; i < vertexIndices.Count; i++)
            {
                int vertexIndex = vertexIndices[i];
                int texCoordIndex = texCoordIndices[i];
                int normalIndex = normalIndices[i];

                // Получение вершины, нормали и текстурной координаты по индексам
                float vertexX = vertices[(vertexIndex ) * 3];
                float vertexY = vertices[(vertexIndex ) * 3 + 1];
                float vertexZ = vertices[(vertexIndex ) * 3 + 2];

                float normalX = normals[(normalIndex ) * 3];
                float normalY = normals[(normalIndex ) * 3 + 1];
                float normalZ = normals[(normalIndex ) * 3 + 2];

                float texCoordU = texCoords[(texCoordIndex ) * 2];
                float texCoordV = texCoords[(texCoordIndex ) * 2 + 1];

                // Добавление вершины, нормали и текстурной координаты в новые списки
                newVertices.Add(vertexX);
                newVertices.Add(vertexY);
                newVertices.Add(vertexZ);

                newNormals.Add(normalX);
                newNormals.Add(normalY);
                newNormals.Add(normalZ);

                newTexCoords.Add(texCoordU);
                newTexCoords.Add(texCoordV);

                int newIndex = newVertices.Count / 3 - 1;

                // Добавление индекса новой вершины
                newIndices.Add(newIndex);
            }
            Mesh mesh = new Mesh(newVertices, normals, newTexCoords, newIndices, texture);

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
                    float x = float.Parse(elements[1], culture);
                    float y = float.Parse(elements[2], culture);
                    texCoords.Add(x);
                    texCoords.Add(y);
                }
                else if (line.StartsWith("f "))
                {
                    // Разбиение строки на элементы и добавление в массив индексов
                    string[] elements = line.Split(' ');
                  
                    for (int i = 1; i < elements.Length; i++)
                    {
                        var vertexData = elements[i].Split('/');
                        vertexIndices.Add(int.Parse(vertexData[0]) - 1); // Индекс вершины
                        texCoordIndices.Add(int.Parse(vertexData[1]) - 1); // Индекс текстурной координаты
                        normalIndices.Add(int.Parse(vertexData[2]) - 1); // Индекс нормали
                    }
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
