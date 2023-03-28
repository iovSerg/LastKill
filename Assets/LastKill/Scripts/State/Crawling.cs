using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Crawling : AbstractAbilityState
	{
		[SerializeField] private float crawlSpeed = 2f;
		[SerializeField] private float capsuleHeightOnCrawl = 0.5f;

		[SerializeField] private LayerMask obstaclesMask;

		[Header("Animation States")]
		[SerializeField] private string startCrawlAnimationState = "Crawling.StandToCrawl";
		[SerializeField] private string stopCrawlAnimationState = "Crawling.CrawlToStand";

		private int hashStartCrawl;
		private int hashStopCrawl;

		private bool startingCrawl = false;
		private bool stoppingCrawl = false;

		private float defaultCapsuleRadius = 0;
		private void Awake()
		{
			hashStartCrawl = Animator.StringToHash(startCrawlAnimationState);
			hashStopCrawl = Animator.StringToHash(stopCrawlAnimationState);

		}
		public override void OnStartState()
		{
			defaultCapsuleRadius = _capsule.GetCapsuleRadius();
			startingCrawl = true;
			_animator.SetAnimationState(hashStartCrawl,0);
			_capsule.SetCapsuleSize(capsuleHeightOnCrawl, defaultCapsuleRadius);
		}

		public override bool ReadyToStart()
		{
			return _input.Crawl && _move.IsGrounded();
		}

		public override void UpdateState()
		{
			if (startingCrawl)
			{
				if (!_animator.HasFinishedAnimation(startCrawlAnimationState,0))
					startingCrawl = false;
			}
			if (stoppingCrawl)
			{
				if (_animator.HasFinishedAnimation(0,0.8f))
					StopState();
				return;
			}
			_move.Move(_input.Move, crawlSpeed);

			if (!_input.Crawl && !_detection.CanGetUp(2f))
			{
				_animator.SetAnimationState(hashStopCrawl);
				stoppingCrawl = true;
				_move.StopMovement();
			}
		}
		public override void OnStopState()
		{
			base.OnStopState();
			startingCrawl = false;
			stoppingCrawl = false;
			_capsule.ResetCapsuleSize();
			
		}
	}
}
