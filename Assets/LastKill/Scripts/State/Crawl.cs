using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class Crawl : AbstractAbilityState
	{
        [SerializeField] private float crawlSpeed = 2f;
        [SerializeField] private float capsuleHeightOnCrawl = 0.5f;

        [Header("Cast Parameters")]
        [SerializeField] private LayerMask obstaclesMask;
        [Tooltip("This is the height that sphere cast can reach to know when should force crawl state")]
        [SerializeField] private float MaxHeightToStartCrawl = 0.75f;

        [Header("Animation States")]
        [SerializeField] private string startCrawlAnimationState = "Stand to Crawl";
        [SerializeField] private string stopCrawlAnimationState = "Crawl to Stand";


        private bool startCrawl = false;
        private bool stopCrawl = false;

        private float _defaultCapsuleRadius = 0;

        private void Awake()
        { 
            _defaultCapsuleRadius = _capsule.GetCapsuleRadius();
        }

        public override bool ReadyToStart()
        {
                return _input.Crawl;
        }


        public override void OnStartState()
        {
            // stop any movement
            _move.StopMovement();

            // Tells system that it's starting crawl
            startCrawl = true;

            // set crawl animation
            _animator.SetAnimationState(startCrawlAnimationState,0);
            //_animator.Animator.CrossFadeInFixedTime(startCrawlAnimationState, 0.2f,-1);
            // resize capsule collider
            _capsule.SetCapsuleSize(capsuleHeightOnCrawl, _capsule.GetCapsuleRadius());
        }


        public override void UpdateState()
        {
            // wait start crawl animation finishes
            if (startCrawl)
            {
                
                if(!_animator.HasFinishedAnimation(startCrawlAnimationState, 0))
                    startCrawl = false;

                return;
            }

            // wait stop crawl finishes to stop this ability
            if (stopCrawl)
            {
               if(_animator.HasFinishedAnimation(0))
                    StopState();

                return;
            }

            _move.Move(_input.Move, crawlSpeed);

            // if crawl was true again, it means should stop ability
            if (!_input.Crawl && !_detection.CanGetUp(2f))
            {
                _animator.SetAnimationState(stopCrawlAnimationState,0);
                stopCrawl = true;
                _move.StopMovement();
            }
        }

        public override void OnStopState()
        {
            // reset control variables
            startCrawl = false;
            stopCrawl = false;

            // reset capsule size
            _capsule.ResetCapsuleSize();
        }

		private bool ForceCrawlByHeight()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, _defaultCapsuleRadius, Vector3.up, out hit,
                MaxHeightToStartCrawl, obstaclesMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.y - transform.position.y > capsuleHeightOnCrawl)
                    return true;
            }

            return false;
        }
    }
}
