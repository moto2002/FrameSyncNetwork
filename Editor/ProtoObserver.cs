using UnityEngine;
using UnityEditor;
using System.Collections;

public class ProtoObserver : AssetPostprocessor
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
        return str.StartsWith("Assets/proto_files") && str.EndsWith(".proto");
    }

    [MenuItem("Window/ProtoBuf/Generate")]
    static void GenerateAndRefresh()
    {
        Generate();
        AssetDatabase.Refresh();
    }
    static void Generate()
    {
        Clean();
        var assetPath = Application.dataPath;
        var src_dir = System.IO.Path.Combine(assetPath, "proto_files/");
        var files = GetFBFiles(src_dir);
        var out_dir =  System.IO.Path.Combine(assetPath, "Scripts/Messages/proto_files");
        foreach (var item in files) {
            GenCsharpFile(src_dir, item, out_dir);
        }
    }

    static void GenCsharpFile(string src_dir, string fullFileName, string outdir)
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
#if UNITY_EDITOR_WIN
        p.StartInfo.FileName = "protoc.exe";
#else
        p.StartInfo.FileName = "protoc";
#endif
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        var tempFileName = fullFileName + "bin";
        p.StartInfo.Arguments = string.Format("-I={0} --descriptor_set_out={1} --include_imports {2}", src_dir, tempFileName, fullFileName);
        Debug.Log("executing command: protoc " + p.StartInfo.Arguments);
        p.Start();//启动程序
        string output = p.StandardOutput.ReadToEnd();
        string error = p.StandardError.ReadToEnd();
        p.WaitForExit();//等待程序执行完退出进程
        p.Close();
        Debug.Log("standard output: " + output);
        Debug.Log("error output: " + error);
        p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "ProtoGen.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.Arguments = string.Format("-output_directory={0} {1}", outdir, tempFileName);
        Debug.Log("executing command: ProtoGen " + p.StartInfo.Arguments);
        p.Start();//启动程序
        output = p.StandardOutput.ReadToEnd();
        error = p.StandardError.ReadToEnd();
        p.WaitForExit();//等待程序执行完退出进程
        p.Close();
        Debug.Log("standard output: " + output);
        Debug.Log("error output: " + error);
        System.IO.File.Delete(tempFileName);
    }

    static void AppendFileList(System.Text.StringBuilder sb, string[] files)
    {
        foreach (var item in files)
        {
            sb.Append(' ');
            sb.Append(item);
        }
    }

    static string[] GetFBFiles(string path)
    {
        System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo(path);
        var files = dirinf.GetFiles("*.proto");
        string[] ret = new string[files.Length];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = files[i].FullName;
        }
        return ret;
    }

    [MenuItem("Window/ProtoBuf/Clean")]
    static void CleanAndRefresh()
    {
        Clean();
        AssetDatabase.Refresh();
    }

    static void Clean()
    {
        var assetPath = Application.dataPath;
        var dir = System.IO.Path.Combine(assetPath, "Scripts/Messages/proto_files");
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

    [MenuItem("Window/ProtoBuf/UpdateProtoFiles")]
    static void UpdateProtoFiles() {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
#if UNITY_EDITOR_WIN
        p.StartInfo.FileName = "git.exe";
#else
        p.StartInfo.FileName = "git";
#endif
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WorkingDirectory = System.IO.Path.Combine(Application.dataPath, "proto_files/");
        p.StartInfo.Arguments = "pull origin master";
        Debug.Log("executing command: git " + p.StartInfo.Arguments);
        p.Start();//启动程序
        string output = p.StandardOutput.ReadToEnd();
        string errorOutput = p.StandardError.ReadToEnd();
        p.WaitForExit();//等待程序执行完退出进程
        p.Close();
        Debug.Log(output);
        if (!string.IsNullOrEmpty(errorOutput)) {
            Debug.LogWarning(errorOutput);
        }
        AssetDatabase.Refresh();
    }
}