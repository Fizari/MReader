using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MReader.Core.Extensions
{
    public static class GeneralExtensions
    {
        public static void ForEachFile(this IEnumerable<FileInfo> list, Action<FileInfo> action)
        {
            foreach (FileInfo f in list)
            {
                action(f);
            }
        }

        public static IEnumerable<FileInfo> CustomSort(this IEnumerable<FileInfo> list)
        {
            int maxLen = list.Select(s => s.Name.Length).Max();

            return list.Select(s => new
            {
                OrgStr = s,
                SortStr = Regex.Replace(s.Name, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, char.IsDigit(m.Value[0]) ? ' ' : '\xffff'))
            })
            .OrderBy(x => x.SortStr)
            .Select(x => x.OrgStr);
        }

        public static void PrintDebug(this object o, string msg = "")
        {
            StackTrace stackTrace = new StackTrace();
            var methodName = stackTrace.GetFrame(1).GetMethod().Name;
            var s = msg == "" ? "" : " | " + msg;
            Console.WriteLine(o.GetType().Name + ">> " + methodName + s);
        }
    }
}
