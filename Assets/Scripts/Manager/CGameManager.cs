using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSingleton;

namespace HideAndSeek {
	public class CGameManager : CMonoSingleton<CGameManager> {

		public enum EGameState: int {
			WaitingState = 0,
			StartGameState = 1,
			UpdateGameState = 2,
			EndGameState = 3
		}

		[Header("Main Character")]
		[SerializeField]	protected CCharacterController m_PrefabCharacter;
		[SerializeField]	protected CCharacterController m_MainCharacter;
		[SerializeField]	protected string m_CurrentRoom;
		[SerializeField]	protected string m_PreviousRoom;

		[Header("Game State")]
		[SerializeField]	protected EGameState m_GameState = EGameState.WaitingState;
		[SerializeField]	protected bool m_StateUpdated = false;

		protected EGameState m_NextGameState = EGameState.WaitingState;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			// Waiting state
			this.m_GameState = EGameState.WaitingState;
			this.m_NextGameState = EGameState.WaitingState;
			// Start character
			this.m_MainCharacter = null;
			// Loading scene
			SceneManager.activeSceneChanged -= OnSceneWasLoaded;
			SceneManager.activeSceneChanged += OnSceneWasLoaded;
		}

		protected virtual void Start() {
			
		}

		protected virtual void Update() {
			switch (this.m_GameState) {
			case EGameState.WaitingState:
				this.UpdateWaitingState ();
				break;
			case EGameState.StartGameState:
				this.UpdateStartGameState ();
				break;
			case EGameState.UpdateGameState:
				this.UpdateUpdateGameState ();
				break;
			case EGameState.EndGameState:
				this.UpdateEndGameState ();
				break;
			}
		}

		protected virtual void OnSceneWasLoaded(Scene old, Scene cur) {
			this.m_GameState = this.m_NextGameState;
		}

		protected virtual void UpdateWaitingState() {
			if (Input.GetMouseButtonDown (0) 
				&& this.m_NextGameState == EGameState.WaitingState) {
				SceneManager.LoadScene (this.m_CurrentRoom);
				this.m_NextGameState = EGameState.StartGameState;
				this.m_StateUpdated = true;
			}
		}

		protected virtual void UpdateStartGameState() {
			if (this.m_MainCharacter != null)
				return;
			// Instantiate main character
			var startPoint = Vector3.zero;
			this.m_MainCharacter = Instantiate (this.m_PrefabCharacter);
			this.m_MainCharacter.SetPosition (startPoint);
			// Dont destroy main character
			DontDestroyOnLoad (this.m_MainCharacter.gameObject);
			// Change state
			this.m_NextGameState = EGameState.UpdateGameState;
			this.m_GameState = this.m_NextGameState;
			this.m_StateUpdated = true;
		}

		public virtual void LoadRoom(string name) {
			if (this.m_CurrentRoom == name)
				return;
			SceneManager.LoadScene (name);
			this.m_PreviousRoom = this.m_CurrentRoom;
			this.m_CurrentRoom = name;
			this.m_StateUpdated = false;
		}

		protected virtual void UpdateUpdateGameState() {
			if (this.m_MainCharacter != null 
				&& this.m_StateUpdated == false) {
				var swapPoint = CRoom.Instance.GetSwapPoint (this.m_PreviousRoom);
				if (swapPoint != null) {
					var startPoint = swapPoint.GetClosestPoint (this.m_MainCharacter.GetPosition ());
					this.m_MainCharacter.SetPosition (startPoint);
				}
				this.m_StateUpdated = true;
			}
		}

		protected virtual void UpdateEndGameState() {
			Destroy (this.m_MainCharacter.gameObject);
		}



	}
}
