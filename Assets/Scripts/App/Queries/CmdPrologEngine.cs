using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using UnityEngine;


namespace App.Queries
{
    public class CmdPrologEngine
    {
        private const string PL_FILENAME = "Assets/Prolog/Minimax.pl";
        private Process prologProcess;
        public CmdPrologEngine()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"Assets\Resources\swipl\bin\swipl.exe";
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            prologProcess = new Process();
            prologProcess.StartInfo = startInfo;
            prologProcess.Start();
            UnityEngine.Debug.Log("Started");

            Task.Run(async () =>
            {
                await SendInput($"consult('{PL_FILENAME}').");
                //var output = await Query(false, "next_turn", "Index",
                //    "[0, 1, 1,  0, 0, 0,  0, 0, 0]", "2", "Index");
                //UnityEngine.Debug.Log($"Output:{output}");([1, 1, 1,  0, 1, 0,  0, 0, 1], ).
                var output = await Query(false, "all_victory_sequences", "Seq",
                    "[1, 1, 1,  0, 1, 0,  0, 0, 1]", "Seq");
                UnityEngine.Debug.Log($"Output:{output}");

            });

            //SendInput($"consult('{PL_FILENAME}').");
            //var output = Query(false, "next_turn", "Index",
            //    "[0, 1, 1,  0, 0, 0,  0, 0, 0]", "2", "Index");
            //UnityEngine.Debug.Log($"Output:{output}");
        }

        // send input
        async Task SendInput(string input)
        {
            await prologProcess.StandardInput.WriteLineAsync(input);
        }

        // read output
        async Task<string> ReadOutput()
        {
            return await prologProcess.StandardOutput.ReadLineAsync();
        }

        private async void TryExample()
        {
            await SendInput($"consult('{PL_FILENAME}').");
            await SendInput("next_turn([0, 1, 1,  0, 0, 0,  0, 0, 0], 2, Index).");
            string output;
            while ((output = await ReadOutput()) != null)
            {
                UnityEngine.Debug.Log($"Output: {output}");
            }
            prologProcess.Close();
        }


        public async Task<string> Query(bool cut, string predicateName, string outputVar, params string[] ps)
        {
            string query = QueryFormat(cut, predicateName, ps);
            await SendInput(query);
            string result = null;
            //await Task.Run(async () =>
            //{
            //    string output;
            //    while ((output = await ReadOutput()) != null)
            //    {
            //        //UnityEngine.Debug.Log($"Output: {output}");
            //        if (output == "Index = 8.")//output.StartsWith(outputVar))
            //        {
            //            result = output;
            //        }
            //    }
            //    UnityEngine.Debug.Log($"Result: {result}");
            //}
            //);

            string output;
            while ((output = await ReadOutput()) != null)
            {
                //UnityEngine.Debug.Log($"Output: {output}");
                if (output.StartsWith(outputVar))
                {
                    return output.Substring((outputVar + " = ").Length).Trim('.');
                }
            }

            return result;
        }

        

        private static string QueryFormat(bool cut, string predicateName, params string[] ps)
        {
            string res = "";

            res += predicateName + "(";
            for (var i = 0; i < ps.Length; i++)
                res += ps[i] +
                    (
                        i == ps.Length - 1
                        ? ""
                        : ", "
                    );
            res += cut
                ? "),!."
                : ").";

            return res;
        }
    }
}
