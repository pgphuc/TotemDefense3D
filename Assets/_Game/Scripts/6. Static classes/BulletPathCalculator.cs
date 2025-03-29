using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletPathCalculator
{
    private static float height = 10f;
    private static int steps = 20;
    
    public static List<Vector3> ParabolPath(Vector3 startPos, Vector3 targetPos)
    {
        List<Vector3> path = new List<Vector3>();

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps; // Tính t từ 0 → 1

            // Tính toán vị trí X, Z theo nội suy tuyến tính
            float x = Mathf.Lerp(startPos.x, targetPos.x, t);
            float z = Mathf.Lerp(startPos.z, targetPos.z, t);

            // Tính toán vị trí Y theo công thức Parabol
            float y = Mathf.Lerp(startPos.y, targetPos.y, t) + height * 4 * t * (1 - t);

            path.Add(new Vector3(x, y, z));
        }
        return path;
    }

    public static List<Vector3> StraightPath(Vector3 startPos, Vector3 targetPos)
    {
        List<Vector3> path = new List<Vector3>();
        for (int i = 0; i <= steps; i++)
        {
            float t  = (float)i / steps;
            Vector3 point = Vector3.Lerp(startPos, targetPos, t);
            path.Add(point);
        }
        return path;
    }
}
