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
}
