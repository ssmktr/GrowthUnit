using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : Singleton<DataManager> {
    public static Dictionary<string, int> DicConfig = new Dictionary<string, int>();
    public static List<UnitDataBase.Data> ListUnitDataBase = new List<UnitDataBase.Data>();

    public static int GetConfigData(string _key)
    {
        if (DicConfig.ContainsKey(_key))
            return DicConfig[_key];

        Debug.LogWarning("Empty ConfigData");
        return 0;
    }

    public static UnitDataBase.Data GetUnitData(int _id)
    {
        for (int i = 0; i < ListUnitDataBase.Count; ++i)
        {
            if (_id == ListUnitDataBase[i].id)
                return ListUnitDataBase[i];
        }

        Debug.LogWarning("Not Found UnitData");
        return null;
    }
}
