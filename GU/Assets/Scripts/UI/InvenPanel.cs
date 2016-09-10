using UnityEngine;
using System.Collections;

public class InvenPanel : UIBasePanel {
    GameObject ModelObj;

    public GameObject BackBtn, CreateBtn, IdleBtn, DeadBtn, WalkBtn, RunBtn, AttackBtn, SkillBtn;
    public GameObject ModelRoot;

    int SelectUnitId = 0;

    int idx = 0;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("LobbyPanel");
        };

        UIEventListener.Get(CreateBtn).onClick = (sender) =>
        {
            //CreateModel(DataManager.ListUnitDataBase[Random.Range(0, DataManager.ListUnitDataBase.Count)]);
            CreateModel(DataManager.ListUnitDataBase[idx]);
            idx++;
        };

        UIEventListener.Get(IdleBtn).onClick = (sender) => 
        {
            if (ModelObj != null)
                ModelObj.GetComponent<UnitBaseCtrl>().SetAnim(UnitBaseCtrl.AnimType.Idle);
        };

        UIEventListener.Get(DeadBtn).onClick = (sender) =>
        {
            if (ModelObj != null)
                ModelObj.GetComponent<UnitBaseCtrl>().SetAnim(UnitBaseCtrl.AnimType.Dead);
        };

        UIEventListener.Get(WalkBtn).onClick = (sender) =>
        {
            if (ModelObj != null)
                ModelObj.GetComponent<UnitBaseCtrl>().SetAnim(UnitBaseCtrl.AnimType.Walk);
        };

        UIEventListener.Get(RunBtn).onClick = (sender) =>
        {
            if (ModelObj != null)
                ModelObj.GetComponent<UnitBaseCtrl>().SetAnim(UnitBaseCtrl.AnimType.Run);
        };

        UIEventListener.Get(AttackBtn).onClick = (sender) =>
        {
            if (ModelObj != null)
                ModelObj.GetComponent<UnitBaseCtrl>().SetAnim(UnitBaseCtrl.AnimType.Attack);
        };

        UIEventListener.Get(SkillBtn).onClick = (sender) =>
        {
            if (ModelObj != null)
                ModelObj.GetComponent<UnitBaseCtrl>().SetAnim(UnitBaseCtrl.AnimType.Skill);
        };
    }

    public override void LateInit()
    {
        base.LateInit();

        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }
    }

    void CreateModel(UnitDataBase.Data _UnitData)
    {
        if (ModelObj != null)
        {
            Destroy(ModelObj);
            ModelObj = null;
        }

        if (ModelObj == null)
        {
            AssetBundleLoad.Instance.AssetUnitLoad(_UnitData.name, (go) => {
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(-100, 100, 100) * _UnitData.cardsize;

                ModelObj = go;

            }, ModelRoot);
        }
    }
}
