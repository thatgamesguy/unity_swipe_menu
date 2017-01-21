using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SwipeMenu;

/// <summary>
/// Updates UI text colour when a button is pressed.https://www.assetstore.unity3d.com/en/#!/content/84
/// </summary>
public class UpdateTextColourOnMenuClick : MonoBehaviour
{
	public Text text;

	public void UpdateColour (MenuItem item)
	{
		text.color = item.gameObject.GetComponent<Renderer> ().material.color;
	}
}
