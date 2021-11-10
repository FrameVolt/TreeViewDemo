using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace UnityEditor.TreeViewExamples
{

	[Serializable]
	internal class ObstacleTreeElement : TreeElement
	{
		//public float floatValue1, floatValue2, floatValue3;
		public GameObject gameObject;
		public string filePath;
		//public string text = "";
		public bool enabled;

		public ObstacleTreeElement (string name, int depth, int id, GameObject obj, string filePath) : base (name, depth, id)
		{
			//floatValue1 = Random.value;
			//floatValue2 = Random.value;
			//floatValue3 = Random.value;
			gameObject = obj;
			this.filePath = filePath;
			enabled = true;
		}
	}
}
