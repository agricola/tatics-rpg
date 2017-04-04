using System;
using System.Collections.Generic;
using System.IO;

// T is your map type and U is your tile type
public class MapReader<T, U>
{
    public static T ReadMapFile(
        string fileName,
        Func<List<List<U>>, T> listToMap,
        Dictionary<char, U> tileDictionary)
    {
        string line;
        List<List<U>> rows = new List<List<U>>();
        StreamReader file = new StreamReader(fileName);
        while((line = file.ReadLine()) != null)
        {
            List<U> row = new List<U>();
            foreach (char c in line)
            {
                row.Add(tileDictionary[c]);
            }
            rows.Add(row);
        }
        return listToMap(rows);
    }
    
}
