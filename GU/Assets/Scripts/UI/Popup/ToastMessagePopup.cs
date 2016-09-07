using UnityEngine;
using System.Collections;

public class ToastMessagePopup : UIBasePanel {

    public UILabel ContentLbl;

    public override void Init()
    {
        base.Init();
    }

    public override void LateInit()
    {
        base.LateInit();
    }

    public void SetData(string msg)
    {
        ContentLbl.text = msg;

        Invoke("Close", 2);
    }
}
