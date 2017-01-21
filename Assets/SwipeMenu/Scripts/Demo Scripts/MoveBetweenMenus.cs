using UnityEngine;
using System.Collections;
using SwipeMenu;
public class MoveBetweenMenus : MonoBehaviour
{

	public void MoveLeft ()
	{
		Menu.instance.MoveLeftRightByAmount (-1);
	}

	public void MoveRight ()
	{
		Menu.instance.MoveLeftRightByAmount (1);
	}
}
