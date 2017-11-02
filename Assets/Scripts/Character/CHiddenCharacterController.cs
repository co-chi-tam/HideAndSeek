using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CHiddenCharacterController : CMainCharacterController {

		#region Fields

		#endregion

		#region Implementation Monobehaviour

		protected override void Awake ()
		{
			base.Awake ();
		}

		#endregion

		#region Main methods

		public override void ActivedObject () {
			base.ActivedObject ();
			if (CGameManager.Instance.DidCatched ())
				return;
			var swapPoints = CRoom.Instance.swapPoints;
			var random = (int) Time.time % swapPoints.Length;
			var swapPointCtrl = swapPoints [random];

			CGameManager.Instance.ShowQuestItem ((itemName) => {
				var closestPoint = swapPointCtrl.GetClosestPoint (this.GetPosition());
				this.m_TargetObject = swapPointCtrl;
				this.SetTargetPosition (closestPoint);
			}, () => {
				var closestPoint = swapPointCtrl.GetClosestPoint (this.GetPosition());
				this.m_TargetObject = swapPointCtrl;
				this.SetTargetPosition (closestPoint);
			});
		}

		public override void UpdateMovable () {
			// TODO
		}

		public override void UpdateTargetObject () {
			if (this.m_TargetObject == null)
				return;
			var closestPoint = this.m_TargetObject.GetClosestPoint (this.GetPosition());
			var direction = closestPoint - this.GetPosition ();
			if (direction.sqrMagnitude <= 0.1f) {
				this.SetActive (false);
				this.m_TargetObject = null;
				CGameManager.Instance.LoadHiddenRoom ();
			}
		}

		#endregion

		#region Getter && Setter

		#endregion
		
	}
}
