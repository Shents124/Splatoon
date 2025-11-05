using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions;
using Runtime.Ball;
using Runtime.ConfigData;
using Runtime.Constant;
using Runtime.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Manager
{
    public class SpawnManager : MonoBehaviour
    {
        public List<Transform> spawnPoints = new();
        public Vector2 randomForce;
        public SpawnConfigData spawnConfigData;
        public BallConfig ballConfig;
        public float angle = 60f;
        
        private int _currentWave;
        private HashSet<int> _ballAlive = new();
        private int _sortOder;
        
        public async UniTask Spawn()
        {
            _ballAlive.Clear();
            var waveConfig = spawnConfigData.GetWaveConfig(_currentWave);
            foreach (var config in waveConfig)
            {
                for (int i = 0; i < config.count; i++)
                {
                    var spawnPosition = GetRandomSpawnPoint(out var force);
                    SpawnBall(config.ballId, config.attack, config.heath, spawnPosition, force);
                    await UniTask.Delay(1000);
                }
            }
        }

        private void SpawnBall(int ballId, float attack, float health, Vector2 spawnPosition, Vector2 force)
        {
            _sortOder++;
            var ball = PoolService.Spawn<BaseBall>(PoolType.Ball, PrefabName.BALL_PREFAB);
            var scale = ballConfig.GetBallConfig(ballId).GetRandomScale();
            var ballData = new BallData()
            {
                id = ballId,
                attack = attack,
                health = health,
                force = force,
                position = spawnPosition,
                scale = scale,
                sortOrder = _sortOder
            };
                
            ball.Initialize(ballData, HandleOnBallDead);
            _ballAlive.Add(ball.GetInstanceID());
        }
        
        private Vector2 GetRandomSpawnPoint(out Vector2 forceBall)
        {
            var spawnPoint = spawnPoints.GetRandomElement();
            var force = Random.Range(randomForce.x, randomForce.y);
            var direction = spawnPoint.position.x > 0 ? Vector2.left : Vector2.right;
            forceBall = direction * force;
            return spawnPoint.position;
        }
        
        private void HandleOnBallDead(BaseBall baseBall)
        {
            _ballAlive.Remove(baseBall.GetInstanceID());

            var ballId = baseBall.ballId;
            if (ballId == 1)
                return;

            var newBallId = ballId - 1;
            var aimDir = Vector2.up;
            float baseAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            float half = angle * 0.5f;

            var attack = baseBall.GetDamage();
            var spawnPositon = baseBall.transform.position;
            var heath = baseBall.maxHealth / 2;
            if (heath <= 0)
                heath = 1;
            
            for (int i = 0; i < 2; i++)
            {
                float t = (float)i / 1; // 0 → 1
                float offset = Mathf.Lerp(-half, half, t);
                float rad = (baseAngle + offset) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                var force = dir * randomForce.y * 2;
                
                SpawnBall(newBallId, attack, heath, spawnPositon, force);
            }
        }
    }
}