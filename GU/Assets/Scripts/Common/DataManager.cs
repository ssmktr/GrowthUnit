﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : Singleton<DataManager> {
    public static Dictionary<string, int> DicConfig = new Dictionary<string, int>();
    public static List<UnitDataBase.Data> ListUnitDataBase = new List<UnitDataBase.Data>();
    public static List<StageDataBase.Data> ListStageDataBase = new List<StageDataBase.Data>();

    
    public static int GetConfigData(string _key)
    {
        if (DicConfig.ContainsKey(_key))
            return DicConfig[_key];

        Debug.LogWarning("Empty ConfigData");
        return 0;
    }

    #region UNITDATA
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
    #endregion
    #region STAGEDATA
    public static StageDataBase.Data GetStageData(int _id)
    {
        for (int i = 0; i < ListStageDataBase.Count; ++i)
        {
            if (_id == ListStageDataBase[i].id)
                return ListStageDataBase[i];
        }

        Debug.LogWarning("Not Found StageData");
        return null;
    }
    #endregion
}
