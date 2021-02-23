using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class GameLua
{
    private static LuaEnv luaEnv = null;

    //将Lua读取为字节流
    public static byte[] SafeReadAllBytes(string inFile)
    {
        try
        {
            if (string.IsNullOrEmpty(inFile))
            {
                return null;
            }

            if (!File.Exists(inFile))
            {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllBytes(inFile);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeReadAllBytes failed! path = {0} with err = {1}", inFile, ex.Message));
            return null;
        }
    }

    //装载器
    public static byte[] CustomLoader(ref string filePath)
    {
        string scriptPath = string.Empty;
        scriptPath = Path.Combine(Application.dataPath, "LuaScripts"); //路径变成Asset/LuaScripts
        scriptPath = Path.Combine(scriptPath, filePath);

        Debug.Log("Custom Load lua script : " + scriptPath);
        return SafeReadAllBytes(scriptPath);
    }

    public static void Init()
    {
        if(luaEnv == null)
        {
            luaEnv = new LuaEnv();
        }
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);
        }
    }

    public static void SafeDoString(string scriptContent)
    { // 执行脚本, scriptContent脚本代码的文本内容;
        if (luaEnv != null)
        {
            try
            {
                luaEnv.DoString(scriptContent); // 执行我们的脚本代码;
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }

    /// <summary>
    /// 调用Lua文件
    /// </summary>
    /// <param name="scriptName">Lua文件名</param>
    public static void LoadScript(string scriptName)
    {
        SafeDoString(string.Format("require('{0}')", scriptName));
    }


    public static void EnterLuaGame()
    { // 进入游戏 
        if (luaEnv != null)
        {
            LoadScript("Main.lua");
        }
    }

}
