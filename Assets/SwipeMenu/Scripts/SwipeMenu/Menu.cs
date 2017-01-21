using UnityEngine;
using System.Collections;

namespace SwipeMenu
{
	/// <summary>
	/// The main menu class. Handles updating the menus position.
	/// </summary>
	public class Menu : MonoBehaviour
	{
		/// <summary>
		/// The starting menu item.
		/// </summary>
		public int startingMenuItem = 1;

		/// <summary>
		/// The angle of menu items that are not centred.
		/// </summary>
		public float menuItemAngle = 50.0f;

		/// <summary>
		/// The distance between menus. Distance between menus must be divisible by 0.5f. This is clamped in the Awake function.
		/// </summary>
		public float distanceBetweenMenus = 1.0f;
		
		/// <summary>
		/// Moves the centred menu closer to the camera. Provides an offset between 
		/// centred menu and background menus.
		/// </summary>
		public float zOffsetForCentredItem = 0.5f;

		/// <summary>
		/// The menu items. The items are audto parented to this transform.
		/// </summary>
		public MenuItem[] menuItems;


		private float _centreOffset = 1.0f;
		private float _currentMenuPosition = 0.0f;
		private float _maxMenuPosition;
		private SwipeHandler _swipeHandler;

		private static Menu _instance;
		/// <summary>
		/// Returns an instance of Menu. Provides centralised access to class form any script.
		/// </summary>
		/// <value>The instance.</value>
		public static Menu instance {
			get {
				if (!_instance) {
					_instance = GameObject.FindObjectOfType<Menu> ();
				}

				return _instance;
			}
		}

		void Awake ()
		{
			if (!_instance) {
				_instance = this;
			}

			distanceBetweenMenus -= IsDivisble (distanceBetweenMenus, 0.5f);

			_maxMenuPosition = (menuItems.Length + 1) * distanceBetweenMenus;

			startingMenuItem = Mathf.Clamp (startingMenuItem, 1, menuItems.Length);

			_currentMenuPosition = ((1) * distanceBetweenMenus) * startingMenuItem;

			ParentMenuItems ();
			UpdateMenuItemsPositionInWorldSpace ();

			if (GetComponent<TouchHandler> () == null) {
				gameObject.AddComponent<TouchHandler> ();
			}

			_swipeHandler = GetComponent<SwipeHandler> ();

			if (_swipeHandler == null) {
				_swipeHandler = gameObject.AddComponent<SwipeHandler> ();
			}

		}


		void Update ()
		{
			UpdateMenuItemsPositionInWorldSpace ();
		}

		/// <summary>
		/// Moves whole menu left/right based on amount parameter.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void MoveLeftRightByAmount (int amount)
		{
			int currentIndex = GetClosestMenuItemIndex ();


			if (currentIndex != -1) {
				currentIndex = Mathf.Clamp (currentIndex + amount, 0, menuItems.Length - 1);
				AnimateToTargetItem (menuItems [currentIndex]);
			}
		}
	
		/// <summary>
		/// Animates to target MenuItem using iTween.
		/// </summary>
		/// <param name="item">Item.</param>
		public void AnimateToTargetItem (MenuItem item)
		{
			float offset = CalcPosXInverse (item.transform.position.x);

			iTween.ValueTo (gameObject, iTween.Hash ("from", _currentMenuPosition, "to", _currentMenuPosition + offset, 
			                                         "time", 0.5, "easetype", iTween.EaseType.easeOutCubic, "onupdate", "UpdateCurrentMenuPosition"));
		}

		/// <summary>
		/// Provides a direct/constant movement by the specified amount. Not animated. Used for swipes that are not classed as flicks.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void Constant (float amount)
		{
			_currentMenuPosition = Mathf.Clamp (_currentMenuPosition + amount, 0, _maxMenuPosition);
		}

		/// <summary>
		/// Moves the specified amout with inerta using iTween. Used for flicks.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public void Inertia (float amount)
		{
			var to = Mathf.Clamp (_currentMenuPosition + amount, 0, _maxMenuPosition);

			iTween.ValueTo (gameObject, iTween.Hash ("from", _currentMenuPosition, "to", to, 
			                                         "time", 0.5f, "easetype", iTween.EaseType.easeOutCubic, 
			                                         "onupdate", "UpdateCurrentMenuPosition", "oncomplete", "AnimationComplete"));
		}

		/// <summary>
		/// Finds MenuItem closest to centre and animates that MenuItem to centre.
		/// </summary>
		public void LockToClosest ()
		{
			MenuItem item = GetClosestMenuItem ();

			if (item != null)
				AnimateToTargetItem (item);
		}

		/// <summary>
		/// Returns truw if the specified menu item is centred.
		/// </summary>
		/// <returns><c>true</c>, if centred was menued, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		public bool MenuCentred (MenuItem item)
		{
			return item.transform.position.x == 0;
		}

		/// <summary>
		/// Disables all menu items.
		/// </summary>
		public void HideMenus ()
		{
			foreach (var menu in menuItems) {
				menu.gameObject.SetActive (false);
			}
		}

		/// <summary>
		/// Enables all menu items.
		/// </summary>
		public void ShowMenus ()
		{
			foreach (var menu in menuItems) {
				menu.gameObject.SetActive (true);
			}
		}

		/// <summary>
		/// Invokes the OnClick event for the specified menu item. Invokes the OnOtherMenuClick for all menus
		/// that are not selected.
		/// </summary>
		/// <param name="item">Item.</param>
		public void ActivateSelectedMenuItem (MenuItem item)
		{
			item.OnClick.Invoke ();

			foreach (var i in menuItems) {
				if (!i.Equals (item)) {
					i.OnOtherMenuClick.Invoke ();
				}
			}
		}

		/// <summary>
		/// Parents the menu items to manu transform.
		/// </summary>
		private void ParentMenuItems ()
		{
			foreach (var menu in menuItems) {
				if (menu == null) {
					Debug.LogError ("Menu item not set in inspector");
				} else {
					menu.transform.SetParent (transform);
				}
			}
		}

		/// <summary>
		/// Returns the menu item closest to centre.
		/// </summary>
		/// <returns>The closest menu item.</returns>
		private MenuItem GetClosestMenuItem ()
		{
			MenuItem item = null;
			
			float xOffset = float.MaxValue;
			
			foreach (var i in menuItems) {
				var x = CalculateOffsetFromX (i.gameObject.transform.position.x, 0);
				
				if (x == 0)
					return i;
				
				if (x < xOffset) {
					item = i;
					xOffset = x;
				}
			}

			return item;
		}

		/// <summary>
		/// Returns the index of the menu item closest to centre.
		/// </summary>
		/// <returns>The closest menu item index.</returns>
		private int GetClosestMenuItemIndex ()
		{
			int index = -1;
			
			float xOffset = float.MaxValue;
			
			for (int i = 0; i < menuItems.Length; i++) {
				var x = CalculateOffsetFromX (menuItems [i].gameObject.transform.position.x, 0);
				
				if (x == 0)
					return i;
				
				if (x < xOffset) {
					index = i;
					xOffset = x;
				}
			}
			
			return index;
		}

		/// <summary>
		/// Calculates the position X inverse.
		/// </summary>
		/// <returns>The position X inverse.</returns>
		/// <param name="realPosx">Real posx.</param>
		private float CalcPosXInverse (float realPosx)
		{
			if (realPosx < -1.0f) {
				return - (realPosx * realPosx + 1) / 2;
			} else if (realPosx < 1.0f) {
				return realPosx;
			} else {
				return (realPosx * realPosx + 1) / 2;
			}
		}

		
		/// <summary>
		/// Calculates the required rotation for the menu. Based on Menu#menuItemAngle.
		/// </summary>
		/// <returns>The menu item rotation.</returns>
		/// <param name="offsetx">Offsetx.</param>
		private float CalculateMenuItemRotation (float offsetx)
		{
			//left covers
			if (offsetx < -_centreOffset) {
				return -menuItemAngle;
			} else if (offsetx > _centreOffset) {
				return menuItemAngle;
			} else {
				return offsetx * (menuItemAngle / _centreOffset);	
			}
		}

		/// <summary>
		/// Calculates the menu item X position.
		/// </summary>
		/// <returns>The menu item X position.</returns>
		/// <param name="offsetx">Offsetx.</param>
		private float CalculateMenuItemXPosition (float offsetx)
		{
			if (offsetx >= 1.0f) {
				return  Mathf.Sqrt (2 * offsetx - 1);
			} else if (offsetx <= -1.0f) {
				return - Mathf.Sqrt (-2 * offsetx - 1);
			} else 
				return offsetx;
		}

		/// <summary>
		/// Calculates the menu item Z position. Based on Menu#distanceBetweenSelectedMenuAndOthers.
		/// </summary>
		/// <returns>The menu item Z position.</returns>
		/// <param name="offsetx">Offsetx.</param>
		private float CalculateMenuItemZPosition (float offsetx)
		{
			if (offsetx < -_centreOffset) {
				return 0;
			} else if (offsetx < 0) {
				return -zOffsetForCentredItem / _centreOffset * offsetx - zOffsetForCentredItem;
			} else if (offsetx < _centreOffset) {
				return zOffsetForCentredItem / _centreOffset * offsetx - zOffsetForCentredItem;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// Updates the menu items position and rotation in world space.
		/// </summary>
		private void UpdateMenuItemsPositionInWorldSpace ()
		{
			for (int i = 0; i < menuItems.Length; i++) {
				float offsetx = distanceBetweenMenus * (i + 1) - _currentMenuPosition;
				float posx = CalculateMenuItemXPosition (offsetx);
				float posz = CalculateMenuItemZPosition (offsetx);
				Vector3 pos = new Vector3 (posx, 0, posz);
				menuItems [i].transform.position = pos;
				Vector3 euler = new Vector3 (0, CalculateMenuItemRotation (offsetx), 0);
				menuItems [i].transform.eulerAngles = euler;
			}
		}

		/// <summary>
		/// Updates the current menu position.
		/// </summary>
		/// <param name="pos">Position.</param>
		private void UpdateCurrentMenuPosition (float pos)
		{
			_currentMenuPosition = pos;
		}

		/// <summary>
		/// Invoked at end of animation. Locks to closest menu item if SwipeHandler#lockToClosest is enabled.
		/// </summary>
		private void AnimationComplete ()
		{
			if (_swipeHandler.lockToClosest)
				LockToClosest ();
		}

		/// <summary>
		/// Calculates if x is divisible by n. Returns remainder.
		/// </summary>
		/// <returns>The remainder.</returns>
		/// <param name="x">X.</param>
		/// <param name="n">N.</param>
		private float IsDivisble (float x, float n)
		{
			return (x % n);
		}

		/// <summary>
		/// Calculates the offset from x.
		/// </summary>
		/// <returns>The offset from x.</returns>
		/// <param name="start">Start.</param>
		/// <param name="x">The x coordinate.</param>
		private float CalculateOffsetFromX (float start, float x)
		{
			return Mathf.Abs (start - x);
		}

	}
}
