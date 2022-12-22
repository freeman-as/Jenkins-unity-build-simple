using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;


public static class BuildCommand
{
    [MenuItem("Build/Build Android")]
    public static void Build()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        var scenes = GetTartgetScenes().ToArray();
        if (scenes.Length == 0)
        {
            Debug.LogError("ビルドシーンが設定されていません");
            return;
        }

        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "./Build/completed.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = IsDevelopmentBuild ? BuildOptions.Development : BuildOptions.None;

        if (IsDevelopmentBuild)
        {
            Debug.Log(GetScriptingDefineSymbolFromCommandlineArgument());
        }

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

    /// <summary>
    /// シンボル取得
    /// </summary>
    /// <returns></returns>
    public static string GetScriptingDefineSymbolFromCommandlineArgument()
    {
        var args = Environment.GetCommandLineArgs();

        int i, len = args.Length;

        for (i = 0; i < len; ++i)
        {
            switch (args[i])
            {
                case "-scriptingDefineSymbol":
                    return args[i + 1];
            }
        }

        return "";
    }


    private static bool IsDevelopmentBuild => Environment.GetCommandLineArgs().Contains("-developmentBuild");

    /// <summary>
    /// 有効なシーンのみ取得
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<string> GetTartgetScenes()
    {
        return EditorBuildSettings.scenes
            .Where(s => s.enabled == true)
            .Select(s => s.path);
    }

    /// <summary>
    /// 
    /// </summary>
    private static void SetPlayerSettings()
    {
        // ビルドごとに変える場合
        PlayerSettings.applicationIdentifier = "";
        PlayerSettings.productName = "";
        PlayerSettings.companyName = "";
    }

    /// <summary>
    /// 
    /// </summary>
    private static void SetEditorUserBuildSettings()
    {
        // ビルドごとに変える場合
        // テスト中はtrueにすると実機確認面倒なので、falseにしておく
        EditorUserBuildSettings.buildAppBundle = false;
    }
}
