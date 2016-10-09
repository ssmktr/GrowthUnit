using UnityEngine;
using System.Collections;

public class UIBasePanel : MonoBehaviour {

    public enum UIType
    {
        Default,
        Message,
        Popup,
        Ignore,
    };
    public UIType eUIType = UIType.Default;

    public object[] parameters;

    public virtual void Init()
    {
    }

    public virtual void LateInit()
    {
        GameObject UpInfo = UIManager.GetUI("UpInfoPanel");
        if (UpInfo != null)
            UpInfo.GetComponent<UpInfoPanel>().SetBackBtn(!(UIManager.GetFirstPanel() is LobbyPanel));

        Open();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Close()
    {
        if (eUIType != UIType.Ignore)
            UIManager.CloseEvent();

        GameObject UpInfo = UIManager.GetUI("UpInfoPanel");
        if (UpInfo != null)
            UpInfo.GetComponent<UpInfoPanel>().SetBackBtn(!(UIManager.GetFirstPanel() is LobbyPanel));

        Destroy(gameObject);
    }

    public virtual void Prev()
    {

    }
}
