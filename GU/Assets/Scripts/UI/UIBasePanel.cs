using UnityEngine;
using System.Collections;

public class UIBasePanel : MonoBehaviour {

    public enum UIType
    {
        Default,
        Message,
        Toast,
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
        Destroy(gameObject);
    }
}
