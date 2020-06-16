//=====================================================
// - FileName :      CollectionPools.cs
// - UserName :      Gning
// - Create by:      2020/06/16
// - Email    :      wuming0108@sina.com
// - Description:   
// -  (C) Copyright 2020, Gning,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NFramework.Editor
{ 

	public class CollectionPools
	{
		/// <summary>
		/// key  当前文件的路径
		/// value 被哪个ab所依赖
		/// </summary>
		public static Dictionary<string, List<string>> m_ResourceDependenceDatas = new Dictionary<string, List<string>>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="abName"></param>
		public static void CollectionData(string path,string abName)
		{ 
			AddToDictionary(m_ResourceDependenceDatas, path, abName);			 
		}


		/// <summary>
		/// 把同一个ab的资源整理到一起
		/// key : abName
		/// value : file list
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, List<string>> GetCollectionData()
		{ 
			Dictionary<string, List<string>> abDatas = new Dictionary<string, List<string>>();
			 
			foreach (var item in m_ResourceDependenceDatas)
			{  
				AddToDictionary(abDatas, GetABName(item.Value), item.Key);
			} 
			return abDatas;
		}



		private static void AddToDictionary(Dictionary<string, List<string>> datas,string key, string value)
		{ 
			if (!datas.ContainsKey(key))
				datas.Add(key, new List<string>());
			datas[key].Add(value);
		}

		/// <summary>
		/// 把所有的依赖需要他的ab 组装成一个新的ab名称
		/// </summary>
		/// <param name="abs"></param>
		/// 先排序 后组装
		/// <returns></returns>
		private static string GetABName(List<string> abs)
		{ 
			abs.Sort((a, b) => string.Compare(a, b));

			string abName = abs[0];

			for (int i = 1; i < abs.Count; i++)
			{
				abName += "_" + abs[i];
			}

			return abName;
		}
	}
}
