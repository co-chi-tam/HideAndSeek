using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HideAndSeek {
	public class CSwapPoint : CObjectController {

		[Header("Info")]
		public string swapRoom;

		public override void ActivedObject() {
			base.ActivedObject ();
			if (this.m_DidActive == true || string.IsNullOrEmpty (this.swapRoom))
				return;
			CGameManager.Instance.LoadRoom (this.swapRoom);
			this.m_DidActive = true;
		}

		public override Vector3 GetClosestPoint (Vector3 value)
		{
			return this.m_Transform.position;
		}
		
	}
}
