using System;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using Game.Common;
using Runtime.Constant;
using Runtime.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Ball
{
    public struct BallData
    {
        public int id;
        public BallType ballType;
        public float attack;
        public float health;
        public float scale;
        public Vector2 force;
        public Vector2 position;
        public int sortOrder;
        public int exp;
        public float bounceHeight;
        public float xVelocity;
        public float weight;
    }
    
    public class BaseBall : BaseEnemy
    {
        [SerializeField] private Rigidbody2D rigid2D;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<Sprite> sprites;
        private bool _isBallBouncingTowardsRight;

        private Tween _mRotationTween;
        
        public override void Initialize(SpawnManager spawnManager, BallData ballData, string key, Action<BaseEnemy> onDead)
        {
            base.Initialize(spawnManager, ballData, key, onDead);
            SetSprite();
            _isBallBouncingTowardsRight = ballData.force.x > 0;
            rigid2D.mass = ballData.weight;
            rigid2D.AddForce(ballData.force, ForceMode2D.Impulse);
            
            _mRotationTween?.Kill();
            Vector3 rotation = Vector3.one;
            rotation.z = Random.Range(200, 360f);
            float rotationDur = 3f;
            _mRotationTween = transform.DORotate(rotation, rotationDur, RotateMode.LocalAxisAdd)
                .SetLoops(-1)
                .SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            _mRotationTween?.Kill();
        }

        private void SetSprite()
        {
            var sprite = sprites.GetRandomElement();
            spriteRenderer.sprite = sprite;
        }
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            var borderDirection = Direction.eNone;
            if (other.gameObject.CompareTag("LeftWall"))
            {
               borderDirection = Direction.eLeft;
                
            }
            else if (other.gameObject.CompareTag("RightWall"))
            {
                borderDirection = Direction.eRight;
            }
            else if (other.gameObject.CompareTag("Ground"))
            {
                borderDirection = Direction.eBottom;
            }
            
            UpdateBallForceForTheDirection(borderDirection);
        }
        
        private void UpdateBallForceForTheDirection(Direction borderDirection)
        {
            if (borderDirection == Direction.eNone)
                return;
            
            var outForce = rigid2D.linearVelocity;

            float xVelocity = ballData.xVelocity;
            
            if (borderDirection is Direction.eLeft or Direction.eRight)
            {
                // Left or right Border
                if (borderDirection == Direction.eRight)
                {
                    outForce.x = -xVelocity;
                    _isBallBouncingTowardsRight = false;
                }
                else
                {
                    outForce.x = xVelocity; 
                    _isBallBouncingTowardsRight = true;
                }
            }
            else
            {
                //to make sure not going out of the screeen
                //outForce.y = -2;

                if (borderDirection == Direction.eBottom)
                {
                    outForce.y = ballData.bounceHeight;

                    outForce.x = -xVelocity;//Deafult right

                    if (_isBallBouncingTowardsRight)
                        outForce.x = xVelocity;// To Left
                }
            }
            
            rigid2D.linearVelocity = outForce;
        }
    }
}