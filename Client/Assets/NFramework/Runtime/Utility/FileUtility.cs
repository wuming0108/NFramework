//=====================================================
// - FileName :      FileUtility.cs
// - UserName :      Gning
// - Create by:      2020/06/11
// - Email    :      wuming0108@sina.com
// - Description:   
// -  (C) Copyright 2020, Gning,Inc.
// -  All Rights Reserved.
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace NFramework
{
    public class FileUtility
    {

        public const string AB_PATH = "Assets/GameResources";
        public static string AssetBundleExt = ".dat";

        /// <summary>
        /// ab名字为第三级目录加上第四级的目录或者文件名
        /// Assets/GameResources/UI/Main.prefab的ab 是 ui_main
        /// Assets/GameResources/UI/Shop/UIShop.prefab的ab 是 ui_shop
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetABNameFromPath(string path)
        {
            string[] forders = path.ToLower().Split('/');

            if (forders.Length < 4)
            {
                Debug.LogError("Path is Error : " + path);
                return "";
            }
            if (forders.Length == 4)
            {
                forders[3] = forders[3].Substring(0, forders[3].LastIndexOf("."));
            }

            return forders[2] + "_" + forders[3];
        }
        /*
       获取文件夹下所有的子目录
       返回的结果是相对路径
        */
        public static void GetAllDirs(string basePath, List<string> result, bool includeSubDir = false)
        {
            DirectoryInfo dir = new DirectoryInfo(basePath);
            if (!dir.Exists)
            {
                return;
            }
            _getAllDirs(dir, "", result, includeSubDir);
        }

        private static void _getAllDirs(DirectoryInfo dir, string prePath, List<string> result, bool includeSubDir)
        {
            if (!dir.Exists)
            {
                return;
            }
            string head = prePath;
            if (head.Length > 0)
            {
                head += "/";
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            for (int i = 0; i < dirs.Length; ++i)
            {
                result.Add(head + dirs[i].Name);
                if (includeSubDir)
                {
                    _getAllDirs(dirs[i], head + dirs[i].Name, result, includeSubDir);
                }
            }
        }

        /*
        获取所有的文件
        返回的结果是相对文件名
        extList为后缀名的列表 *.txt|*.png
        */
        public static void GetAllFiles(string basePath, bool inChildDir, string extList, List<string> result)
        {
            DirectoryInfo dir = new DirectoryInfo(basePath);
            if (!dir.Exists)
            {
                return;
            }
            List<string> ext = new List<string>();
            if (extList.Length > 0)
            {
                string[] strs = extList.Split('|');
                for (int i = 0; i < strs.Length; ++i)
                {
                    ext.Add(strs[i]);
                }
            }
            _getAllFiles(dir, inChildDir, "", ext, result);
        }

        private static void _getAllFiles(DirectoryInfo dir, bool inChildDir, string prePath, List<string> extList, List<string> result)
        {
            if (!dir.Exists)
            {
                return;
            }
            string head = prePath;
            if (head.Length > 0)
            {
                head += "/";
            }
            if (extList.Count > 0)
            {
                for (int i = 0; i < extList.Count; ++i)
                {
                    FileInfo[] files = dir.GetFiles(extList[i]);
                    for (int j = 0; j < files.Length; ++j)
                    {
                        result.Add(head + files[j].Name);
                    }
                }
            }
            else
            {
                FileInfo[] files = dir.GetFiles();
                for (int j = 0; j < files.Length; ++j)
                {
                    result.Add(head + files[j].Name);
                }
            }
            if (inChildDir)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                for (int i = 0; i < dirs.Length; ++i)
                {
                    _getAllFiles(dirs[i], inChildDir, head + dirs[i].Name, extList, result);
                }
            }
        }

        public static string MD5EncryptFile(string fileName, bool upper = true)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                string ret = sb.ToString();
                return upper ? ret.ToUpper() : ret.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        public static string MD5EncryptString(string str, bool upper = true)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            MD5 md5Hash = new MD5CryptoServiceProvider();
            byte[] b = Encoding.UTF8.GetBytes(str);

            //Debug.Log("md5:" + str);
            byte[] data = md5Hash.ComputeHash(b);
            md5Hash.Clear();
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string ret = sBuilder.ToString();
            if (ret.Length != 32)
            {
                Debug.LogError("md5 error");
            } 
            if (upper)
            {
                return ret.ToUpper();
            }
            return ret;
        }
    }   
}

