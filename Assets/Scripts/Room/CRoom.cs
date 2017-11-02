using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HideAndSeek {
	public class CRoom : CObjectController {

		#region Singleton

		protected static CRoom m_Instance = null;
		public static CRoom Instance {
			get { 
				if (m_Instance == null) {
					var go = GameObject.FindObjectOfType<CRoom> ();
					m_Instance = go;
				}
				return m_Instance;
			}
		}

		#endregion

		#region Fields

		[Header("Info")]
		public string roomName;
		public string roomSceneName;

		[Header("Stuffs")]
		public CStuff[] stuffs;

		[Header("Swap points")]
		public CSwapPoint[] swapPoints;

		[Header("Hiden points")]
		public CHiddenPoint[] hiddenPoints;

		#endregion

		#region Implementation MonoBehaviour

		protected override void Awake ()
		{
			base.Awake ();
			m_Instance = this;
		}

		protected override void Update ()
		{
			base.Update ();
		}

		#endregion

		#region Main methods

		public virtual CObjectController DetectObject(GameObject value) {
			if (value == null)
				return null;
			var objCtrl = value.GetComponent<CObjectController> ();
			if (objCtrl == null)
				return null;
			if (Array.IndexOf (this.stuffs, objCtrl) != -1) {
				return objCtrl;
			}
			if (Array.IndexOf (this.swapPoints, objCtrl) != -1) {
				return objCtrl;
			}
			return objCtrl;
		}

		public virtual CObjectController DetectStuffObject(GameObject value) {
			if (value == null)
				return null;
			var objCtrl = value.GetComponent<CObjectController> ();
			if (objCtrl == null)
				return null;
			return Array.IndexOf (this.stuffs, objCtrl) != -1 ? objCtrl : null;
		}

		public virtual CSwapPoint GetSwapPoint(string name) {
			for (int i = 0; i < this.swapPoints.Length; i++) {
				if (this.swapPoints [i].swapRoom == name)
					return this.swapPoints [i];
			}
			return null;
		}

		public virtual CHiddenPoint GetHiddenPoint(int index) {
			if (index < 0 || index >= this.hiddenPoints.Length)
				return null;
			return this.hiddenPoints[index];
		}

		#endregion

		#region Getter && Setter 

		public override Vector3 GetClosestPoint (Vector3 value) {
			return value;
		}

		#endregion

	}
}
