using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Used in the multiple menu test scene to update the stars shown when the use presses a button.
/// </summary>
public class UpdateMenuItemOnClick : MonoBehaviour
{
	public Text debugText;
	public TextMesh titleText;

	public SpriteRenderer[] starRenderers;

	public Sprite starSprite;

	private int _currentItem = -1;

	/// <summary>
	/// Updates the star sprite on button press.
	/// </summary>
	public void UpdateStar ()
	{
		debugText.text = "Load: " + titleText.text;

		if (_currentItem == 2)
			return;

		_currentItem = (_currentItem + 1) % starRenderers.Length;

		starRenderers [_currentItem].sprite = starSprite;
	

	}
}
