﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;

[RequireComponent(typeof(Button))]
public class UIDrop : UIMember, IDropHandler {

	#region Properties

	[SerializeField]	private EDropState m_EDropState = EDropState.Free;
	[SerializeField]	private UIGroup m_Group;

	public enum EDropState
	{
		Free 	= 0,
		Dropped = 1
	}

	private Button m_CancelDropButton;

	public GameObject cloneDragableObject;
	public UIDrag dragableObject;
	public Action<Vector2> OnEventDrop;
	public Action<Vector2> OnEventEndDrop;
	public Action<Vector2> OnEventCancelDrop;

	#endregion

	#region Monobehaviour

	protected override void Awake ()
	{
		base.Awake ();
		m_CancelDropButton = this.GetComponent<Button> ();
		if (m_CancelDropButton != null) {
			m_CancelDropButton.onClick.RemoveAllListeners ();
		} else {
			m_CancelDropButton = this.gameObject.AddComponent<Button> ();
		}
	}

	#endregion

	#region IDropHandler implementation

	public void OnDrop (PointerEventData eventData)
	{
		OnItemDrop (eventData.position);
		if (m_EDropState == EDropState.Free) {
			SetDropObject (eventData);
		} else {
			OnItemCancelDrop(Input.mousePosition);
			SetDropObject (eventData);
		}
	}

	#endregion

	#region IMember implementation

	public override void Clear ()
	{
		base.Clear ();
		ClearDropObject ();
	}

	#endregion

	#region Main methods

	protected virtual void OnItemDrop(Vector2 position) {
		
	}

	protected virtual void OnItemEndDrop(Vector2 position) {
		CloneDragObject ();
		if (OnEventEndDrop != null) {
			OnEventEndDrop (position);
		}
	}

	protected virtual void OnItemCancelDrop(Vector2 position) {
		ClearDropObject ();
		if (OnEventCancelDrop != null) {
			OnEventCancelDrop (position);
		}
	}

	public void SetDropObject(PointerEventData eventData) {
		SetDropObject (eventData.pointerDrag, eventData.position);
	}

	public void SetDropObject(GameObject dropObject, Vector2 position) {
		dragableObject = dropObject.GetComponent<UIDrag> ();
		if (dragableObject != null) {
			dragableObject.OnEventEndDrag -= OnItemEndDrop;
			dragableObject.OnEventEndDrag += OnItemEndDrop;
			dragableObject.dropableObject = this;
			m_CancelDropButton.onClick.RemoveAllListeners ();
			m_CancelDropButton.onClick.AddListener (() => {
				OnItemCancelDrop(Input.mousePosition);
			});
		} 
		if (OnEventDrop != null) {
			OnEventDrop (position);
		} 
		m_EDropState = EDropState.Dropped;
	}

	private void CloneDragObject() {
		dragableObject.enabled = false;
		cloneDragableObject = Instantiate(dragableObject.content);
		var contentRectTransform = cloneDragableObject.transform as RectTransform;
		contentRectTransform.SetParent (this.transform);
		contentRectTransform.localPosition = Vector3.zero;
		contentRectTransform.localScale = Vector3.one;
		contentRectTransform.anchorMin = Vector2.zero;
		contentRectTransform.anchorMax = Vector2.one;
		contentRectTransform.anchoredPosition = Vector2.zero;
		contentRectTransform.sizeDelta = Vector2.one;
		dragableObject.enabled = true;
		dragableObject.content.SetActive (false);
	}

	public void ClearDropObject() {
		if (cloneDragableObject != null) {
			DestroyImmediate (cloneDragableObject);
		}
		if (dragableObject != null) {
			dragableObject.enabled = true;
			dragableObject.content.SetActive (true);
			dragableObject.OnEventEndDrag -= OnItemEndDrop;
			dragableObject.dropableObject = null;
			cloneDragableObject = null;
			m_CancelDropButton.onClick.RemoveAllListeners ();
		}
		m_EDropState = EDropState.Free;
	}

	public void SetState(EDropState state) {
		m_EDropState = state;
	}

	#endregion

}
