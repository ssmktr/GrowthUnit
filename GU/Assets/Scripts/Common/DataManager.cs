using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : Singleton<DataManager> {
    public static Dictionary<string, int> DicConfig = new Dictionary<string, int>();

    public static Dictionary<int, UnitDataBase.Data> DicUnitDataBase = new Dictionary<int, UnitDataBase.Data>();
    public static Dictionary<int, UnitDataBase.ResourceData> DicUnitResourceData = new Dictionary<int, UnitDataBase.ResourceData>();

    public static Dictionary<int, NameDataBase.Data> DicNameDataBase = new Dictionary<int, NameDataBase.Data>();

    public static Dictionary<int, StageDataBase.Data> DicStageDataBase = new Dictionary<int, StageDataBase.Data>();
    
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
        if (DicUnitDataBase.ContainsKey(_id))
            return DicUnitDataBase[_id];

        Debug.LogWarning("Not Found UnitData");
        return null;
    }

    public static UnitDataBase.ResourceData GetUnitResourceData(int _id)
    {
        if (DicUnitResourceData.ContainsKey(_id))
            return DicUnitResourceData[_id];

        Debug.LogWarning("Not Found UnitResourceData");
        return null;
    }
    #endregion
    #region STAGEDATA
    public static StageDataBase.Data GetStageData(int _id)
    {
        if (DicStageDataBase.ContainsKey(_id))
            return DicStageDataBase[_id];

        Debug.LogWarning("Not Found StageData");
        return null;
    }
    #endregion

    #region NAMEDATA
    public static NameDataBase.Data GetNameData(int _id)
    {
        if (DicNameDataBase.ContainsKey(_id))
            return DicNameDataBase[_id];

        Debug.LogWarning("Not Found NameData");
        return null;
    }

    public static string GetName(int _id)
    {
        if (DicNameDataBase.ContainsKey(_id))
            return DicNameDataBase[_id].kor;

        Debug.LogWarning("Not Found Name String");
        return null;
    }
    #endregion
}
