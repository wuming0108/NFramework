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
		/// key  ��ǰ�ļ���·��
		/// value ���ĸ�ab������
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
		/// ��ͬһ��ab����Դ����һ��
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
		/// �����е�������Ҫ����ab ��װ��һ���µ�ab����
		/// </summary>
		/// <param name="abs"></param>
		/// ������ ����װ
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
