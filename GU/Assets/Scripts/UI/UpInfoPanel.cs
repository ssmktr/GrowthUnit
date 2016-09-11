using UnityEngine;
using System.Collections;

public class UpInfoPanel : UIBasePanel {

    public UITexture MainBack;

    public override void Init()
    {
        base.Init();
    }

    public override void LateInit()
    {
        base.LateInit();

        AssetBundleLoad.Instance.AssetTextureLoad("DesertBackGround", (tex) => {
            if (tex != null)
                MainBack.mainTexture = tex;
        });
    }
}
