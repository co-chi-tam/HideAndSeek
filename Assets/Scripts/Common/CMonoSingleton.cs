using UnityEngine;
using System.Collections;

namespace SimpleSingleton {
	public class CMonoSingleton<T>: MonoBehaviour where T : MonoBehaviour {

		#region Singleton

		protected static T m_Instance;
		private static object m_SingletonObject = new object();
		public static T Instance {
			get { 
				lock (m_SingletonObject) {
					if (m_Instance == null) {
						var objOfType = GameObject.FindObjectOfType<T> ();
						if (objOfType == null) {
							var go = new GameObject ();
							m_Instance = go.AddComponent<T> ();
						} 
						m_Instance = objOfType;
						m_Instance.gameObject.SetActive (true);
						m_Instance.name = typeof(T).Name;
					}
					return m_Instance;
				}
			}
		}

		public static T GetInstance() {
			return Instance;
		}

		#endregion

		#region Implementation Monobehaviour

		protected virtual void Awake() {
			m_Instance = this as T;
		} 

		#endregion
	}
}
