using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HideAndSeek {
	public class CMainCharacterController : CCharacterController {

		#region Fields

		[Header("Target")]
		[SerializeField]	protected LayerMask m_TargetLayerMask;
		[SerializeField]	protected CObjectController m_TargetObject;

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
			if (this.IsPointerUp (0)) {
				var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast (ray, out hitInfo, 100f, this.m_TargetLayerMask)) {
					var objCtrl = CRoom.Instance.DetectObject (hitInfo.collider.gameObject);
					if (objCtrl == null) {
						this.m_TargetObject = null;
						this.SetTargetPosition (hitInfo.point);
					} else {
						var closestPoint = objCtrl.GetClosestPoint (this.GetPosition());
						this.m_TargetObject = objCtrl;
						this.SetTargetPosition (closestPoint);
					}
				}
			}
		}


		private bool isUITouched = false;
		private bool IsPointerUp(int index) {
#if UNITY_EDITOR
			if (Input.GetMouseButtonUp(index)) {
				isUITouched = EventSystem.current.IsPointerOverGameObject();
				return true && !isUITouched;
			}
#elif UNITY_ANDROID
			if (Input.touchCount != 1) 
				return false;
			var fingerTouch = Input.GetTouch (index);
			switch (fingerTouch.phase) {
			case TouchPhase.Began:
				isUITouched = EventSystem.current.IsPointerOverGameObject (fingerTouch.fingerId);
				return false;
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
				return true && !isUITouched;
			}
#endif
			return false;
		}

		public override void UpdateTargetObject () {
			base.UpdateTargetObject ();
			if (this.m_TargetObject == null)
				return;
			var closestPoint = this.m_TargetObject.GetClosestPoint (this.GetPosition());
			var direction = closestPoint - this.GetPosition ();
			if (direction.sqrMagnitude <= 0.1f) {
				this.m_TargetObject.ActivedObject ();
				this.m_TargetObject = null;
			}
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
