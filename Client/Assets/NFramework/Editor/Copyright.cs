//=====================================================
// - FileName :      Copyright.cs
// - UserName :      Gning
// - Create by:      2020/06/11
// - Email    :      wuming0108@sina.com
// - Description:   
// -  (C) Copyright 2020, Gning,Inc.
// -  All Rights Reserved.
//======================================================
using UnityEngine;
using System.Collections;
using System.IO;

namespace NFramework
{
    public class Copyright : UnityEditor.AssetModificationProcessor
    {
        private const string AuthorName = "Gning";
        private const string AuthorEmail = "wuming0108@sina.com";

        private const string DateFormat = "yyyy/MM/dd";
        private const string DateFormat2 = "yyyy";
        private static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string allText = File.ReadAllText(path);
                allText = allText.Replace("#AuthorName#", AuthorName);
                allText = allText.Replace("#AuthorEmail#", AuthorEmail);
                allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString(DateFormat));
                allText = allText.Replace("#CopyrightTime#", System.DateTime.Now.ToString(DateFormat2));
                File.WriteAllText(path, allText);
                UnityEditor.AssetDatabase.Refresh();
            }

        }
    }   
}

