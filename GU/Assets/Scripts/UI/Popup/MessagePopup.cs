using UnityEngine;
using System.Collections;

public class MessagePopup : UIBasePanel {

    public GameObject OkBtn, CancelBtn;
    public UILabel ContentLbl, TitleLbl;

    System.Action OkCall, CancelCall = null;

    public override void Init()
    {
        base.Init();

        UIEventListener.Get(OkBtn).onClick = (sender) =>
        {
            if (OkCall != null)
                OkCall();

            Close();
        };

        UIEventListener.Get(CancelBtn).onClick = (sender) =>
        {
            if (CancelCall != null)
                CancelCall();

            Close();
        };
    }

    public override void LateInit()
    {
        base.LateInit();
    }

    public void SetData(string msg, System.Action _call1 = null, System.Action _call2 = null, string _btnName1 = "확인", string _btnName2 = "취소", string _title = "알림")
    {
        TitleLbl.text = _title;
        ContentLbl.text = msg;
        OkCall = _call1;
        CancelCall = _call2;
        OkBtn.transform.FindChild("name").GetComponent<UILabel>().text = _btnName1;
        CancelBtn.transform.FindChild("name").GetComponent<UILabel>().text = _btnName2;

        OkBtn.SetActive(_call1 != null);
        CancelBtn.SetActive(_call2 != null);
        if (_call1 != null && _call2 != null)
        {
            OkBtn.transform.localPosition = new Vector3(200, -120, 0);
            CancelBtn.transform.localPosition = new Vector3(-200, -120, 0);
        }
        else if (_call1 != null && _call2 == null)
        {
            OkBtn.transform.localPosition = new Vector3(0, -120, 0);
        }
    }
}
