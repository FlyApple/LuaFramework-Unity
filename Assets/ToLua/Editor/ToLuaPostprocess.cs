

using System.IO;
using System.Collections;
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
    public string name;
    public BuildTarget target;

    public string toluaBasedir { get { return Path.Combine(Application.dataPath, "ToLua/Lua"); } }
    public string luaBasedir { get { return Path.Combine(Application.dataPath, "Lua"); } }

    public string assetsBasedir;
    //
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        this.BuildingBegin(report);
    }

#if UNITY_ANDROID
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        MonoBehaviour.print("Building (" + this.name + ") : generate gradle -> " + path);
        this.assetsBasedir = Path.Combine(path, "src/main/assets");
    }
#endif

    public void OnPostprocessBuild(BuildReport report)
    {
        this.BuildingEnd(report);
    }


    private bool BuildingBegin(BuildReport report)
    {
        this.name = report.name;
        this.target = report.summary.platform;

        MonoBehaviour.print("Building ("+ this.target + "->" + this.name + ") : ----- ToLua Building Begin ----- ");

        //
        MonoBehaviour.print("Building (" + this.name + ") Bundle Version: " + PlayerSettings.bundleVersion);
#if UNITY_ANDROID
        MonoBehaviour.print("Building (" + this.name + ") Bundle Code: " + PlayerSettings.Android.bundleVersionCode);
#elif UNITY_IOS
        MonoBehaviour.print("Building (" + this.name + ") Bundle Code: " + PlayerSettings.iOS.buildNumber)
#endif
        MonoBehaviour.print("Building (" + this.name + ") Bundle ID: " + PlayerSettings.applicationIdentifier);
        MonoBehaviour.print("Building (" + this.name + ") ToLua Path: " + this.toluaBasedir);
        MonoBehaviour.print("Building (" + this.name + ") Lua Path: " + this.luaBasedir);

        //Total lua script
        ArrayList[] filenames = this.TraversalDirectory(this.toluaBasedir, "*.lua");
        ArrayList[] tempnames = this.TraversalDirectory(this.luaBasedir, "*.lua");
        filenames[0].AddRange(tempnames[0]);
        filenames[1].AddRange(tempnames[1]);
        MonoBehaviour.print("Building (" + this.name + ") Total Lua : " + filenames[0].Count + "," + filenames[1].Count);

        //
        return true;
    }

    private bool BuildingEnd(BuildReport report)
    {
        MonoBehaviour.print("Building (" + this.name + ") : ----- ToLua Building End ----- ");
        MonoBehaviour.print("Building (" + this.name + ") : result -> " + report.summary.result);
        if (report.summary.result == BuildResult.Failed || report.summary.result == BuildResult.Cancelled) {
            return false;
        }
        MonoBehaviour.print("Building (" + this.name + ") : output -> " + report.summary.outputPath);
        return true;
    }


    private ArrayList[] TraversalDirectory(string directory, string filter)
    {
        ArrayList dirnames = new ArrayList();
        ArrayList filenames = new ArrayList();
        dirnames.Add(directory);

        string[] dirs = Directory.GetDirectories(directory);
        string[] files = Directory.GetFiles(directory, filter, SearchOption.TopDirectoryOnly);
        foreach (var v in files) {
            if (File.Exists(v)) {
                filenames.Add(v);
            }
        }
        foreach (var v in dirs) {
            if (Directory.Exists(v)) {
                ArrayList[] result = this.TraversalDirectory(v, filter);
                dirnames.AddRange(result[0]);
                filenames.AddRange(result[1]);
            }
        }
        return new ArrayList[] { dirnames, filenames };
    }
}

#endif
