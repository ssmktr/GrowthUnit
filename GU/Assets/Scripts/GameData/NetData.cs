
public class NetData {
    public class UnitData
    {
        public int uid;
        public int id;
        public int level;
        public int stringid;
        public int type;
        public int classtype;
        public float move_speed;
        public float hp;
        public int atk;
        public int def;
        public float cri;
        public float attackrange;

        public void Set(UnitDataBase.Data _data)
        {
            id = _data.id;
            stringid = _data.stringid;
            type = _data.type;
            classtype = _data.classtype;
            move_speed = _data.move_speed;
            hp = _data.hp;
            atk = _data.atk;
            def = _data.def;
            cri = _data.cri;
            attackrange = _data.attackrange;
        }
    };
}
