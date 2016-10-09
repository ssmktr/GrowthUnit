using UnityEngine;
using System.Collections;

public class UpInfoPanel : UIBasePanel {

    public UITexture MainBack;
    public GameObject EnergyBtn, GoldBtn, DiaBtn, HeartBtn, BackBtn;
    public UILabel EnergyValueLbl, GoldValueLbl, DiaValueLbl, HeartValueLbl;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            UIManager.Instance.Prev();
        };

        UIEventListener.Get(EnergyBtn).onClick = (sender) =>
        {
            Debug.Log("EnergyBtn");
        };

        UIEventListener.Get(GoldBtn).onClick = (sender) =>
        {
            Debug.Log("GoldBtn");
        };

        UIEventListener.Get(DiaBtn).onClick = (sender) =>
        {
            Debug.Log("DiaBtn");
        };

        UIEventListener.Get(HeartBtn).onClick = (sender) =>
        {
            Debug.Log("HeartBtn");
        };
    }

    public override void LateInit()
    {
        base.LateInit();

        BackBtn.SetActive(!(UIManager.GetFirstPanel() is LobbyPanel));

        AssetBundleLoad.Instance.AssetTextureLoad("DesertBackGround", (tex) => {
            if (tex != null)
                MainBack.mainTexture = tex;
        });

        SetValue();
    }

    public void SetValue()
    {
        EnergyValueLbl.text = string.Format("{0}/{1}", GameManager.Instance.Energy, GameManager.Instance.MaxEnergy);
        GoldValueLbl.text = GameManager.Instance.Gold.ToString("N0");
        DiaValueLbl.text = GameManager.Instance.Dia.ToString("N0");
        HeartValueLbl.text = GameManager.Instance.Heart.ToString("N0");
    }

    public void SetBackBtn(bool _bOn)
    {
        BackBtn.SetActive(_bOn);
    }
}
