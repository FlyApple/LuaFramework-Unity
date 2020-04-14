
using UnityEngine;
using LuaInterface;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;

public class LuaAndroidAssetsLoader
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject   _activityMainObject;

    private AndroidJavaClass    _moduleClass;
    private AndroidJavaObject   _moduleObject;
#endif
    public LuaAndroidAssetsLoader()
    {
        InitializeAndroid();
    }

    private bool InitializeAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _activityMainObject = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

        _moduleClass = new AndroidJavaClass("com.mcmcx.util.LuaAssetsLoader");
        _moduleObject = _moduleClass.CallStatic<AndroidJavaObject>("getSingletonAndCreate",
            new object[] { _activityMainObject });
#endif
        return true;
    }

    public byte[] ReadBufferFromFile(string fileName)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        if (_moduleObject != null) {
            sbyte[] buffer = _moduleObject.Call<sbyte[]>("readBufferFromFile", new object[] { fileName });
            if(buffer != null) {
                byte[] result = new byte[buffer.Length];
                System.Buffer.BlockCopy(buffer, 0, result, 0, buffer.Length);
                return result;
            }
        }
#endif
        return null;
    }

    public string ReadStringFromFile(string fileName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_moduleObject != null) {
            return _moduleObject.Call<string>("readStringFromFile", new object[] { fileName });
        }
#endif
        return null;
    }
}
