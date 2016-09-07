using UnityEngine;
using System.Collections;

public class InvenPanel : UIBasePanel {

    public GameObject BackBtn;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(BackBtn).onClick = (sender) =>
        {
            Hide();
            UIManager.OpenUI("LobbyPanel");
        };
    }

    public override void LateInit()
    {
        base.LateInit();
    }
}
