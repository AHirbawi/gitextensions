﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GitCommandsTest.Helpers
{
    public static class FileInfoExtensions
    {
        public static string CheckSum(this FileInfo currentFile)
        {
            var md5Provider = MD5.Create();
            var md5Bytes = md5Provider.ComputeHash(File.ReadAllBytes(currentFile.FullName));

            StringBuilder hash = new StringBuilder();

            foreach (var md5Byte in md5Bytes)
            {
                hash.Append(md5Byte.ToString("x2"));
            }

            return hash.ToString();
        }

        public static bool Compare(this FileInfo currentFile, string otherFilePath)
        {
            try
            {
                byte[] currentFileBytes = File.ReadAllBytes(currentFile.FullName);
                byte[] otherFileBytes = File.ReadAllBytes(otherFilePath);

                bool areEqual = true;

                Parallel.For(0, currentFileBytes.LongLength, (i, state) =>
                    {
                        if (currentFileBytes[i] != otherFileBytes[i])
                        {
                            areEqual = false;
                            state.Stop();
                            return;
                        }
                    });

                return areEqual;
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            
        }

        public static bool Compare(this FileInfo currentFile, FileInfo otherFile)
        {
            if (currentFile == null || otherFile == null)
                return false;

            return currentFile.Compare(otherFile.FullName);
        }
    }
}
