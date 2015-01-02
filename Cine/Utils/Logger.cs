using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Cine.Utils
{
    public class Logger
    {
        public static void Log(string text, 
                                StackTraza parentMember = null,
                                [CallerFilePath] string file = "", 
                                [CallerMemberName] string member = "",
                                [CallerLineNumber] int line = 0)
                                
        {
            if (parentMember != null)
            {
                foreach (var traza in parentMember.ParentMembers)
                {
                    Trace.WriteLine(String.Format("{0}:{1} => {2} {3}", Path.GetFileName(traza.File), traza.Line, traza.Member, traza.Text));
                }
            }
            Trace.WriteLine(String.Format("{0}:{1} => {2} : {3}", Path.GetFileName(file), line, member, text));
        }
    }
}
