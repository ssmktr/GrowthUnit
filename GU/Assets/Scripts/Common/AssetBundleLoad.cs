using UnityEngine;
using System.Collections;

public class AssetBundleLoad : Singleton<AssetBundleLoad> {

    public static bool UnitLoadReady = true;
    public static bool TextureLoadReady = true;
    public static bool TableLoadReady = true;

    #region UNITLOAD
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
        WWW www = WWW.LoadFromCacheOrDownload(GameInfo.NetworkUrl + GameInfo.AssetBundlePath + "UnitData/UnitData.unity3d", GameInfo.UnitVersion);
        while (!www.isDone)
            yield return null;

        yield return www;

        if (www.error == null)
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
                        GameManager.ViewDebug("Not Found : " + _UnitName);
                    }
                }
            }
        }
        else
        {
            UnitLoadReady = true;
            GameManager.ViewDebug(www.error);
        }
    }
    #endregion
    #region TEXTURELOAD
    // 유닛 에셋번들 로드
    public void AssetTextureLoad(string _TextureName, System.Action<Texture> _call = null)
    {
        StartCoroutine(_AssetTextureLoad(_TextureName, _call));
    }

    IEnumerator _AssetTextureLoad(string _TextureName, System.Action<Texture> _call = null)
    {
        while (!TextureLoadReady)
            yield return null;

        TextureLoadReady = false;
        WWW www = WWW.LoadFromCacheOrDownload(GameInfo.NetworkUrl + GameInfo.AssetBundlePath + "/TextureData/TextureData.unity3d", GameInfo.TextureVersion);
        while (!www.isDone)
            yield return null;

        yield return www;

        if (www.error == null)
        {
            AssetBundle bundle = www.assetBundle;
            if (bundle != null)
            {
                AssetBundleRequest req = bundle.LoadAssetAsync(_TextureName, typeof(Texture));
                yield return req;

                if (req != null)
                {
                    Texture UnitAsset = req.asset as Texture;
                    if (UnitAsset != null)
                    {
                        bundle.Unload(false);
                        TextureLoadReady = true;

                        if (_call != null)
                            _call(UnitAsset);
                    }
                    else
                    {
                        TextureLoadReady = true;
                        GameManager.ViewDebug("Not Found : " + _TextureName);
                    }
                }
            }
        }
        else
        {
            TextureLoadReady = true;
            GameManager.ViewDebug(www.error);
        }
    }
    #endregion
}
