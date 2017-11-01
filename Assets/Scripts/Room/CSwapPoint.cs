using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HideAndSeek {
	public class CSwapPoint : CObjectController {

		#region Fields

		[Header("Info")]
		public string swapRoom;

		#endregion

		#region Main methods

		public override void ActivedObject() {
			base.ActivedObject ();
			if (this.m_DidActive == true || string.IsNullOrEmpty (this.swapRoom))
				return;
			CGameManager.Instance.LoadRoom (this.swapRoom);
			this.m_DidActive = true;
		}

		#endregion

		#region Getter && Setter

		public override Vector3 GetClosestPoint (Vector3 value) {
			return this.m_Transform.position;
		}

		#endregion
		
	}
}
