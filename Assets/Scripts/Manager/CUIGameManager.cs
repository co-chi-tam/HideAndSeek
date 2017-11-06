using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UICustomize;

namespace HideAndSeek {
	public class CUIGameManager : MonoBehaviour {

		#region Fields

		[Header("Start game panel")]
		[SerializeField] 	protected GameObject m_StartGamePanel;

		[Header("Ingame panel")]
		[SerializeField] 	protected GameObject m_InGamePanel;
		[SerializeField]	protected GameObject m_ObjectInfoPanel;
		[SerializeField]	protected CUIStuff m_ObjectInfoPrefab;

		[Header("Quest info")]
		[SerializeField]	protected Text m_QuestInfoText;

		[Header("Stuff Item")]
		[SerializeField] 	protected GameObject m_ShowItemPanel;
		[SerializeField]	protected Image m_ItemImage;
		[SerializeField]	protected Button m_SubmitItemButton;
		[SerializeField]	protected Button m_CancelItemButton;

		[Header("Inventory Item")]
		[SerializeField]	protected Image m_InventoryItemImage;
		[SerializeField]	protected UIDrag m_InventoryDragItem;

		protected Sprite[] m_ResourceItemSprites;
		protected List<CUIStuff> m_GoInfoObjectPool;

		#endregion

		#region Implementation MonoBehaviour

		protected virtual void Awake() {
			var resourceImages = Resources.LoadAll<Sprite> ("ItemImages");
			this.m_ResourceItemSprites = resourceImages;
			this.m_GoInfoObjectPool = new List<CUIStuff> ();
		}

		protected virtual void Start() {
			this.DroppedItem ();
		}

		#endregion

		#region Main methods

		public virtual void LoadInfoObjects(CStuff[] values) {
			// Deactive all
			for (int i = 0; i < this.m_GoInfoObjectPool.Count; i++) {
				var objUI = this.m_GoInfoObjectPool [i];
				objUI.gameObject.SetActive (false);
			}
			// Load UI Object pool
			for (int i = 0; i < values.Length; i++) {
				if (i >= this.m_GoInfoObjectPool.Count) {
					var objSpawned = Instantiate (this.m_ObjectInfoPrefab);
					objSpawned.transform.SetParent (this.m_ObjectInfoPanel.transform);
					// Add object pool.
					this.m_GoInfoObjectPool.Add (objSpawned);
				}
				var objUI = this.m_GoInfoObjectPool [i];
				objUI.SetFollowObject (values[i].gameObject);
				objUI.gameObject.SetActive (true);
			}
			this.m_ObjectInfoPrefab.gameObject.SetActive (false);
		}

		public virtual void TakeItem(string name) {
			// Load item image
			for (int i = 0; i < this.m_ResourceItemSprites.Length; i++) {
				if (this.m_ResourceItemSprites [i].name == name) {
					this.m_InventoryItemImage.sprite = this.m_ResourceItemSprites [i];
					break;
				}
			}
			// End drag item
			this.m_InventoryDragItem.enabled = true;
			this.m_InventoryDragItem.OnEventEndDrag = null;
			this.m_InventoryDragItem.OnEventEndDrag += (pos) => {
				CGameManager.Instance.DropItem(pos);
			};
		}

		public virtual void DroppedItem() {
			// End drag item
			this.m_InventoryDragItem.OnEventEndDrag = null;
			this.m_InventoryDragItem.enabled = false;
			this.m_InventoryItemImage.sprite = null;
		}

		public virtual void ShowItemPanel(string name, Action<string> submit, Action cancel) {
			// Active p anel
			this.m_ShowItemPanel.SetActive (true);
			// Load item image
			for (int i = 0; i < this.m_ResourceItemSprites.Length; i++) {
				if (this.m_ResourceItemSprites [i].name == name) {
					this.m_ItemImage.sprite = this.m_ResourceItemSprites [i];
					break;
				}
			}
			// Submit button
			this.m_SubmitItemButton.onClick.RemoveAllListeners ();
			this.m_SubmitItemButton.onClick.AddListener (() => {
				if (submit != null) {
					submit (name);
				}
				this.m_ShowItemPanel.SetActive (false);
			});
			// Cancel button
			this.m_CancelItemButton.onClick.RemoveAllListeners ();
			this.m_CancelItemButton.onClick.AddListener (() => {
				if (cancel != null) {
					cancel ();
				}
				this.m_ShowItemPanel.SetActive (false);
			});
		}

		public virtual void SetUIState(CGameManager.EGameState state) {
			this.m_StartGamePanel.SetActive (state == CGameManager.EGameState.WaitingState);
			this.m_InGamePanel.SetActive (state == CGameManager.EGameState.StartGameState 
				|| state == CGameManager.EGameState.UpdateGameState);
			this.m_ShowItemPanel.SetActive (!this.m_InGamePanel.activeInHierarchy);
		}

		#endregion

		#region Quest

		public virtual void SetQuestInfoText(string text) {
			this.m_QuestInfoText.text = text;
		}

		#endregion
		
	}
}
