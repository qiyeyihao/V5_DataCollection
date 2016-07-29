using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XmlDatabase
{
    static class TextWriterExtensions
    {
        public static void WriteEx(this TextWriter writer, string value,bool quickFlush) {
            writer.WriteLine(value);
            if (quickFlush)
                writer.Flush();
        }

        public static void WriteEx(this TextWriter writer, string value) {
            WriteEx(writer, value, false);
        }
    }
}
