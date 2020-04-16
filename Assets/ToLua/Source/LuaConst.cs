using UnityEngine;

public static class LuaConst
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static string luaBaseDir = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS && !UNITY_EDITOR
    public static string luaBaseDir = Application.dataPath + "Raw";
#else 
    public static string luaBaseDir = Application.dataPath;
#endif
    public static string luaDir = luaBaseDir + "/Lua";                //lua逻辑代码目录
    public static string toluaDir = luaBaseDir + "/ToLua/Lua";        //tolua lua文件目录

    public static string luaResBaseDir = Application.persistentDataPath;
#if UNITY_STANDALONE
    public static string osDir = "Win";
#elif UNITY_ANDROID
    public static string osDir = "Android";            
#elif UNITY_IPHONE
    public static string osDir = "iOS";        
#else
    public static string osDir = "";        
#endif

    public static string luaResDir = string.Format("{0}/{1}/Lua", luaResBaseDir, osDir);      //手机运行时lua文件下载目录        

    public static bool openLuaSocket = true;            //是否打开Lua Socket库
    public static bool openLuaDebugger = false;         //是否连接lua调试器
}
