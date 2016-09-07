using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

    public UICamera UiCamera;

	void Start () {
        for (int i = 0; i < 58; ++i)
        {
            AssetBundleLoad.Instance.AssetUnitLoad(string.Format("human{0:00}", i + 1), (go) => {
                go.transform.localPosition = Vector3.zero;
            }, UiCamera.gameObject);
        }
	}
}
