using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCheese.Game
{
    public class Lua
    {
        public static Script script = new Script();

        public static void RunLuaScript(string name)
        {
            name = name.Replace(".lua", "");

            Log.Get().Info("[Lua] Running script {0}", name);

            script.DoString(File.ReadAllText("Script/" + name + ".lua"));
        }

        public static DynValue GetLuaGlobal(string globalName)
        {
            return script.Globals.Get(globalName);
        }
    }

    public class LuaNonGlobal
    {
        public static Dictionary<string, Script> data = new Dictionary<string, Script>();

        public static Script RunLuaScript(string name)
        {
            name = name.Replace(".lua", "");

            Log.Get().Info("[Lua] Running script {0}", name);

            Script script = new Script();

            script.DoString(File.ReadAllText("Script/" + name + ".lua"));

            data.Add(name, script);

            return script;
        }

        public static object GetLuaGlobal(string scriptName, string globalName)
        {
            if (data.ContainsKey(scriptName))
            {
                return data[scriptName].Globals.Get(globalName);
            }

            return null;
        }
    }
}
