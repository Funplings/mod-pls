using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static string ReadFile(string file)
    {
        return File.ReadAllText(Application.dataPath + file);
    }

    public static string[] DirFiles(string dir)
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + dir);

        if (di.Exists)
        {
            FileInfo[] files = di.GetFiles();
            List<string> filePaths = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i];

                if(file.Extension != ".txt") continue;
                
                filePaths.Add(file.Name);
            }

            return filePaths.ToArray();
        }
        else
        {
            throw new System.ArgumentException(dir + " doesn't exist!");
        }
    }

    public static void setImageAlpha(Image image, float alpha)
    {
        var tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }

    public static float Map(float minIn, float maxIn, float minOut, float maxOut, float input)
    {
        return Mathf.Lerp(minOut, maxOut, Mathf.InverseLerp(minIn, maxIn, input));
    }

    public static float HoursLinear(float hour)
    {
        if(hour < 13)
            return hour + 24;

        return hour;
    }
}