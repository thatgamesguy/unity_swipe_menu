using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SwipeMenu
{
	/// <summary>
	/// Attach to any menu item. 
	/// </summary>
	public class MenuItem : MonoBehaviour
	{
		/// <summary>
		/// The behaviour to be invoked when the menu item is selected.
		/// </summary>
		public Button.ButtonClickedEvent OnClick;

		/// <summary>
		/// The behaviour to be invoked when another menu item is selected.
		/// </summary>
		public Button.ButtonClickedEvent OnOtherMenuClick;

	}
}