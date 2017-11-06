using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CSwapPoint : CObjectController {

		#region Fields

		[Header("Swap info")]
		public string swapRoom;

		#endregion

		#region Main methods

		public override void ActivedObject() {
			base.ActivedObject ();
			if (string.IsNullOrEmpty (this.swapRoom))
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
