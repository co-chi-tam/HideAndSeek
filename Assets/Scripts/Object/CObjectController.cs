using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CObjectController : MonoBehaviour {

		#region Fields

		[Header("Info")]
		public string objectName;

		[Header("Collider")]
		[SerializeField]	protected Collider m_Collider;

		[Header("Animator")]
		[SerializeField]	protected Animator m_Animator;

		protected bool m_DidActive;
		protected Transform m_Transform;

		#endregion

		#region Implementation MonoBehaviour

		protected virtual void Awake() {
			this.m_Transform = this.transform;
			this.m_DidActive = false;
		}

		protected virtual void Start() {
		
		}

		protected virtual void Update() {
		
		}

		protected virtual void LateUpdate() {

		}

		#endregion

		#region Main methods

		public virtual void ActivedObject() {

		}

		#endregion

		#region Getter && Setter

		public virtual void SetActive(bool value) {
			this.m_DidActive = value;
		}

		public virtual bool GetActive() {
			return this.m_DidActive;
		}

		public virtual void SetAnimation (string name, object param = null)
		{
			if (this.m_Animator == null)
				return;
			if (param is int) {
				this.m_Animator.SetInteger (name, (int)param);
			} else if (param is bool) {
				this.m_Animator.SetBool (name, (bool)param);
			} else if (param is float) {
				this.m_Animator.SetFloat (name, (float)param);
			} else if (param == null) {
				this.m_Animator.SetTrigger (name);
			}
		}

		public virtual void SetPosition(Vector3 value) {
			if (this.m_Transform == null)
				return;
			this.m_Transform.position = value;
		}

		public virtual Vector3 GetPosition() {
			if (this.m_Transform == null)
				return Vector3.zero;
			var result = this.m_Transform.position;
			return result;
		}

		public virtual Vector3 GetClosestPoint(Vector3 value) {
			if (this.m_Collider == null)
				return this.m_Transform.position;
			var result = this.m_Collider.bounds.ClosestPoint (value);
			return result;
		}

		#endregion
		
	}
}
