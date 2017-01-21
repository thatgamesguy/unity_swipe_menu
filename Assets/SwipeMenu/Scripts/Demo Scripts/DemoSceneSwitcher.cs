using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Switches between demo scenes when enter key pressed.
/// </summary>
public class DemoSceneSwitcher : MonoBehaviour
{
	public string sceneToLoad;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.KeypadEnter) || Input.GetKeyUp ("return")) {
			SceneManager.LoadScene (sceneToLoad);
		}
	}
}
