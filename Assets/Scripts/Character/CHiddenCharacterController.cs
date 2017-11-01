using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CHiddenCharacterController : CMainCharacterController {

		#region Fields

		protected bool m_DidHidden = false;

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
			this.m_DidHidden = false;
		}

		#endregion

		#region Main methods

		public override void ActivedObject () {
			base.ActivedObject ();
			if (this.m_DidHidden == true)
				return;
			if (CGameManager.Instance.DidCatched ())
				return;
			var swapPoints = CRoom.Instance.swapPoints;
			var random = (int) Time.time % swapPoints.Length;
			var swapPointCtrl = swapPoints [random];

			this.m_CurrentTargetObject = swapPointCtrl;
			var closestPoint = swapPointCtrl.GetClosestPoint (this.GetPosition());
			this.SetTargetPosition (closestPoint);

			this.m_DidHidden = true;
		}

		public override void UpdateMovable () {
			// TODO
		}

		public override void UpdateTargetObject () {
			if (this.m_CurrentTargetObject == null)
				return;
			var closestPoint = this.m_CurrentTargetObject.GetClosestPoint (this.GetPosition());
			var direction = closestPoint - this.GetPosition ();
			if (direction.sqrMagnitude <= 0.1f) {
				this.SetActive (false);
				this.m_DidHidden = false;
				CGameManager.Instance.LoadHiddenRoom ();
			}
#if UNITY_EDITOR
			Debug.DrawLine(this.transform.position, this.m_CurrentTargetObject.transform.position);
#endif
		}

		#endregion

		#region Getter && Setter

		#endregion
		
	}
}
