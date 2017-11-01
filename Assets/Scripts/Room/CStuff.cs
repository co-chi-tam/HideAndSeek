using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CStuff : CObjectController {

		#region Fields

		[Header("Info")]
		public string stuffName;

		[Header("Target point")]
		public GameObject targetPoint;

		#endregion

		#region Main methods

		public override void ActivedObject() {
			base.ActivedObject ();
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
