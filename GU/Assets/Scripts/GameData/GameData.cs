using UnityEngine;
using System.Collections.Generic;

public class UnitDataBase {

    public class Data
    {
        public int id = 0;
        public string name = "";
        public int type = 0;
        public int classtype = 0;
        public float move_speed = 0f;
        public float hp = 0f;
        public int atk = 0;
        public int def = 0;
        public float cri = 0f;
        public float cardsize = 0f;
        public float attackrange = 0f;
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
