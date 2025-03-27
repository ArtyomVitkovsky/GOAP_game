using UnityEngine;

namespace Game.Car.Installers
{
    public static class DebugExtension
    {
        public static void DrawDebugSphere(Vector3 position, float radius, Color color)
        {            
            int segments = 12; // Количество сегментов (чем больше, тем круглее)
            float angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                float angleA = Mathf.Deg2Rad * (i * angleStep);
                float angleB = Mathf.Deg2Rad * ((i + 1) * angleStep);

                // Ось XZ (вид сверху)
                Vector3 pointA = position + new Vector3(Mathf.Cos(angleA), 0, Mathf.Sin(angleA)) * radius;
                Vector3 pointB = position + new Vector3(Mathf.Cos(angleB), 0, Mathf.Sin(angleB)) * radius;
                Debug.DrawLine(pointA, pointB, color);

                // Ось XY (вид сбоку)
                Vector3 pointC = position + new Vector3(Mathf.Cos(angleA), Mathf.Sin(angleA), 0) * radius;
                Vector3 pointD = position + new Vector3(Mathf.Cos(angleB), Mathf.Sin(angleB), 0) * radius;
                Debug.DrawLine(pointC, pointD, color);

                // Ось ZY (еще один боковой вид)
                Vector3 pointE = position + new Vector3(0, Mathf.Sin(angleA), Mathf.Cos(angleA)) * radius;
                Vector3 pointF = position + new Vector3(0, Mathf.Sin(angleB), Mathf.Cos(angleB)) * radius;
                Debug.DrawLine(pointE, pointF, color);
            }
        }
    }
}