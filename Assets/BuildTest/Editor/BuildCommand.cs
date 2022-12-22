using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;


public class BuildCommand
{
    [MenuItem("Build/Build Android")]
    public static void Build()
    {
        string[] scenes =
        {
            "Assets/BuildTest/BuildTest.unity"
        };

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "D:/Development/Unity/Jenkins-unity-build-simple/Build/completed.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        // ビルド実行
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("ビルド成功: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("ビルド失敗");
        }
    }
}
