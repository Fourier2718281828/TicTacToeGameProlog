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
        private const string PL_FILENAME33 = "Assets/Prolog/Minimax33.pl";
        private const string PL_FILENAME34 = "Assets/Prolog/Minimax34.pl";
        private const string PL_FILENAME43 = "Assets/Prolog/Minimax43.pl";
        private const string PL_FILENAME44 = "Assets/Prolog/Minimax44.pl";
        private Process prologProcess;
        public CmdPrologEngine(int rows, int cols)
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
                UnityEngine.Debug.Log($"Filename {GetFilename(rows, cols)}");
                await SendInput($"consult('{GetFilename(rows, cols)}').");
            });

        }

        async Task SendInput(string input)
        {
            await prologProcess.StandardInput.WriteLineAsync(input);
        }

        async Task<string> ReadOutput()
        {
            return await prologProcess.StandardOutput.ReadLineAsync();
        }

        public async Task<string> Query(bool cut, string predicateName, string outputVar, params string[] ps)
        {
            string query = QueryFormat(cut, predicateName, ps);
            await SendInput(query);
            string result = null;
            string output;
            while ((output = await ReadOutput()) != null)
            {
                if (output.StartsWith(outputVar))
                {
                    return output.Substring((outputVar + " = ").Length).Trim('.');
                }
            }

            return result;
        }

        private string GetFilename(int rows, int cols)
        {
            if (rows == 3 && cols == 3)
                return PL_FILENAME33;
            if (rows == 3 && cols == 4)
                return PL_FILENAME34;
            if (rows == 4 && cols == 3)
                return PL_FILENAME43;
            if (rows == 4 && cols == 4)
                return PL_FILENAME44;
            return PL_FILENAME33;
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
