using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LuaInterface;


public class GlobalEventHanlder
{
    public static LuaFunction luaGlobalCallback = null;


    public static void Call(int id)
    {
        luaGlobalCallback.BeginPCall();
        luaGlobalCallback.Push(id);
        luaGlobalCallback.PCall();
        luaGlobalCallback.EndPCall();
    }

    public static void Call(int id, int int1)
    {
        luaGlobalCallback.BeginPCall();
        luaGlobalCallback.Push(id);
        luaGlobalCallback.Push(int1);
        luaGlobalCallback.PCall();
        luaGlobalCallback.EndPCall();
    }


	public static void Call(int id, bool b)
	{
		luaGlobalCallback.BeginPCall();
		luaGlobalCallback.Push(id);
		luaGlobalCallback.Push(b);
		luaGlobalCallback.PCall();
		luaGlobalCallback.EndPCall();
	}

    public static void Call(int id, int int1, float float1)
    {
        luaGlobalCallback.BeginPCall();
        luaGlobalCallback.Push(id);
        luaGlobalCallback.Push(int1);
        luaGlobalCallback.Push(float1);
        luaGlobalCallback.PCall();
        luaGlobalCallback.EndPCall();
    }

    public static void Call(int id, int int1, bool bool1)
    {
        luaGlobalCallback.BeginPCall();
        luaGlobalCallback.Push(id);
        luaGlobalCallback.Push(int1);
        luaGlobalCallback.Push(bool1);
        luaGlobalCallback.PCall();
        luaGlobalCallback.EndPCall();
    }

    public static void Call(int id, int int1, Vector3 v1)
    {
        luaGlobalCallback.BeginPCall();
        luaGlobalCallback.Push(id);
        luaGlobalCallback.Push(int1);
        luaGlobalCallback.Push(v1);
        luaGlobalCallback.PCall();
        luaGlobalCallback.EndPCall();
    }


	public static void Call(int id, int int1, GameObject go1)
	{
		luaGlobalCallback.BeginPCall();
		luaGlobalCallback.Push(id);
		luaGlobalCallback.Push(int1);
		luaGlobalCallback.Push(go1);
		luaGlobalCallback.PCall();
		luaGlobalCallback.EndPCall();
	}


	public static void Call(int id, int int1, GameObject go1, int int2, int int3)
	{
		luaGlobalCallback.BeginPCall();
		luaGlobalCallback.Push(id);
		luaGlobalCallback.Push(int1);
		luaGlobalCallback.Push(go1);
		luaGlobalCallback.Push(int2);
		luaGlobalCallback.Push(int3);
		luaGlobalCallback.PCall();
		luaGlobalCallback.EndPCall();
	}

	public static void Call(int id, string str1, int int1, string str2)
	{
		luaGlobalCallback.BeginPCall();
		luaGlobalCallback.Push(id);
		luaGlobalCallback.Push(str1);
		luaGlobalCallback.Push(int1);
		luaGlobalCallback.Push(str2);
		luaGlobalCallback.PCall();
		luaGlobalCallback.EndPCall();
	}

    public static void Call(int id, int int1, string str)
    {
        luaGlobalCallback.BeginPCall();
        luaGlobalCallback.Push(id);
        luaGlobalCallback.Push(int1);
        luaGlobalCallback.Push(str);
        luaGlobalCallback.PCall();
        luaGlobalCallback.EndPCall();
    }
}

