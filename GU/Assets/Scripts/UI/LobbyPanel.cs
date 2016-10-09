using UnityEngine;
using System.Collections;

public class LobbyPanel : UIBasePanel {

    public GameObject InvenBtn, ReadyBtn, CollectionBtn, GachaBtn;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(InvenBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("InvenPanel");
        };

        UIEventListener.Get(ReadyBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("ReadyPanel");
        };

        UIEventListener.Get(CollectionBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("CollectionPanel");
        };

        UIEventListener.Get(GachaBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("GachaPanel");
        };
    }

    public override void LateInit()
    {
        base.LateInit();
    }
}
