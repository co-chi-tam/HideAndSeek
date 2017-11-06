using System;
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

		[Header("Quest")]
		[SerializeField]	protected ScriptableObject[] m_QuestAssets;
		[SerializeField]	protected int m_QuestIndex = 0;
		[SerializeField]	protected bool m_IsQuestCompleted = false;
		[SerializeField]	protected string m_QuestRoomName = "KitchenRoom";
		[SerializeField]	protected string m_QuestStuffName = "Kitchen";
		[SerializeField]	protected string m_QuestStuffItem = "Knife";

		[Header("Main Character")]
		[SerializeField]	protected CCharacterController m_PrefabCharacter;
		[SerializeField]	protected CCharacterController m_MainCharacter;
		[SerializeField]	protected string m_CurrentRoom;
		[SerializeField]	protected string m_PreviousRoom;
		[SerializeField]	protected string m_InventoryStuff;

		[Header("Hidden Character")]
		[SerializeField]	protected CCharacterController m_PrefabHiddenCharacter;
		[SerializeField]	protected CCharacterController m_HiddenCharacter;
		[SerializeField]	protected string m_HiddenRoom;
		[SerializeField]	protected UnityEngine.Object[] m_HiddeRoomList;

		[Header("Game State")]
		[SerializeField]	protected EGameState m_GameState = EGameState.WaitingState;

		[Header("UI Game")]
		[SerializeField]	protected CUIGameManager m_UIManager;

		protected EGameState m_NextGameState = EGameState.WaitingState;
		protected bool m_StateUpdated = false;
		protected CQuest m_CurrentQuest;

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
			this.LoadMainCharacter();
			// Instantiate hidden character
			this.LoadHiddenCharacter();
			// Load quest
			this.LoadQuest();
			// Load object info
			this.LoadObjectInfo();
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
				// Load quest
				if (this.m_IsQuestCompleted) {
					this.CompletedQuest ();
				}
				// Load object info
				this.LoadObjectInfo();
				// Update state
				this.m_StateUpdated = true;
			}
		}

		protected virtual void UpdateEndGameState() {
			// Change state
			this.m_NextGameState = EGameState.EndGameState;
			this.m_GameState = this.m_NextGameState;
		}

		#endregion

		#region Object info

		public virtual void LoadObjectInfo() {
			// Load info
			this.m_UIManager.LoadInfoObjects (CRoom.Instance.stuffs);
		}

		#endregion

		#region Character 

		public virtual void LoadMainCharacter() {
			var startPoint = Vector3.zero;
			this.m_MainCharacter = Instantiate (this.m_PrefabCharacter);
			this.m_MainCharacter.SetPosition (startPoint);
			this.m_MainCharacter.SetActive (true);
			// Dont destroy main character
			DontDestroyOnLoad (this.m_MainCharacter.gameObject);
		}

		public virtual void LoadHiddenCharacter() {
			var startPoint = Vector3.zero;
			this.m_HiddenCharacter = Instantiate (this.m_PrefabHiddenCharacter);
			this.m_HiddenCharacter.SetPosition (startPoint);
			this.m_HiddenCharacter.SetActive (false);
			// Dont destroy main character
			DontDestroyOnLoad (this.m_HiddenCharacter.gameObject);
		}

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

		protected virtual void OnSceneWasLoaded(Scene old, Scene cur) {
			this.m_GameState = this.m_NextGameState;
			this.m_StateUpdated = false;
			this.m_UIManager.SetUIState (this.m_GameState);
		}

		public virtual void LoadHiddenRoom() {
			for (int i = 0; i < this.m_HiddeRoomList.Length; i++) {
				var random = UnityEngine.Random.Range (0, this.m_HiddeRoomList.Length);
				var room = this.m_HiddeRoomList [random];
				if (room.name != this.m_HiddenRoom) {
					this.m_HiddenRoom = room.name;
					break;
				}
			}
		}

		#endregion

		#region Item

		public virtual void DropItem(Vector2 pos) {
			var rayPoint = Camera.main.ScreenPointToRay(pos);
			RaycastHit hitInfo;
			if (Physics.Raycast (rayPoint, out hitInfo, 100f)) {
				var room = CRoom.Instance;
				var objCtrl = room.DetectStuffObject (hitInfo.collider.gameObject);
				// RESET QUEST
				this.UpdateQuest (this.IsQuestCompleted (objCtrl));

				this.m_MainCharacter.SetTargetPosition (hitInfo.point);
				this.RemoveItem ();
			}
		}

		public virtual void AddItem(string name) {
			this.m_InventoryStuff = name;
			this.m_UIManager.TakeItem (name);
		}

		public virtual void RemoveItem() {
			this.m_InventoryStuff = string.Empty;
			this.m_UIManager.DroppedItem ();
		}

		public virtual void ShowItem(string name, string displayName, Action<string> submit, Action cancel) {
			if (name == this.m_InventoryStuff
				|| name == this.m_QuestStuffItem) {
				if (cancel != null) {
					cancel ();
				}
				return;
			}
			this.ShowItemPanel (name, displayName, submit, cancel);
		}

		public virtual void ShowQuestItem(Action<string> submit, Action cancel) {
			if (string.IsNullOrEmpty (this.m_QuestStuffItem)) {
				if (cancel != null) {
					cancel ();
				}
				return;
			}
			this.ShowItemPanel (this.m_QuestStuffItem, this.m_QuestStuffItem, submit, cancel);
		}

		private void ShowItemPanel(string name, string displayName, Action<string> submit, Action cancel) {
			this.m_UIManager.ShowItemPanel (name, displayName,
				(itemName) => {
					this.AddItem(name);
					if (submit != null) {
						submit(itemName);
					}
				}, cancel);
		}

		#endregion

		#region Quest

		public virtual bool IsQuestCompleted(CObjectController objCtrl) {
			var room = CRoom.Instance;
			return objCtrl != null
				&& objCtrl.objectName == this.m_CurrentQuest.questStuffName
				&& room.roomSceneName == this.m_CurrentQuest.questRoomName
				&& this.m_InventoryStuff == this.m_CurrentQuest.questStuffItemName;
		}

		public virtual void LoadQuest() {
			this.m_CurrentQuest		= this.m_QuestAssets[this.m_QuestIndex] as CQuest;
			this.m_QuestRoomName 	= this.m_CurrentQuest.questRoomName;
			this.m_QuestStuffName 	= this.m_CurrentQuest.questStuffName;
			this.m_QuestStuffItem 	= this.m_CurrentQuest.questStuffItemName;
			this.m_IsQuestCompleted = false;
			this.m_UIManager.SetQuestInfoText (this.m_CurrentQuest.questDescription);
		}

		public virtual void UpdateQuest(bool result) {
			if (result) {
				this.m_QuestRoomName 	= string.Empty;
				this.m_QuestStuffName 	= string.Empty;
				this.m_QuestStuffItem 	= string.Empty;
				this.m_UIManager.SetQuestInfoText (this.m_CurrentQuest.questCompleteText);
			} else {
//				this.m_UIManager.SetQuestInfoText (this.m_CurrentQuest.questFailText);
			}
			this.m_IsQuestCompleted = result;
		}

		public virtual void CompletedQuest() {
			if (this.m_CurrentQuest == null
				|| this.m_CurrentQuest.questReceiveRoomName != CRoom.Instance.roomSceneName)
				return;
			this.m_QuestIndex = this.m_QuestIndex + 1 >= this.m_QuestAssets.Length 
				? this.m_QuestAssets.Length
				: this.m_QuestIndex + 1;
			if (this.m_QuestIndex >= this.m_QuestAssets.Length) {
				return;
			}
			this.LoadQuest ();
		}

		#endregion

	}
}
