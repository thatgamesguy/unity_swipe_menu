using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// A simple example script. Updates text in the demo to show a menu item has been selected.
/// </summary>
public class ExampleCharacterSelector : MonoBehaviour
{
	public Text text;

	public void Select (string name)
	{
		text.text = (name + " selected");
	}
}
