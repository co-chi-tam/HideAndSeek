using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CStuff : CObjectController {

		#region Fields

		[Header("Info")]
		public string stuffName;
		public string[] stuffContain;

		[Header("Target point")]
		public GameObject targetPoint;

		#endregion

		#region Main methods

		public override void ActivedObject() {
			base.ActivedObject ();
			if (this.stuffContain.Length == 0)
				return;
			CGameManager.Instance.ShowItem (this.stuffContain [0], (itemName) => {
				
			}, () => {
				
			});
			this.m_DidActive = true;
		}

		#endregion

		#region Getter && Setter

		public override Vector3 GetClosestPoint (Vector3 value) {
			if (this.targetPoint == null)
				return base.GetClosestPoint (value);
			return this.targetPoint.transform.position;
		}

		#endregion

	}
}
