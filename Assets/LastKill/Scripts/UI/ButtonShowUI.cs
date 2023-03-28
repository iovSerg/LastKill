using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastKill
{
    public class ButtonShowUI : MonoBehaviour
    {
		[SerializeField] private Image buttonImage;
		[SerializeField] private Sprite crouch;
		[SerializeField] private Sprite crawl;

		private bool clickBtn = true;
		private void Awake()
		{
			buttonImage = gameObject.GetComponent<Image>();
		}

		public void ChangeIcon()
		{
			buttonImage.sprite = clickBtn ? crawl : crouch;
			clickBtn = !clickBtn;
		}
	}
}
