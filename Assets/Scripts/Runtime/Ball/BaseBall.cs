using System;
using System.Collections.Generic;
using Extensions;
using Runtime.Constant;
using Runtime.Manager;
using UnityEngine;

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
    }
    
    public class BaseBall : BaseEnemy
    {
        [SerializeField] private Rigidbody2D rigid2D;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<Sprite> sprites;
        public override void Initialize(SpawnManager spawnManager, BallData ballData, string key, Action<BaseEnemy> onDead)
        {
            base.Initialize(spawnManager, ballData, key, onDead);
            SetSprite();
            rigid2D.AddForce(ballData.force, ForceMode2D.Impulse);
        }

        private void SetSprite()
        {
            var sprite = sprites.GetRandomElement();
            spriteRenderer.sprite = sprite;
        }
    }
}