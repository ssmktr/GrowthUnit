using UnityEngine;
using System.Collections.Generic;

public class UnitDataBase {

    public class Data
    {
        public int id = 0;
        public string name = "";
        public int stringid = 0;
        public int type = 0;
        public int classtype = 0;
        public float move_speed = 0f;
        public float hp = 0f;
        public int atk = 0;
        public int def = 0;
        public float cri = 0f;
        public float attackrange = 0f;
        public float cardsize = 0f;
        public float battlesize = 0f;

        public void Set(Data _data)
        {
            id = _data.id;
            name = _data.name;
            stringid = _data.stringid;
            type = _data.type;
            classtype = _data.classtype;
            move_speed = _data.move_speed;
            hp = _data.hp;
            atk = _data.atk;
            def = _data.def;
            cri = _data.cri;
            attackrange = _data.attackrange;
            cardsize = _data.cardsize;
            battlesize = _data.battlesize;
        }

    };

    public class SlotData
    {
        public int id = 0;
        public int uid = 0;
        public int exp = 0;
        public int lv = 0;
        public int grade = 0;
        public int type = 0;
        public int classtype = 0;

        public void CreateSlotData(Data _data)
        {
            id = _data.id;
            uid = 1;
            lv = 1;
            exp = 0;
            grade = 1;
            type = _data.type;
            classtype = _data.classtype;
        }

        public void CreateSlotData(NetData.UnitData _data)
        {
            id = _data.id;
            uid = _data.uid;
            lv = _data.level;
            exp = _data.exp;
            grade = _data.grade;
            type = _data.type;
            classtype = _data.classtype;
        }
    };

    public class UnitData : SlotData
    {
        public float move_speed = 0f;
        public float hp = 0f;
        public int atk = 0;
        public int def = 0;
        public float cri = 0f;
        public float attackrange = 0f;

        public void CreateUnitData(Data _data)
        {
            CreateSlotData(_data);
            move_speed = _data.move_speed;
            hp = _data.hp;
            atk = _data.atk;
            def = _data.def;
            cri = _data.cri;
            attackrange = _data.attackrange;
        }
    };

    public class ResourceData
    {
        public int id = 0;
        public int type = 0;
        public string assetbundlename = "";
    };
};

public class StageDataBase
{
    public class Data
    {
        public int id = 0;
        public int accountexp = 0;
        public int unitexp = 0;
        public int rewardgold = 0;
        public List<int> ListEnemyId = new List<int>();
        public int enemycount = 0;
        public List<int> ListBossId = new List<int>();
        public int bosscount = 0;
    };
};

public class NameDataBase
{
    public class Data
    {
        public int id;
        public string kor;
    };
};



public enum UnitSortType
{
    Get = 0,
    Level,
    Grade,
    Type,
}
