using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.TreeViewExamples
{
	
	[CreateAssetMenu (fileName = "ObstacleTreeAsset", menuName = "Obstacle Tree Asset", order = 1)]
	public class ObstacleTreeAsset : ScriptableObject
	{
		[SerializeField] List<ObstacleTreeElement> m_TreeElements = new List<ObstacleTreeElement> ();

		internal List<ObstacleTreeElement> treeElements
		{
			get { return m_TreeElements; }
			set { m_TreeElements = value; }
		}

		void Awake ()
		{
			if (m_TreeElements.Count == 0)
				m_TreeElements = ObstacleTreeElementGenerator.GenerateEmptyTree();
		}
	}
}
