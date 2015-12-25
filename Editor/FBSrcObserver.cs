using UnityEngine;
using UnityEditor;
using System.Collections;

public class FBSrcObserver : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool modified = false;
        for (var i = 0; i < importedAssets.Length; i++)
        {
            var str = importedAssets[i];
            if (CheckFileName(str))
            {
                modified = true;
                break;
            }
        }
        for (var i = 0; i < deletedAssets.Length; i++)
        {
            var str = deletedAssets[i];
            if (CheckFileName(str))
            {
                modified = true;
                break;
            }
        }

        for (var i = 0; i < movedFromAssetPaths.Length; i++)
        {
            var str = movedFromAssetPaths[i];
            if (CheckFileName(str))
            {
                modified = true;
                break;
            }
        }
        if (modified)
        {
            GenerateAndRefresh();
        }
    }

    static bool CheckFileName(string str)
    {
        return str.StartsWith("Assets/FlatStructs") && str.EndsWith(".fb");
    }

    [MenuItem("Window/FlatBuffer/Generate")]
    static void GenerateAndRefresh()
    {
        Generate();
        AssetDatabase.Refresh();
    }
    static void Generate()
    {
        Clean();
        var assetPath = Application.dataPath;
        var src_dir = System.IO.Path.Combine(assetPath, "FlatStructs/");
        var files = GetFBFiles(src_dir);
        var out_dir = System.IO.Path.Combine(assetPath, "Scripts/Messages/");
        if (files.Length == 0)
            return;
        Debug.Log(Application.dataPath);
        System.Diagnostics.Process p = new System.Diagnostics.Process();
#if UNITY_EDITOR_WIN
        p.StartInfo.FileName = "flatc.exe";
#else 
        p.StartInfo.FileName = "flatc";
#endif
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        var sb = new System.Text.StringBuilder("-n -o ");
        sb.Append(out_dir);
        AppendFileList(sb, GetFBFiles(src_dir));
        p.StartInfo.Arguments = sb.ToString();
        p.Start();//启动程序
        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();//等待程序执行完退出进程
        p.Close();
        Debug.Log(output);
    }

    static void AppendFileList(System.Text.StringBuilder sb, string[] files)
    {
        foreach (var item in files) {
            sb.Append(' ');
            sb.Append(item);
        }
    }

    static string[] GetFBFiles(string path)
    {
        System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo(path);
        var files = dirinf.GetFiles("*.fb");
        string[] ret = new string[files.Length];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = files[i].FullName;
        }
        return ret;
    }

    [MenuItem("Window/FlatBuffer/Clean")]
    static void CleanAndRefresh()
    {
        Clean();
        AssetDatabase.Refresh();
    }
    static void Clean()
    { 
        var assetPath = Application.dataPath;
        var dir = System.IO.Path.Combine(assetPath, "Scripts/Messages/");
        System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo(dir);
        var dirs = dirinf.GetDirectories("*", System.IO.SearchOption.TopDirectoryOnly);
        foreach (var item in dirs)
        {
            item.Delete(true);
        }
        var files = dirinf.GetFiles("*", System.IO.SearchOption.TopDirectoryOnly);
        foreach (var item in files)
        {
            item.Delete();
        }
    }
}
