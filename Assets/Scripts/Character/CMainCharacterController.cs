using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CMainCharacterController : CCharacterController {

		#region Fields

		[Header("Target")]
		[SerializeField]	protected LayerMask m_TargetLayerMask;
		[SerializeField]	protected CObjectController m_CurrentTargetObject;

		#endregion

		#region Implementation MonoBehaviour

		protected override void Awake ()
		{
			base.Awake ();
		}

		#endregion

		#region Main methods

		public override void UpdateMovable () {
			base.UpdateMovable ();
			if (Input.GetMouseButtonUp (0)) {
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
#if UNITY_EDITOR
			Debug.DrawLine(this.transform.position, this.m_CurrentTargetObject.transform.position);
#endif
		}

		#endregion

		#region Getter && Setter 

		public override void SetActive (bool value)
		{
			base.SetActive (value);
			this.gameObject.SetActive (value);
		}

		public override void SetPosition (Vector3 value)
		{
			base.SetPosition (value);
			this.m_NavMeshAgent.transform.position = value;
		}

		#endregion

	}
}
