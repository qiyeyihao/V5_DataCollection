using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace V5_DataCollection._Class.PythonExt {
    /// <summary>
    /// 执行python脚本
    /// </summary>
    public class PythonExtHelper {

        /// <summary>
        /// 运行Python脚本
        /// </summary>
        /// <param name="pythonFile"></param>
        /// <returns></returns>
        public static string RunPython(string pythonFile, string inputUrl, object inputObj) {

            ScriptEngine _engine = Python.CreateEngine();

            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pythonFile);

            #region 字符串
            //加载必须的类库
            var code = @"import sys" + "\n" +
                @"from System.IO import Path" + "\n" +
                @"sys.path.append("".\pythonlib.zip"")" + "\n" +
                @"import clr" + "\n" +
                @"clr.AddReferenceToFileAndPath(Path.GetFullPath(r'System\pythonlibs\V5_PythonLibs.dll'))" + "\n" +
                @"execfile(Path.GetFullPath(r'" + pythonFile + @"'))";
            var source = _engine.CreateScriptSourceFromString(code);

            var scope = _engine.CreateScope();
            source.Execute(scope);
            #endregion

            #region 文件
            //var source = _engine.CreateScriptSourceFromFile(fileName, Encoding.Default, SourceCodeKind.Statements);

            //CompiledCode _code = source.Compile();

            //var scope = _engine.CreateScope();

            //_code.Execute(scope);
            #endregion

            var main = scope.GetVariable<Func<string, object, string>>("main");

            return main(inputUrl, inputObj);

        }
    }
}
