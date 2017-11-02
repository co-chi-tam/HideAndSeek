using UnityEngine;
using System.Collections;
using UICustomize;

public class UIGroup : MonoBehaviour, IGroup {

	#region Properties

	[SerializeField]	private string m_GroupName = "group 1";
	[SerializeField]	public UIMember[] members;

	#endregion

	#region Monobehaviour

	protected virtual void Awake ()
	{
		
	}

	public virtual void Update() {
		this.UpdateBaseTime (Time.deltaTime);
	}

	public virtual void UpdateBaseTime (float dt)
	{
		
	}

	#endregion

	public string GetGroupName() {
		return m_GroupName;
	}

}
