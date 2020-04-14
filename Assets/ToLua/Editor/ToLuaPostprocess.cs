

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
#if UNITY_ANDROID
using UnityEditor.Android;
#endif

#if UNITY_EDITOR 
public class ToLuaPostprocess : IPreprocessBuildWithReport, IPostprocessBuildWithReport
#if UNITY_ANDROID
    , IPostGenerateGradleAndroidProject
#endif
{
    //
    public static string name;
    public static BuildTarget target;

    public static string toluaBasedir { get { return FixPathName(Path.Combine(Application.dataPath, "ToLua/Lua")); } }
    public static string luaBasedir { get { return FixPathName(Path.Combine(Application.dataPath, "Lua")); } }

    public static string assetsBasedir;
    public static string[] scriptFilenames;
    //
    public int callbackOrder { get { return 0; } }

    private static string FixPathName(string path)
    {
#if UNITY_EDITOR_WIN
        string pattern = @"/";
        return Regex.Replace(path, pattern, "\\");
#else
        string pattern = @"\\";
        return Regex.Replace(path, pattern, "/");
#endif
    }

    private static List<string>[] TraversalDirectory(string directory, string filter)
    {
        List<string> dirnames = new List<string>();
        List<string> filenames = new List<string>();
        dirnames.Add(FixPathName(directory));

        string[] dirs = Directory.GetDirectories(directory);
        string[] files = Directory.GetFiles(directory, filter, SearchOption.TopDirectoryOnly);
        foreach (var v in files)
        {
            if (File.Exists(v))
            {
                filenames.Add(FixPathName(v));
            }
        }
        foreach (var v in dirs)
        {
            if (Directory.Exists(v))
            {
                List<string>[] result = TraversalDirectory(v, filter);
                dirnames.AddRange(result[0]);
                filenames.AddRange(result[1]);
            }
        }
        return new List<string>[] { dirnames, filenames };
    }


    public void OnPreprocessBuild(BuildReport report)
    {
        this.BuildingBegin(report);
    }

#if UNITY_ANDROID
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        MonoBehaviour.print("Building (" + name + ") : generate gradle -> " + path);
        assetsBasedir = FixPathName(Path.Combine(path, "src/main/assets"));

        //
        if (scriptFilenames != null && scriptFilenames.Length > 0)
        {
            foreach (var n in scriptFilenames) {
                string tn = n.Replace(FixPathName(Application.dataPath + "/"), "");
                tn = FixPathName(Path.Combine(assetsBasedir, tn));

                FileInfo fi = new FileInfo(n);
                if(fi.Exists) {
                    string dn = Path.GetDirectoryName(tn);
                    if (!Directory.Exists(dn)) {
                        Directory.CreateDirectory(dn);
                        if (!Directory.Exists(dn)) { continue; }
                    }

                    fi.CopyTo(tn, true);
                    //MonoBehaviour.print("Building (" + name + ") : (copy) [" + fi.Length + " bytes] -> " + tn);
                }
            }
        }
    }
#endif

    public void OnPostprocessBuild(BuildReport report)
    {
        this.BuildingEnd(report);
    }


    private bool BuildingBegin(BuildReport report)
    {
        name = report.name;
        target = report.summary.platform;

        MonoBehaviour.print("Building ("+ target + "->" + name + ") : ----- ToLua Building Begin ----- ");

        //
        MonoBehaviour.print("Building (" + name + ") Bundle Version: " + PlayerSettings.bundleVersion);
#if UNITY_ANDROID
        MonoBehaviour.print("Building (" + name + ") Bundle Code: " + PlayerSettings.Android.bundleVersionCode);
#elif UNITY_IOS
        MonoBehaviour.print("Building (" + name + ") Bundle Code: " + PlayerSettings.iOS.buildNumber)
#endif
        MonoBehaviour.print("Building (" + name + ") Bundle ID: " + PlayerSettings.applicationIdentifier);
        MonoBehaviour.print("Building (" + name + ") ToLua Path: " + toluaBasedir);
        MonoBehaviour.print("Building (" + name + ") Lua Path: " + luaBasedir);

        //Total lua script
        List<string>[] filenames = TraversalDirectory(toluaBasedir, "*.lua");
        List<string>[] tempnames = TraversalDirectory(luaBasedir, "*.lua");
        filenames[0].AddRange(tempnames[0]);
        filenames[1].AddRange(tempnames[1]);
        scriptFilenames = filenames[1].ToArray();
        MonoBehaviour.print("Building (" + name + ") Total Lua : " + filenames[0].Count + "," + filenames[1].Count);

        //
        return true;
    }

    private bool BuildingEnd(BuildReport report)
    {
        MonoBehaviour.print("Building (" + name + ") : ----- ToLua Building End ----- ");
        MonoBehaviour.print("Building (" + name + ") : result -> " + report.summary.result);
        if (report.summary.result == BuildResult.Failed || report.summary.result == BuildResult.Cancelled) {
            return false;
        }
        MonoBehaviour.print("Building (" + name + ") : output -> " + report.summary.outputPath);
        return true;
    }



}

#endif
