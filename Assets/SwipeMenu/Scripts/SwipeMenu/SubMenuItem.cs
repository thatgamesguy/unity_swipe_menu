using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SwipeMenu
{
	/// <summary>
	/// Attach to any sub-menu item. See Multiple Menu example scene for usage.
	/// </summary>
	public class SubMenuItem : MonoBehaviour
	{
		/// <summary>
		/// The menu item who owns this sub-menu.
		/// </summary>
		public MenuItem OwnerMenu;

		/// <summary>
		/// The behaviour to be invoked when this sub-menu is selected.
		/// </summary>
		public Button.ButtonClickedEvent OnClick;

		void Update ()
		{
			
			if (!Menu.instance.MenuCentred (OwnerMenu)) {
				return;
			}

#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					CheckTouch (Input.GetTouch (0).position);
				}
			}
#else
            if (Input.GetMouseButtonUp (0) && Helper.GetMouseAxis(MouseAxis.x) == 0) {
				CheckTouch (Input.mousePosition);
			}
#endif
		}
		
		private void CheckTouch (Vector3 screenPoint)
		{
			Ray touchRay = Camera.main.ScreenPointToRay (screenPoint);
			RaycastHit hit;
			
			Physics.Raycast (touchRay, out hit);
			
			if (hit.collider != null && hit.collider.gameObject.Equals (gameObject)) {
				
				OnClick.Invoke ();
			}
		}
	}
}
