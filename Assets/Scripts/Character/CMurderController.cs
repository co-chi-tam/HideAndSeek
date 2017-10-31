using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CMurderController : CCharacterController {

		[Header("Target")]
		[SerializeField]	protected LayerMask m_TargetLayerMask;
		[SerializeField]	protected CObjectController m_CurrentTargetObject;

		protected override void Awake ()
		{
			base.Awake ();
			this.m_DidActive = true;
		}

		public override void UpdateMovable () {
			base.UpdateMovable ();
			if (Input.GetMouseButtonDown (0)) {
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast (ray, out hitInfo, 100f, this.m_TargetLayerMask)) {
					var objCtrl = CRoom.Instance.DetectObject (hitInfo.collider.gameObject);
					if (objCtrl == null) {
						this.m_CurrentTargetObject = null;
						this.SetTargetPosition (hitInfo.point);
					} else {
						this.m_CurrentTargetObject = objCtrl;
						var closestPoint = objCtrl.GetClosestPoint (hitInfo.point);
						this.SetTargetPosition (closestPoint);
					}
				}
			}
		}

		public override void UpdateTargetObject ()
		{
			base.UpdateTargetObject ();
			if (this.m_CurrentTargetObject == null)
				return;
			var closestPoint = this.m_CurrentTargetObject.GetClosestPoint (this.GetPosition());
			var direction = closestPoint - this.GetPosition ();
			if (direction.sqrMagnitude <= 0.1f) {
				this.m_CurrentTargetObject.ActivedObject ();
			}
		}

	}
}
