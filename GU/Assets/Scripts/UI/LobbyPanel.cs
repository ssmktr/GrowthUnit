using UnityEngine;
using System.Collections;

public class LobbyPanel : UIBasePanel {

    public GameObject InvenBtn, ReadyBtn;

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
    }

    public override void LateInit()
    {
        base.LateInit();

    }
}
