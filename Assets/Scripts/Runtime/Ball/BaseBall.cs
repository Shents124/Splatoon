using System;
using Runtime.Constant;
using Runtime.Manager;
using Runtime.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

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
        
        public override void Initialize(SpawnManager spawnManager, BallData ballData, string key, Action<BaseEnemy> onDead)
        {
            base.Initialize(spawnManager, ballData, key, onDead);
            rigid2D.AddForce(ballData.force, ForceMode2D.Impulse);
        }
    }
}