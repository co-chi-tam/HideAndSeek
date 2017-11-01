using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSingleton;

namespace HideAndSeek {
	public class CGameManager : CMonoSingleton<CGameManager> {

		#region Fields

		public enum EGameState: int {
			WaitingState = 0,
			StartGameState = 1,
			UpdateGameState = 2,
			EndGameState = 3
		}

		[Header("Room")]
		[SerializeField]	protected string[] m_RoomList;

		[Header("Main Character")]
		[SerializeField]	protected CCharacterController m_PrefabCharacter;
		[SerializeField]	protected CCharacterController m_MainCharacter;
		[SerializeField]	protected string m_CurrentRoom;
		[SerializeField]	protected string m_PreviousRoom;

		[Header("Hidden Character")]
		[SerializeField]	protected CCharacterController m_PrefabHiddenCharacter;
		[SerializeField]	protected CCharacterController m_HiddenCharacter;
		[SerializeField]	protected string m_HiddenRoom;

		[Header("Game State")]
		[SerializeField]	protected EGameState m_GameState = EGameState.WaitingState;

		[Header("UI Game")]
		[SerializeField]	protected CUIGameManager m_UIManager;

		protected EGameState m_NextGameState = EGameState.WaitingState;
		protected bool m_StateUpdated = false;

		#endregion

		#region Implementation MonoBehaviour

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
			CSceneManager.Instance.activeSceneChanged -= OnSceneWasLoaded;
			CSceneManager.Instance.activeSceneChanged += OnSceneWasLoaded;
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

		#endregion

		#region Game State

		protected virtual void OnSceneWasLoaded(Scene old, Scene cur) {
			this.m_GameState = this.m_NextGameState;
			this.m_StateUpdated = false;
			this.m_UIManager.SetUIState (this.m_GameState);
		}

		public virtual void StartGameState() {
			if (this.m_NextGameState == EGameState.WaitingState) {
				CSceneManager.Instance.LoadScene (this.m_CurrentRoom);
				this.m_NextGameState = EGameState.StartGameState;
				this.m_StateUpdated = true;
			}
		}

		protected virtual void UpdateWaitingState() {
			
		}

		protected virtual void UpdateStartGameState() {
			if (this.m_MainCharacter != null 
				|| this.m_HiddenCharacter != null)
				return;
			// Instantiate main character
			var startPoint = Vector3.zero;
			this.m_MainCharacter = Instantiate (this.m_PrefabCharacter);
			this.m_MainCharacter.SetPosition (startPoint);
			this.m_MainCharacter.SetActive (true);
			// Dont destroy main character
			DontDestroyOnLoad (this.m_MainCharacter.gameObject);

			// Instantiate hidden character
			this.m_HiddenCharacter = Instantiate (this.m_PrefabHiddenCharacter);
			this.m_HiddenCharacter.SetPosition (startPoint);
			this.m_HiddenCharacter.SetActive (false);
			// Dont destroy main character
			DontDestroyOnLoad (this.m_HiddenCharacter.gameObject);

			// Change state
			this.m_NextGameState = EGameState.UpdateGameState;
			this.m_GameState = this.m_NextGameState;
			this.m_StateUpdated = true;
		}

		protected virtual void UpdateUpdateGameState() {
			if (this.m_StateUpdated == false) {
				// Load main character
				if (this.m_MainCharacter != null) {
					var swapPoint = CRoom.Instance.GetSwapPoint (this.m_PreviousRoom);
					if (swapPoint != null) {
						var startPoint = swapPoint.GetClosestPoint (this.m_MainCharacter.GetPosition ());
						this.m_MainCharacter.SetPosition (startPoint);
						this.m_MainCharacter.SetActive (true);
					}
				}
				// Load hidden character
				if (this.m_HiddenCharacter != null) {
					var currentRoom = CRoom.Instance;
					if (currentRoom.roomSceneName == this.m_HiddenRoom) {
						var hiddenPoint = currentRoom.hiddenPoints [(int)Time.time % currentRoom.hiddenPoints.Length];
						this.m_HiddenCharacter.SetPosition (hiddenPoint.transform.position);
						this.m_HiddenCharacter.SetActive (true);
					}
				}
				this.m_StateUpdated = true;
			}
		}

		protected virtual void UpdateEndGameState() {
			Destroy (this.m_MainCharacter.gameObject);
		}

		#endregion

		#region Character 

		public virtual bool DidCatched() {
			return false;
		}

		public virtual void LoadRoom(string name) {
			if (this.m_CurrentRoom == name)
				return;
			this.m_MainCharacter.SetActive (false);
			this.m_HiddenCharacter.SetActive (false);

			CSceneManager.Instance.LoadScene (name);
			this.m_PreviousRoom = this.m_CurrentRoom;
			this.m_CurrentRoom = name;
		}

		public virtual void LoadHiddenRoom() {
			for (int i = 0; i < this.m_RoomList.Length; i++) {
				var random = Random.Range (0, this.m_RoomList.Length);
				var room = this.m_RoomList [random];
				if (room != this.m_HiddenRoom) {
					this.m_HiddenRoom = room;
					break;
				}
			}
		}

		#endregion

	}
}
