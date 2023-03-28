using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class ButtonShowUI : MonoBehaviour
    {
        [SerializeField] GameObject objectCrouch;
        [SerializeField] GameObject objectCrawl;
		private bool crouch = false;
		private void Awake()
		{
			IInput.OnCrouch += OnCrouch;
		}

		private void OnCrouch()
		{
			objectCrouch.SetActive(false);
			objectCrawl.SetActive(true);
		}

		public void EnableCrouch()
		{
			if(crouch)
			{
				objectCrawl.SetActive(false);
				objectCrouch.SetActive(true);
				crouch = false;
			}
			crouch = true;
		}
		public void DisableCrouch()
		{
			objectCrouch.SetActive(false);
			objectCrawl.SetActive(true);
		}

	}
}
