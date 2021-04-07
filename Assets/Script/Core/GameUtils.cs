using System;
using System.Collections.Generic;
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
        Random random = new Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
}
