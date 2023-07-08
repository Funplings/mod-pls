using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

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
}