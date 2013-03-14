using System;
using System.IO;

namespace DatabaseSizer.Helpers
{
    public class LogFile
    {
        private readonly TextWriter _textWriter;

        public LogFile(string fileName, bool deleteIfExists)
        {
            try
            {
                if (File.Exists(fileName))
                    if (deleteIfExists)
                        File.Delete(fileName);

                this._textWriter = new StreamWriter(fileName);
                this._textWriter = TextWriter.Synchronized(this._textWriter);
            }
            catch
            {
                this._textWriter = null;
            }
        }

        public void WriteLn(string message)
        {
            var fullMessage = DateTime.Now.ToString("dd/MM/yyyy mm:hh:ss.ff").PadRight(30) + message;
            this._textWriter.WriteLine(fullMessage);
            this._textWriter.Flush();
        }

// ReSharper disable MethodOverloadWithOptionalParameter
        public void WriteLn(string formatMessage, params object[] args)
// ReSharper restore MethodOverloadWithOptionalParameter
        {
            this.WriteLn(string.Format(formatMessage, args));
        }
    }
}