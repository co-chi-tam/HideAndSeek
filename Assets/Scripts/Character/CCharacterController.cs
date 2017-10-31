using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HideAndSeek {
	[RequireComponent(typeof(NavMeshAgent))]
	public class CCharacterController : CObjectController {

		#region Fields

		protected NavMeshAgent m_NavMeshAgent;

		#endregion

		#region Implementation MonoBehaviour

		protected override void Awake() {
			base.Awake ();
			// NavMeshAgent
			this.m_NavMeshAgent = this.GetComponent<NavMeshAgent> ();
		}

		protected override void Update ()
		{
			base.Update ();
			this.UpdateMovable ();
			this.UpdateTargetObject ();
		}

		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			var speed = this.m_NavMeshAgent.velocity.magnitude / this.m_NavMeshAgent.speed;
			this.SetAnimation ("MoveSpeed", speed);
		}

		#endregion

		#region Main methods

		public virtual void UpdateMovable() {
			
		}

		public virtual void UpdateTargetObject() {
		
		}

		#endregion

		#region Getter && Setter 

		public virtual void SetTargetPosition(Vector3 value) {
			value.y = 0f;
			this.m_NavMeshAgent.destination = value;
		}

		public virtual Vector3 GetTargetPosition() {
			return this.m_NavMeshAgent.destination;
		}

		#endregion
		
	}
}
