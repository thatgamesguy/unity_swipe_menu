using UnityEngine;
using System.Collections;
using SwipeMenu;

/// <summary>
/// Plays an audio clip on menu selection.
/// </summary>
[RequireComponent (typeof(AudioSource))]
public class ExampleMenuAudioPlayer : MonoBehaviour
{
	private AudioSource _audio;
	
	void Awake ()
	{
		_audio = GetComponent<AudioSource> ();
	}

	/// <summary>
	/// Plays the specified clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void PlayClip (AudioClip clip)
	{
		_audio.Stop ();
		_audio.PlayOneShot (clip);
	}

	/// <summary>
	/// Stops all currently playing audio instances.
	/// </summary>
	public void StopClip ()
	{
		_audio.Stop ();
	}

}
