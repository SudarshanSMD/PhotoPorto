using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPorto.Utility
{
    /// <summary>
    /// FileUtility provides file utility methods.
    /// </summary>
    public class FileUtility
    {

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        // Refer: http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file-in-c
        // to use
        // using (Stream file = File.Create(filename))
        // {
        //CopyStream(input, file);
        // }
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
