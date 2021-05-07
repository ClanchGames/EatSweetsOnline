using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using System;

public static class MyMethod
{
    public static GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 色を薄くするメソッド
    /// </summary>
    /// <param name="image"></param>
    public static void FadeOut(Image image)
    {
        image.DOFade(0.1f, 0);
    }
    /// <summary>
    /// 色を戻すメソッド
    /// </summary>
    /// <param name="image"></param>
    public static void FadeIn(Image image)
    {
        image.DOFade(1, 0);
    }

    public static GameObject FindObject(string objname)
    {
        GameObject obj = GameObject.Find(objname);
        return obj;
    }
    public static T GetComponentInObject<T>(string objectName = "")
    {
        T c = default(T);
        if (objectName != "")
        {
            GameObject gameObject = GameObject.Find(objectName);
            if (gameObject == null)
            {
                Debug.LogError(objectName + " is not found");
            }
            else
            {
                c = gameObject.GetComponent<T>();
                if (c == null)
                {
                    Debug.LogError(nameof(T) + " is not found");
                }
            }
        }

        return c;
    }
    public static T MyGetComponent<T>(GameObject obj)
    {
        T c = default;

        c = obj.GetComponent<T>();
        if (c == null)
        {
            obj.AddComponent(typeof(T));
            c = obj.GetComponent<T>();
        }
        return c;
    }
    public static int[,] ReverseMap(int[,] map)
    {
        //int[,] reversemap = map;

        for (int i = 0; i <= map.GetLength(0) - 1; i++)
        {
            for (int j = 0; j <= map.GetLength(1) - 1; j++)
            {
                int row = map.GetLength(0) - 1;
                int column = map.GetLength(1) - 1;

                if (!(i > (row / 2)))
                {


                    int s1 = map[i, j];
                    int s2 = map[column - i, j];
                    //SetTile(mymain.UnitPrefabs[UnitPosition[i, j]], i, j);
                    map[i, j] = s2;
                    map[column - i, j] = s1;
                }
            }
        }

        return map;
    }








    public static string SetTag(string tagname, List<string> taglist)
    {
        if (taglist.Contains(tagname))
        {
            return tagname;
        }
        else
        {
            return null;
        }
    }

}
