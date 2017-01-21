using UnityEngine;
using System.Collections;

namespace SwipeMenu
{
	/// <summary>
	/// Handles touches seperate from swipes. Supports mouse and mobile touch controls.
	/// If a menu item is selected and isn't centred, then the menu item is animated to centre. If
	/// a menu item is centred than its <see cref="MenuItem.OnClick"/> is invoked.
	/// </summary>
	public class TouchHandler : MonoBehaviour
	{
		/// <summary>
		/// If true, menu selection is handled.
		/// </summary>
		public bool handleTouches = true;

		/// <summary>
		/// The selected menu item has to be centred for selectiion to occur.
		/// </summary>
		public bool requireMenuItemToBeCentredForSelectiion = true;

		private SwipeHandler _swipeHandler;

		void Start ()
		{
			_swipeHandler = GetComponent<SwipeHandler> ();
		}

		void LateUpdate ()
		{
			if (!handleTouches)
				return;

			if (_swipeHandler && _swipeHandler.isSwiping) {
				return;
			}

#if (!UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL)
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
			
			if (hit.collider != null && hit.collider.gameObject.CompareTag ("MenuItem")) {

				var item = hit.collider.GetComponent<MenuItem> ();

				if (Menu.instance.MenuCentred (item)) {
					Menu.instance.ActivateSelectedMenuItem (item);
				} else {
					Menu.instance.AnimateToTargetItem (item);

					if (!requireMenuItemToBeCentredForSelectiion) {
						Menu.instance.ActivateSelectedMenuItem (item);
					}
				}
			}
		}

	}
}
