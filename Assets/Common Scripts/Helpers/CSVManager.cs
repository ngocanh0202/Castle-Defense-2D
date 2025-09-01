using System;
using System.Collections.Generic;
using Common2D;
using UnityEngine;

public class CSVManager
{
    public static List<T> LoadDatasFromCSV<T>(string path, Func<string[], T> OnLoadItem)
    {
        TextAsset textAssetItemdata = Resources.Load<TextAsset>("CSV/"+path);
        string[] lines = textAssetItemdata.text.Split('\n');
        int linesLength = lines.Length;
        List<T> datas = new List<T>();

        for (int i = 1; i < linesLength; i++)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrEmpty(line))
                continue;

            string[] lineDetails = line.Split(",");
            if (lineDetails.Length == 0)
                continue;
            datas.Add(OnLoadItem.Invoke(lineDetails));
        }
        
        return datas;
    }
}
