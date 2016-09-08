using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetBundleMake {

    [MenuItem("Build/AssetBundle Make")]
    public static void Make()
    {
        string path = Application.dataPath + "/AssetBundle" + "/TextureData.unity3d";
        Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        BuildPipeline.BuildAssetBundle(Selection.activeObject, selects, path, BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
    }
}
