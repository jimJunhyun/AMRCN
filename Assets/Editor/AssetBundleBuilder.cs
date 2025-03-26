using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder
{
	[MenuItem("Assets/Build AssetBundles")]
	internal static void BuildAllAssetBundles()
	{
		try
		{
			string assetBundleDirectory = "Assets/StreamingAssets";
			if (!Directory.Exists(assetBundleDirectory))
			{
				Directory.CreateDirectory(assetBundleDirectory);
			}
			BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
			Debug.Log("BUILDED");
		}
		catch (System.Exception e)
		{
			Debug.LogError($"BUILD FAILED : {e}");
		}
	}

	//static AssetBundleBuilder()
	//{
	//	Debug.Log("BUILDING");
	//	BuildAllAssetBundles();
	//}
	
}
