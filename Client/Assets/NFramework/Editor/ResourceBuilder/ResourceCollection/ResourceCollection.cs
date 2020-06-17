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
        [MenuItem("AssetBundle/Android AB", false)]

        public static void BuildAndroidAB()
        {
            CollectionDependents();
        }



        public static void CollectionDependents()
        {  
            string[] paths = Directory.GetDirectories(AB_PATH, "*", SearchOption.TopDirectoryOnly);
       
            for (int i = 0; i < paths.Length; ++i)
            { 
                string[] paths2 = Directory.GetDirectories(paths[i], "*", SearchOption.AllDirectories);

                for (int j = 0; j < paths2.Length; j++)
                {
                    CollectionResourceInfo(paths2[j],0);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="layer">�������ĵڼ��� </param>
        public static bool CollectionResourceInfo(string path,int layer)
        {
            // ������ϵ����3�� ֱ�ӱ��� ������Դ
            if (layer >= 3)
            {
                Debug.LogError("Dependencies Layer Depth over 3 : " + path);
                return false;
            }

            //��������������ڴ����·������ ����ֱ�Ӻ���
            if (layer > 0 && path.Contains(AB_PATH))
            { 
                return true;
            }

            // ���ݹ��� ��ȡab����
            string abName = GetABNameFromPath(path);

            CollectionPools.CollectionData(path, abName);

            string[] files = AssetDatabase.GetDependencies(path, true);

            for (int i = 0; i < files.Length; i++)
            { 
                bool error = CollectionResourceInfo(files[i], layer + 1);

                if (!error)
                {
                    Debug.LogError("Dependencie Error : " + path);
                    return false;
                }
            } 
            return true;
        }

  
        /// <summary>
        /// �����ͼ���Ļ� ֱ����ͼ��������ab��
        /// </summary>
        /// <param name="path"></param>
        /// <param name="abName"></param>
        /// <param name="isForder"></param>
        /// <returns></returns>
        public static string GetABNameFromPath(string path)
        {
            var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            
            //�����ͼ�� �Ͱ�ͬ����ͼ������һ��ab����

            if (textureImporter != null )
            {
                if (textureImporter.textureType == TextureImporterType.Sprite)
                {
                    if (!string.IsNullOrEmpty(textureImporter.spritePackingTag))
                        return "atlas_" + textureImporter.spritePackingTag;
                }
            }
             
            return FileUtility.GetABNameFromPath(path);
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

