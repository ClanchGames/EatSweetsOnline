using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Runtime.InteropServices;

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
    /// �F�𔖂����郁�\�b�h
    /// </summary>
    /// <param name="image"></param>
    public static void FadeOut(Image image)
    {
        image.DOFade(0.1f, 0);
    }
    /// <summary>
    /// �F��߂����\�b�h
    /// </summary>
    /// <param name="image"></param>
    public static void FadeIn(Image image)
    {
        image.DOFade(1, 0);
    }
    public static Sequence UpDown(Transform transform)
    {
        Sequence seq;
        seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(-1f, 0.5f).SetEase(Ease.Linear).SetRelative());
        seq.Append(transform.DOLocalMoveY(1f, 0.5f).SetEase(Ease.Linear).SetRelative());
        seq.SetLoops(-1);
        return seq;
    }

    public static void KillSequence(Sequence seq)
    {
        seq.Kill();
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

    /// <summary>
    /// �w�肵��2�����z���1�����z��ɕϊ����܂��B
    /// </summary>
    public static T[] ToOneDimensional<T>(T[,] src)
    {
        int ymax = src.GetLength(0);
        int xmax = src.GetLength(1);
        int len = xmax * ymax;
        var dest = new T[len];

        for (int y = 0, i = 0; y < ymax; y++)
        {
            for (int x = 0; x < xmax; x++, i++)
            {
                dest[i] = src[y, x];
            }
        }
        return dest;
    }

    /// <summary>
    /// �g�ݍ��݌^�݂̂�Ώۂ�2�����z���1�����z��ɕϊ����܂��B
    /// </summary>
    public static T[] ToOneDimensionalPrimitives<T>(T[,] src)
    {
        int ymax = src.GetLength(0);
        int xmax = src.GetLength(1);
        int len = xmax * ymax;
        var dest = new T[len];

        var size = Marshal.SizeOf(typeof(T));
        Buffer.BlockCopy(src, 0, dest, 0, len * size);
        return dest;
    }

    /// <summary>
    /// �w�肵��2�����z���1�����z��ɕϊ����܂��B
    /// <para>T[height, width] �͈͂𒴂��镪�͐؂�̂āA�s�����Ă��镪��(T)�̏����l�ɂȂ�܂��B</para>
    /// </summary>
    public static T[,] ToTowDimensional<T>(T[] src, int width, int heigth)
    {
        var dest = new T[heigth, width];
        int len = width * heigth;
        len = src.Length < len ? src.Length : len;
        for (int y = 0, i = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                if (i >= len)
                {
                    return dest;
                }
                dest[y, x] = src[i];
            }
        }

        return dest;
    }

    /// <summary>
    ///  �g�ݍ��݌^�݂̂�Ώۂ�1�����z���2�����z��ɕϊ����܂��B
    /// <para>T[height, width] �͈͂𒴂��镪�͐؂�̂āA�s�����Ă��镪��(T)�̏����l�ɂȂ�܂��B</para>
    /// </summary>
    public static T[,] ToTowDimensionalPrimitives<T>(T[] src, int width, int heigth)
    {
        var dest = new T[heigth, width];
        int len = width * heigth;
        len = src.Length < len ? src.Length : len;

        var size = Marshal.SizeOf(typeof(T));
        Buffer.BlockCopy(src, 0, dest, 0, len * size);
        return dest;
    }


}



