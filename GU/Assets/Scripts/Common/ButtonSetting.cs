using UnityEngine;
using System.Collections;

public class ButtonSetting : MonoBehaviour {
    public enum ButtonType
    {
        ScaleColor,
        Scale,
        Color,
    };
    public ButtonType eButtonType = ButtonType.ScaleColor;

    public Transform Target;

	void Start () {
        switch (eButtonType)
        {
            case ButtonType.ScaleColor:
                SetButtonScale();
                SetButtonColor();
                break;

            case ButtonType.Scale:
                SetButtonScale();
                break;

            case ButtonType.Color:
                SetButtonColor();
                break;
        };
	}

    void SetButtonScale()
    {
        if (gameObject.GetComponent<UIButtonScale>())
            gameObject.AddComponent<UIButtonScale>();
        if (Target != null)
            gameObject.AddComponent<UIButtonScale>().tweenTarget = Target;
        gameObject.AddComponent<UIButtonScale>().duration = 0.05f;
    }

    void SetButtonColor()
    {
        if (gameObject.GetComponent<UIButtonColor>())
            gameObject.AddComponent<UIButtonColor>();
        if (Target != null)
            gameObject.AddComponent<UIButtonColor>().tweenTarget = Target.gameObject;
    }
}
