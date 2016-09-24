using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager> {
    static List<UIBasePanel> ListPanel;

    public GameObject UIRoot, SystemRoot;

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
            if (UIManager.Instance.UIRoot != null)
                Panel.transform.parent = UIManager.Instance.UIRoot.transform;
            Panel.transform.localPosition = Vector3.zero;
            Panel.transform.localScale = Vector3.one;
            Panel.GetComponent<UIBasePanel>().Init();

            ListPanel.Insert(0, Panel.GetComponent<UIBasePanel>());
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
        UIBasePanel Panel = ListPanel.Find(_panel => _panel.name == _ui);

        if (Panel == null)
            return null;

        return Panel.gameObject;
    }

    public static UIBasePanel GetFirstPanel()
    {
        if (ListPanel.Count > 0)
            return ListPanel[0];

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
                    break;
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
                UIBasePanel curPanel = ListPanel[CurIdx];
                curPanel.Hide();
                ListPanel.Remove(curPanel);
                ListPanel.Add(curPanel);

                ListPanel[CurIdx].LateInit();
                CurPanel = ListPanel[CurIdx];
                CurPanel.Prev();
                OpenEvent();
            }
        }
    }

    static void OpenEvent()
    {
        UIBasePanel Panel = ListPanel.Find(_panel => _panel == CurPanel);
        if (Panel != null)
        {
            ListPanel.Remove(Panel);
            ListPanel.Insert(0, Panel);
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
