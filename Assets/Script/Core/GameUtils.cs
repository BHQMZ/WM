
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 一些游戏通用方法
/// </summary>
public class GameUtils
{
    /// <summary>
    /// 产生一个List随机排序后的新List
    /// </summary>
    /// <param name="ListT">要排序的List</param>
    /// <returns></returns>
    public static List<T> RandomSortList<T>(List<T> ListT)
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }

    /// <summary>
    /// 绘制圆环
    /// </summary>
    /// <param name="center">圆心</param>
    /// <param name="radius">半径</param>
    /// <param name="step">步长,步长越小，轨迹点越密集(0.001:在起点和终点之间画1/0.001=1000个点)</param>
    /// <param name="CSYSX">坐标系下的X(0:x,1:y,2:z)</param>
    /// <param name="CSYSY">坐标系下的Y(0:x,1:y,2:z)</param>
    /// <returns></returns>
    public static Vector3[] DrawCircle(Vector3 center, float radius, float step, int CSYSX = 0, int CSYSY = 1)
    {
        List<Vector3> vectors = new List<Vector3>();

        float a = center[CSYSX];
        float b = center[CSYSY];

        float t = 0F;
        do
        {
            float angle = 360 * t;

            float radian = (angle / 180) * Mathf.PI;
            float x = a + radius * Mathf.Cos(radian);
            float y = b + radius * Mathf.Sin(radian);

            t += step;

            Vector3 vector = new Vector3(center.x, center.y, center.z);
            vector[CSYSX] = x;
            vector[CSYSY] = y;

            Debug.Log(vector);

            vectors.Add(vector);
        }
        while (t <= 1);

        return vectors.ToArray();
    }

    /// <summary>
    /// 绘制n阶贝塞尔曲线路径
    /// </summary>
    /// <param name="points">输入点</param>
    /// <param name="count">点数(n+1)</param>
    /// <param name="step">步长,步长越小，轨迹点越密集(0.001:在起点和终点之间画1/0.001=1000个点)</param>
    /// <returns></returns>
    public static Vector3[] DrawBezierCurves(Vector3[] points, float step)
    {
        int count = points.Length;
        List<Vector3> bezier_curves_points = new List<Vector3>();
        float t = 0F;
        do
        {
            Vector3 temp_point = BezierInterpolationFunc(t, points, count);    // 计算插值点
            t += step;
            bezier_curves_points.Add(temp_point);
        }
        while (t <= 1 && count > 1);    // 一个点的情况直接跳出.
        return bezier_curves_points.ToArray();  // 曲线轨迹上的所有坐标点
    }
    /// <summary>
    /// n阶贝塞尔曲线插值计算函数
    /// 根据起点，n个控制点，终点 计算贝塞尔曲线插值
    /// </summary>
    /// <param name="t">当前插值位置0~1 ，0为起点，1为终点</param>
    /// <param name="points">起点，n-1个控制点，终点</param>
    /// <param name="count">n+1个点</param>
    /// <returns></returns>
    private static Vector3 BezierInterpolationFunc(float t, Vector3[] points, int count)
    {
        if (points.Length < 1)  // 一个点都没有
            throw new ArgumentOutOfRangeException();

        if (count == 1)
            return points[0];
        else
        {
            Vector3[] tmp_points = new Vector3[count];
            for (int i = 1; i < count; i++)
            {
                tmp_points[i - 1] = points[i - 1] * t + points[i] * (1 - t);
            }
            return BezierInterpolationFunc(t, tmp_points, count - 1);
        }

    }
}
