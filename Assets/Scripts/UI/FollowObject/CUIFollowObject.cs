using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUIFollowObject : MonoBehaviour {

	#region Fields

	[Header("Follower")]
	[SerializeField]	protected GameObject m_Follower;

	protected RectTransform m_RectTransform;

	#endregion

	#region Implementation Monobehaviour

	protected virtual void Awake() {
		this.m_RectTransform = this.transform as RectTransform;
	}

	protected virtual void LateUpdate() {
		if (this.m_Follower == null)
			return;
		this.m_RectTransform.position = Camera.main.WorldToScreenPoint (this.m_Follower.transform.position);
	}

	#endregion

	#region Getter && Setter

	public virtual void SetFollowObject(GameObject value) {
		this.m_Follower = value;
	}

	#endregion

}
