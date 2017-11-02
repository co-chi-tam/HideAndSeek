using System;
using UnityEngine;

public class CQuest : ScriptableObject, ICloneable{

	[Header("Info")]
	public string questName;
	public string questDescription;
	[Header("Detail")]
	public string questReceiveRoomName;
	public string questRoomName;
	public string questStuffName;
	public string questStuffItemName;

	public object Clone() {
		return this.MemberwiseClone ();
	}

}
