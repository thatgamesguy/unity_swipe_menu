using UnityEngine;
using System.Collections;

/// <summary>
/// Used in the audio player test scene. Plays and stops audio clips on menu button presses.
/// </summary>
public class SwipeMenuAudioPlayer : MonoBehaviour
{

	public ExampleMenuAudioPlayer audioPlayer;
	public SpriteRenderer spriteRenderer;
	public Sprite playingSrpite;
	public Sprite notPlayingSprite;

	private bool _isPlaying = false;

	/// <summary>
	/// If the clip is currently playing then stops clip else plays the clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void Activate (AudioClip clip)
	{
		if (_isPlaying) {
			audioPlayer.StopClip ();
			_isPlaying = false;
			spriteRenderer.sprite = notPlayingSprite;
		} else {
			audioPlayer.PlayClip (clip);
			_isPlaying = true;
			spriteRenderer.sprite = playingSrpite;
		}
	}

	/// <summary>
	/// Sets _isPlaying to false and updates the sprite accordingly.
	/// </summary>
	public void Deactivate ()
	{
		if (_isPlaying) {
			_isPlaying = false;
			spriteRenderer.sprite = notPlayingSprite;
		}
	}
}
