using UnityEngine;
using UnityEditor;

public class QuestAsset
{
	[MenuItem("Assets/Create/QuestAsset")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<CQuest> ();
	}
}