﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameBaseClassLibrary
{
    public class VCBTool
    {
        public static List<GameBases> ReadBasesFromVCB(string VCBpath)
        {
            List<GameBases> lRet = new List<GameBases>();

            FileInfo fn = new FileInfo(VCBpath);
            if (fn.Extension.Contains("vcb"))
            {
                FileStream inputConfigStream = new FileStream(VCBpath, FileMode.Open, FileAccess.Read);
                GZipStream decompressedConfigStream = new GZipStream(inputConfigStream, CompressionMode.Decompress);
                IFormatter formatter = new BinaryFormatter();
                lRet = (List<GameBases>)formatter.Deserialize(decompressedConfigStream);
            }

            return lRet;
        }
        public static void ExportFile(List<GameBases> precomp, GameConsoles console, string outf)
        {
            CheckAndFixFolder(outf);
            Stream createConfigStream = new FileStream($@"{outf}\bases.vcb{console.ToString().ToLower()}", FileMode.Create, FileAccess.Write);
            GZipStream compressedStream = new GZipStream(createConfigStream, CompressionMode.Compress);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(compressedStream, precomp);
            compressedStream.Close();
            createConfigStream.Close();
        }
        private static void CheckAndFixFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        
    }
}
