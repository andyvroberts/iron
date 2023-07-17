using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataReader.Common
{
    internal class FileUtils
    {

        /// <summary>
        /// Validate the file path is a file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool DoesFileExist(string path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Determine if the given file is encrypted.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool IsFileEncrypted(string path)
        {
            FileAttributes f1 = File.GetAttributes(path);

            if ((f1 & FileAttributes.Encrypted) == FileAttributes.Encrypted)
                return true;
            else
                return false;
        }



        //internal enum CharType
        //{
        //    Comma = 1,
        //    Tab = 2,
        //    DoubleQuote = 3,
        //    SingleQuote = 4,
        //    Newline = 5,
        //    CR = 6,
        //    Digit = 7,
        //    Letter = 8,
        //    Punctuation = 9,
        //    Other = 99
        //};

    }
}
