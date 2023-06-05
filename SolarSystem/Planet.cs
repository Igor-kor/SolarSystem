using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using SolarSystemSimulation;

namespace SolarSystem
{
    internal class Planet:InterfaceObject
    {
        public float SemiMajorAxis { get; set; }
        public float Eccentricity { get; set; }
        public float OrbitalPeriod { get; set; }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        private float currentAngle = 0.0f;
        public Mesh Mesh { get; set; }
        // Создание массива точек орбиты для объекта
        Vector3[] orbitPoints = new Vector3[360];
        OrbitRenderer orbitRenderer;

        private float gravitationalConstant = 6.67430e-11f; // Гравитационная постоянная
        public float Mass { get; set; }
        public float Velocity { get; set; }

        public float AxialTilt { get; set; }
        public float RotationPeriod { get; set; }

        public Planet(Mesh mesh, float semiMajorAxis, float eccentricity, float orbitalPeriod, float mass, float velocity, float axialTilt, float rotationPeriod) 
          
        {
            Mesh = mesh;
            SemiMajorAxis = semiMajorAxis;
            Eccentricity = eccentricity;
            OrbitalPeriod = orbitalPeriod;
            Mass = mass;
            Velocity = velocity;
            AxialTilt = axialTilt;
            RotationPeriod = rotationPeriod;


            // Calculate initial position and rotation
            Position = CalculatePosition(0.0f);
            Rotation = Quaternion.Identity;

            for (int i = 0; i < 360; i++)
            {
                float angle = MathHelper.DegreesToRadians(i);
                float x = SemiMajorAxis * (float)Math.Cos(angle);
                float y = SemiMajorAxis * (float)Math.Sin(angle);
                orbitPoints[i] = new Vector3(x, 0.0f, y);
            }
            // Создание экземпляра OrbitRenderer
            orbitRenderer = new OrbitRenderer(orbitPoints);
        }
        public void Update(float elapsedTime)
        {
            // Calculate new position and rotation based on elapsed time
            float meanAnomaly = (2.0f * (float)Math.PI / OrbitalPeriod) * elapsedTime;
            float eccentricAnomaly = CalculateEccentricAnomaly(meanAnomaly);
            currentAngle = CalculateTrueAnomaly(eccentricAnomaly);
            Position = CalculatePosition(currentAngle);
            Rotation = CalculateRotation(elapsedTime);
        }

        private float CalculateEccentricAnomaly(float meanAnomaly)
        {
            // Расчет эксцентрической аномалии по средней аномалии и эксцентриситету

            float eccentricAnomaly = meanAnomaly; // Начальное значение эксцентрической аномалии
            float delta = 0.01f; // Погрешность вычислений

            while (true)
            {
                float nextEccentricAnomaly = eccentricAnomaly - ((eccentricAnomaly - Eccentricity * (float)Math.Sin(eccentricAnomaly) - meanAnomaly) / (1.0f - Eccentricity * (float)Math.Cos(eccentricAnomaly)));

                if (Math.Abs(nextEccentricAnomaly - eccentricAnomaly) < delta)
                    break;

                eccentricAnomaly = nextEccentricAnomaly;
            }

            return eccentricAnomaly;
        }

        private float CalculateTrueAnomaly(float eccentricAnomaly)
        {
            // Расчет истинной аномалии по эксцентрической аномалии

            float trueAnomaly = 2.0f * (float)Math.Atan2(Math.Sqrt(1.0f + Eccentricity) * (float)Math.Sin(eccentricAnomaly / 2.0f), Math.Sqrt(1.0f - Eccentricity) * (float)Math.Cos(eccentricAnomaly / 2.0f));

            return trueAnomaly;
        }

        private Vector3 CalculatePosition(float trueAnomaly)
        {
            // Расчет новой позиции объекта в трехмерном пространстве по истинной аномалии

            float x = SemiMajorAxis * (float)Math.Cos(trueAnomaly);
            float y = SemiMajorAxis * (float)Math.Sin(trueAnomaly);

            Vector3 newPosition = new Vector3(x, 0.0f, y);

            return newPosition;
        }

        private Quaternion CalculateRotation(float elapsedTime)
        {
            // Calculate axial tilt and rotation based on elapsed time
            float axialTilt = 23.5f; // Earth's axial tilt (in degrees)
            float rotationPeriod = 24.0f; // Earth's rotation period (in hours)

            // Calculate axial tilt and rotation based on elapsed time
            float rotationAngle = (360.0f / rotationPeriod) * elapsedTime;
            float tiltAngle = MathHelper.DegreesToRadians(axialTilt);

            // Create rotation quaternions for axial tilt and rotation
            Quaternion tiltRotation = Quaternion.FromAxisAngle(Vector3.UnitX, tiltAngle);
            Quaternion axisRotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(rotationAngle));

            // Combine the rotations
            Quaternion rotation = tiltRotation * axisRotation;

            return rotation;
        }

        public void DrawMesh()
        {
            Mesh.DrawMesh();
        }
        public void DrawOrbit()
        {
            orbitRenderer.Render();
        }


        public Matrix4 GetModelMatrix()
        {
            Matrix4 translationMatrix = Matrix4.CreateTranslation(Position);
            Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(Rotation);
            return translationMatrix * rotationMatrix;
        }

        public Vector3 CalculateGravityForce(Vector3 sunPosition)
        {
            Vector3 direction = sunPosition - Position;
            float distanceToSun = (Position - sunPosition).Length;
            direction.Normalize();
            float forceMagnitude = gravitationalConstant * (Mass * 1.989e30f) / (distanceToSun * distanceToSun);
            Vector3 gravityForce = direction * forceMagnitude;
            return gravityForce;
        }
    }
}
