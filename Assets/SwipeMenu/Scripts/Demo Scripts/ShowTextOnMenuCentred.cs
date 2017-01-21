using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SwipeMenu;

/// <summary>
/// Enables a mesh renderer when a menu item is centred and conversly disables renderer when menu not centred.
/// </summary>
[RequireComponent (typeof(MeshRenderer))]
public class ShowTextOnMenuCentred : MonoBehaviour
{
	public MenuItem ownerMenu;

	private MeshRenderer _text;
	
	void Start ()
	{
		_text = GetComponent<MeshRenderer> ();
	}

	void Update ()
	{
		if (Menu.instance.MenuCentred (ownerMenu)) {
			_text.enabled = true;
		} else {
			_text.enabled = false;
		}
	}
}
