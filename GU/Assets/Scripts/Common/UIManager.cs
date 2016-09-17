using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager> {
    static List<UIBasePanel> ListPanel;

    static GameObject UIRoot, SystemRoot;

    public static UIBasePanel CurPanel;

    public UIManager()
    {
        CurPanel = null;
        ListPanel = new List<UIBasePanel>();
        
    }

    public void SetManager(GameObject _UIRoot = null, GameObject _SystemRoot = null)
    {
        UIRoot = _UIRoot;
        SystemRoot = _SystemRoot;
    }

    public static GameObject OpenUI(string _ui, params object[] _parameters)
    {
        GameObject Panel = GetUI(_ui);
        if (Panel == null)
        {
            Panel = Instantiate(Resources.Load("UI/" + _ui)) as GameObject;
            Panel.name = _ui;
            if (UIRoot != null)
                Panel.transform.parent = UIRoot.transform;
            Panel.transform.localPosition = Vector3.zero;
            Panel.transform.localScale = Vector3.one;
            Panel.GetComponent<UIBasePanel>().Init();

            if (Panel.GetComponent<UIBasePanel>().eUIType != UIBasePanel.UIType.Ignore)
                ListPanel.Add(Panel.GetComponent<UIBasePanel>());
        }

        if (Panel.GetComponent<UIBasePanel>().eUIType == UIBasePanel.UIType.Ignore)
        {
            Panel.GetComponent<UIBasePanel>().parameters = _parameters;
            Panel.GetComponent<UIBasePanel>().LateInit();
        }
        else
        {
            CurPanel = Panel.GetComponent<UIBasePanel>();
            CurPanel.parameters = _parameters;
            CurPanel.LateInit();
            OpenEvent();
        }

        return Panel;
    }

    public static GameObject GetUI(string _ui)
    {
        for (int i = 0; i < ListPanel.Count; ++i)
        {
            if (ListPanel[i].gameObject.name == _ui)
                return ListPanel[i].gameObject;
        }
        return null;
    }

    public static UIBasePanel GetFirstPanel()
    {
        for (int i = 0; i < ListPanel.Count; ++i)
        {
            if (ListPanel[i].eUIType == UIBasePanel.UIType.Default)
                return ListPanel[i];
        }

        return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Prev();
    }

    public void Prev()
    {
        int CurIdx = -1;
        int PrevIdx = -1;
        for (int i = 0; i < ListPanel.Count; ++i)
        {
            if (CurIdx == -1 && CurPanel.name == ListPanel[i].name)
            {
                CurIdx = i;
            }
            else
            {
                if (ListPanel[i].eUIType != UIBasePanel.UIType.Ignore)
                {
                    PrevIdx = i;
                }
            }
        }

        if (CurIdx >= 0)
        {
            if (ListPanel[CurIdx] is LobbyPanel)
            {
                OpenMessagePopup("게임을 종료 하시겠습니까?", delegate ()
                {
                    Debug.Log("OK");
                }, delegate () { }, "예", "아니오", "게임 종료");
            }
            else if (PrevIdx >= 0)
            {
                ListPanel[CurIdx].Hide();
                ListPanel[PrevIdx].LateInit();
                CurPanel = ListPanel[PrevIdx];
                CurPanel.Prev();
            }
        }
    }

    static void OpenEvent()
    {
        for (int i = 0; i < ListPanel.Count; ++i)
        {
            if (CurPanel.name == ListPanel[i].name)
            {
                UIBasePanel panel = ListPanel[i];
                ListPanel.RemoveAt(i);
                ListPanel.Insert(0, panel);
                CurPanel = panel;
                break;
            }
        }
    }

    public static void CloseEvent()
    {
        for (int i = 0; i < ListPanel.Count; ++i)
        {
            if (CurPanel.name == ListPanel[i].name)
            {
                CurPanel = ListPanel[i + 1];
                ListPanel.RemoveAt(i);
                break;
            }
        }
    }

    public void OpenMessagePopup(string msg, System.Action _call1 = null, System.Action _call2 = null, string _btnName1 = "확인", string _btnName2 = "취소", string _title = "알림")
    {
        OpenUI("Popup/MessagePopup").GetComponent<MessagePopup>().SetData(msg, _call1, _call2, _btnName1, _btnName2, _title);
    }

    public void OpenToastMessagePopup(string msg)
    {
        OpenUI("Popup/ToastMessagePopup").GetComponent<ToastMessagePopup>().SetData(msg);
    }



    // Root부터 하위 모든 오브젝트의 레이어를 _Layer로 바꾼다
    public static void SetLayer(Transform _Root, int _Layer)
    {
        Transform[] trans = _Root.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trans.Length; ++i)
            trans[i].gameObject.layer = _Layer;
    }
}
