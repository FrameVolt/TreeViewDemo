using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using System.IO;
using System.Collections;

namespace UnityEditor.TreeViewExamples
{

	static class ObstacleTreeElementGenerator
	{
		static int IDCounter;
		static int minNumChildren = 5;
		static int maxNumChildren = 10;
		static float probabilityOfBeingLeaf = 0.5f;


		public static List<ObstacleTreeElement> GenerateEmptyTree()
		{
			IDCounter = 0;
			var treeElements = new List<ObstacleTreeElement>();
			var root = new ObstacleTreeElement("Root", -1, IDCounter, null, "");
			treeElements.Add(root);

			return treeElements;
		}

		public static List<ObstacleTreeElement> GetDraggingElements(TreeModel<ObstacleTreeElement> treeModel)
        {
			List<ObstacleTreeElement> treeElements = new List<ObstacleTreeElement>();

			foreach (Object obj in DragAndDrop.objectReferences)
			{
				if (obj.GetType() == typeof(GameObject))
				{
					treeElements.Add(CreateElement(treeModel, obj as GameObject));
				}
				else if(IsAssetAFolder(obj))
                {
					var folderPath = AssetDatabase.GetAssetPath(obj);

					string[] files = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

					foreach (var absolutepath in files)
                    {
						var prefab = AssetDatabase.LoadAssetAtPath(absolutepath, typeof(GameObject));
						treeElements.Add(CreateElement(treeModel, prefab as GameObject));
					}
					
				}
			}

			return treeElements;
		}

		//private static Object GetSubAssetsRecursive()
		//      {

		//      }

		public static T[] GetAtPath<T>(string path)
		{
			ArrayList al = new ArrayList();
			string[] fileEntries = Directory.GetFiles(path);
			foreach (string fileName in fileEntries)
			{
				int index = fileName.LastIndexOf("/");
				string localPath = path.Replace(Application.dataPath, "Assets");

				if (index > 0)
					localPath += fileName.Substring(index);

				Object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

				if (t != null)
					al.Add(t);
			}
			T[] result = new T[al.Count];
			for (int i = 0; i < al.Count; i++)
				result[i] = (T)al[i];

			return result;
		}

		private static bool IsAssetAFolder(Object obj)
		{
			string path = "";

			if (obj == null)
			{
				return false;
			}

			path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

			if (path.Length > 0)
			{
				if (Directory.Exists(path))
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			return false;
		}


		private static ObstacleTreeElement CreateElement(TreeModel<ObstacleTreeElement> treeModel, GameObject obj)
		{
			int depth = 0;
			int id = treeModel.GenerateUniqueID();
			var filePath = AssetDatabase.GetAssetPath(obj);
			var element = new ObstacleTreeElement(obj.name, depth, id, obj, filePath);
			return element;
		}

		public static List<ObstacleTreeElement> GenerateRandomTree(int numTotalElements)
		{
			int numRootChildren = numTotalElements / 4;
			IDCounter = 0;
			var treeElements = new List<ObstacleTreeElement>(numTotalElements);

			var root = new ObstacleTreeElement("Root", -1, IDCounter, null, "");
			treeElements.Add(root);
			for (int i = 0; i < numRootChildren; ++i)
			{
				int allowedDepth = 6;
				AddChildrenRecursive(root, Random.Range(minNumChildren, maxNumChildren), true, numTotalElements, ref allowedDepth, treeElements);
			}

			return treeElements;
		}
		static void AddChildrenRecursive(TreeElement element, int numChildren, bool force, int numTotalElements, ref int allowedDepth, List<ObstacleTreeElement> treeElements)
		{
			if (element.depth >= allowedDepth)
			{
				allowedDepth = 0;
				return;
			}

			for (int i = 0; i < numChildren; ++i)
			{
				if (IDCounter > numTotalElements)
					return;

				var child = new ObstacleTreeElement("Element " + IDCounter, element.depth + 1, ++IDCounter, null, "");
				treeElements.Add(child);

				if (!force && Random.value < probabilityOfBeingLeaf)
					continue;

				AddChildrenRecursive(child, Random.Range(minNumChildren, maxNumChildren), false, numTotalElements, ref allowedDepth, treeElements);
			}
		}
	}
}
