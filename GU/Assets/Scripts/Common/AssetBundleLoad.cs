using UnityEngine;
using System.Collections;

public class AssetBundleLoad : Singleton<AssetBundleLoad> {

    static bool UnitLoadReady = true;
    static bool TableLoadReady = true;

    // 유닛 에셋번들 로드
    public void AssetUnitLoad(string _UnitName, System.Action<GameObject> _call = null, GameObject _parent = null, float _size = 100)
    {
        StartCoroutine(_AssetUnitLoad(_UnitName, _call, _parent, _size));
    }

    IEnumerator _AssetUnitLoad(string _UnitName, System.Action<GameObject> _call = null, GameObject _parent = null, float _size = 100)
    {
        while (!UnitLoadReady)
            yield return null;

        UnitLoadReady = false;
        WWW www = WWW.LoadFromCacheOrDownload("http://ssmktr.ivyro.net/GrowthUnit/AssetBundle/UnitDatas/UnitDatas.unity3d", GameInfo.UnitVersion);
        yield return www;

        if (www.isDone && www.error == null)
        {
            AssetBundle bundle = www.assetBundle;
            if (bundle != null)
            {
                AssetBundleRequest req = bundle.LoadAssetAsync(_UnitName, typeof(GameObject));
                yield return req;

                if (req != null)
                {
                    GameObject UnitAsset = req.asset as GameObject;
                    if (UnitAsset != null)
                    {
                        GameObject obj = Instantiate(UnitAsset);
                        if (_parent != null)
                            obj.transform.parent = _parent.transform;
                        obj.transform.localScale = Vector3.one * _size;
                        bundle.Unload(false);
                        UnitLoadReady = true;

                        if (_call != null)
                            _call(obj);
                    }
                    else
                    {
                        UnitLoadReady = true;
                        Debug.LogWarning("Not Found : " + _UnitName);
                    }
                }
            }
        }
        else
        {
            UnitLoadReady = true;
            Debug.LogError(www.error);
        }
    }
}
