//=====================================================
// - FileName :      ResourceCollection.cs
// - UserName :      Gning
// - Create by:      2020/06/11
// - Email    :      wuming0108@sina.com
// - Description:   
// -  (C) Copyright 2020, Gning,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using NFramework;
namespace NFramework.Editor
{
    public class ResourceCollection 
    {
        public const string AB_PATH = "Assets/GameResources";
        [MenuItem("AssetBundle/资源收集", false)]
        public static void CollectionDependents()
        {  
            string[] paths = Directory.GetDirectories(AB_PATH, "*", SearchOption.TopDirectoryOnly);
       
            for (int i = 0; i < paths.Length; ++i)
            { 
                string[] paths2 = Directory.GetDirectories(paths[i], "*", SearchOption.AllDirectories);

                for (int j = 0; j < paths2.Length; j++)
                {
                    CollectionABResource(paths2[j]);
                }
            }
        }



        public static void CollectionABResource(string path)
        {
            if (path.EndsWith(".meta"))
                return;
            string abName = FileUtility.GetABNameFromPath(path);
            CollectionPools.CollectionData(path, abName); 

            string[] files = AssetDatabase.GetDependencies(path, true);

            for (int i = 0; i < files.Length; i++)
                CollectionABResource(files[i]);
        }


        /// <summary>
        /// 如果是图集的话 直接用图集名称做ab名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="abName"></param>
        /// <param name="isForder"></param>
        /// <returns></returns>
        public static bool CollectionAltasResource(string path, string abName)
        {
            var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            if (textureImporter == null)
                return false;

            if (textureImporter.textureType == TextureImporterType.Sprite)
                return false;

            return true;
        }


        public static void BuildAssetBundles(BuildTarget targetPlatform, string outPath)
        {

            Dictionary<string, List<string>> abData = CollectionPools.GetCollectionData();
             
            List<AssetBundleBuild> abLists = new List<AssetBundleBuild>();

            foreach (var item in abData)
            {
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = item.Key.ToLower();
                build.assetBundleVariant = FileUtility.AssetBundleExt;
                build.assetNames = item.Value.ToArray(); 
                abLists.Add(build);
            } 

            BuildPipeline.BuildAssetBundles(outPath, abLists.ToArray(), BuildAssetBundleOptions.None, targetPlatform);//LZMA

        }






    }   
}

