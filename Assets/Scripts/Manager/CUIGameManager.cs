using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CUIGameManager : MonoBehaviour {

		#region Fields

		[Header("Start game panel")]
		[SerializeField] 	protected GameObject m_StartGamePanel;

		[Header("Ingame panel")]
		[SerializeField] 	protected GameObject m_InGamePanel;

		#endregion

		#region Main methods

		public virtual void SetUIState(CGameManager.EGameState state) {
			this.m_StartGamePanel.SetActive (state == CGameManager.EGameState.WaitingState);
//			this.m_InGamePanel.SetActive (state == CGameManager.EGameState.StartGameState 
//				|| state == CGameManager.EGameState.UpdateGameState);
			this.m_InGamePanel.SetActive (false);
		}

		#endregion
		
	}
}
