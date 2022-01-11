using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Playground.Logging
{
    internal static partial class Debug
    {
        /// <summary>
        /// Custom debug method that provides additional detail about origin caller and file.
        /// </summary>
        /// <param name="debugMessage">Message to write to console</param>
        /// <param name="file">Name of file from where the Debug is called from</param>
        /// <param name="member">Member that calls the Debug method</param>
        /// <param name="line">Line number in file from where the Debug method is called</param>
        [Conditional("DEBUG")]
        internal static void WriteLine(object debugMessage, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            System.Diagnostics.Debug.WriteLine($"{Path.GetFileName(file)} - {member}({line}): {debugMessage}");
        }
    }
}
