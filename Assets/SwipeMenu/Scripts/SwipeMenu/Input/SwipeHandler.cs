using UnityEngine;
using UnityEngine.UI;

namespace SwipeMenu
{
	/// <summary>
	/// Handles swiping and flicking. Includes mouse and mobile support.
	/// </summary>
	public class SwipeHandler : MonoBehaviour
	{
		/// <summary>
		/// If true, swipes will be handled.
		/// </summary>
		public bool handleSwipes = true;

		/// <summary>
		/// Flicks are classed as swipes but with a force greater than SwipeHandler#requiredForceForFlick.
		/// </summary>
		public bool handleFlicks = true;

		/// <summary>
		/// The required force for a swipe to be classes as a flick.
		/// </summary>
		public float requiredForceForFlick = 7f; 
	
		public enum FlickType
		{
			Inertia,
			MoveOne
		}
		/// <summary>
		/// The type of flick. Inertia scrolls kinematically, MoveOne moves the menu in the x direction by one for each flick.
		/// </summary>
		public FlickType flickType = FlickType.Inertia;

		/// <summary>
		/// Once a swipe or flick has finished this will move the menu closest to the centre, to the centre.
		/// </summary>
		public bool lockToClosest = true;

        /// <summary>
        /// Limits the maximum force applied when swiping.
        /// </summary>
        public float maxForce = 15f;

		private Vector3 finalPosition, startpos, endpos, oldpos;
		private float length, startTime, mouseMove, force;
		private bool SW;

		/// <summary>
		/// Gets a value indicating whether this <see cref="SwipeMenu.SwipeHandler"/> is swiping.
		/// </summary>
		/// <value><c>true</c> if is swiping; otherwise, <c>false</c>.</value>
		public bool isSwiping {
			get {
				return SW || length != 0;
			}
		}

		void Update ()
		{

#if (!UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_WEBPLAYER && !UNITY_WEBGL)
			HandleMobileSwipe ();

#else
            HandleMouseSwipe();
#endif


        }

		private void HandleMobileSwipe ()
		{

            if (Input.touchCount > 0) {

				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					startTime = Time.time;
					finalPosition = Vector3.zero;
					length = 0;
					SW = false;
					Vector2 touchDeltaPosition = Input.GetTouch (0).position;
					startpos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);
					oldpos = startpos;
				}   

				if (Input.GetTouch (0).phase == TouchPhase.Moved) {
					SW = true;

					Vector2 touchDeltaPosition = Input.GetTouch (0).position;
					Vector3 pos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);

					if (handleSwipes && pos.x != oldpos.x) {
						var f = pos - oldpos;

						var l = f.x < 0 ? (f.magnitude * Time.deltaTime) : -(f.magnitude * Time.deltaTime);
					
						l *= .2f;

						Menu.instance.Constant (l);
					}

					oldpos = pos;
				}
			
				if (Input.GetTouch (0).phase == TouchPhase.Canceled) {
					SW = false;
				}
			
				if (Input.GetTouch (0).phase == TouchPhase.Stationary) {
					SW = false;
				}

				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					if (SW && handleFlicks) {
						Vector2 touchPosition = Input.GetTouch (0).position;
						endpos = new Vector3 (touchPosition.x, 0, touchPosition.y);
						finalPosition = endpos - startpos;
						length = finalPosition.x < 0 ? -(finalPosition.magnitude * Time.deltaTime) : (finalPosition.magnitude * Time.deltaTime);

						length *= .35f;

						var force = length / (Time.time - startTime);

                        force = Mathf.Clamp(force, -maxForce, maxForce);

                        if (handleFlicks && Mathf.Abs (force) > requiredForceForFlick) {
							Menu.instance.Inertia (-length);
						}  
					}

					if (lockToClosest) {
						Menu.instance.LockToClosest ();
					}
				}

			}


		
		}

		private void HandleMouseSwipe ()
		{

            if (Input.GetMouseButtonDown (0)) {
				startTime = Time.time;
				finalPosition = Vector3.zero;
				length = 0;
				Vector2 touchDeltaPosition = Input.mousePosition;
				startpos = new Vector3 (touchDeltaPosition.x, 0, touchDeltaPosition.y);
			}

			if (Input.GetMouseButtonUp (0)) {
                Vector2 touchPosition = Input.mousePosition;
				endpos = new Vector3 (touchPosition.x, 0, touchPosition.y);
				finalPosition = endpos - startpos;
				length = finalPosition.x < 0 ? (finalPosition.magnitude * Time.deltaTime) : -(finalPosition.magnitude * Time.deltaTime);
				length *= .5f;

				force = length / (Time.time - startTime);

                force = Mathf.Clamp(force, -maxForce, maxForce);

				if (handleFlicks && Mathf.Abs (force) > requiredForceForFlick) {

					if (flickType == FlickType.Inertia) {
                        Menu.instance.Inertia (length);
					} else {
						if (length > 0) {
							Menu.instance.MoveLeftRightByAmount (1);
						} else {
							Menu.instance.MoveLeftRightByAmount (-1);
						}
					}
				} else if (lockToClosest && force != 0) {
					Menu.instance.LockToClosest ();
				}

			}

            mouseMove = Helper.GetMouseAxis(MouseAxis.x); 
        
            if (handleSwipes && Input.GetMouseButton (0) && mouseMove != 0) {
         
                Menu.instance.Constant (-(mouseMove * .1f));
			}  

		
		}
	}
}