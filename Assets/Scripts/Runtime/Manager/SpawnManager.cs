using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions;
using Runtime.Ball;
using Runtime.ConfigData;
using Runtime.Constant;
using Runtime.Pool;
using Runtime.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Manager
{
    public class SpawnManager : MonoBehaviour
    {
        public BuffManager buffManager;
        public LevelUI levelUI;
        public LevelUpConfig levelUpConfig;
        public List<Transform> spawnPoints = new();
        public Vector2 randomForce;
        public SpawnConfigData spawnConfigData;
        public BallConfig ballConfig;
        public float angle = 60f;
        public float spawnDelay = 1f;
        
        private int _currentWave;
        private HashSet<int> _ballAlive = new();
        private int _sortOder;

        private int _currentLevel = 1;
        private int _currentExp;
        private int _expRequired;

        public async UniTask Initialize()
        {
            _currentLevel = 1;
            _currentExp = 0;
            _expRequired = levelUpConfig.ExpRequired(_currentLevel);
            levelUI.UpdateData(_currentLevel, (float)_currentExp / _expRequired,
                _currentLevel >= levelUpConfig.maxLevel);
            await SpawnWave();
        }

        private async UniTask SpawnWave()
        {
            Debug.LogWarning("Spawn Wave: " +  _currentWave);
            _ballAlive.Clear();
            var waveConfig = spawnConfigData.GetWaveConfig(_currentWave);
            foreach (var config in waveConfig)
            {
                for (int i = 0; i < config.count; i++)
                {
                    var spawnPosition = GetRandomSpawnPoint(out var force);
                    SpawnBall(config.ballType, config.ballId, config.attack, config.heath, spawnPosition, force);
                    await UniTask.Delay(TimeSpan.FromSeconds(spawnDelay));
                }
            }
        }

        private void SpawnBall(BallType ballType, int ballId, float attack, float health, Vector2 spawnPosition, Vector2 force)
        {
            _sortOder++;
            var key = ballType == BallType.Normal ? PrefabName.BALL_PREFAB : PrefabName.MINI_BOSS;
            var ball = PoolService.Spawn<BaseEnemy>(PoolType.Ball, key);
            var ballConfigCsv = ballConfig.GetBallConfig(ballId, ballType);
            var ballData = new BallData()
            {
                ballType = ballType,
                id = ballId,
                attack = attack,
                health = health,
                force = force,
                position = spawnPosition,
                scale = ballConfigCsv.GetRandomScale(ballType),
                sortOrder = _sortOder,
                exp = ballConfigCsv.exp,
            };
                
            ball.Initialize(this, ballData, key, HandleOnBallDead);
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
        
        private void HandleOnBallDead(BaseEnemy baseBall)
        {
            if (_ballAlive.Remove(baseBall.GetInstanceID()) == false)
                return;

            var exp = baseBall.ballData.exp;
            _currentExp += exp;
            if (_currentExp >= _expRequired)
            {
                _currentLevel++;
                if (_currentLevel >= levelUpConfig.maxLevel)
                {
                    levelUI.UpdateData(_currentLevel, 1, true);
                }
                else
                {
                    _currentExp -= _expRequired;
                    _expRequired = levelUpConfig.ExpRequired(_currentLevel);
                    levelUI.UpdateData(_currentLevel, (float)_currentExp / _expRequired);
                }
                
                buffManager.ShowSelectBuff(levelUpConfig.BuffRarity(_currentLevel - 1));
            }
            else
            {
                levelUI.UpdateData(_currentLevel, (float)_currentExp / _expRequired);
            }
            
            var ballId = baseBall.ballId;
            if (ballId == 1)
            {
                CheckClearWave();
                return;
            }

            var newBallId = ballId - 1;
            var aimDir = Vector2.up;
            float baseAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            float half = angle * 0.5f;

            var attack = baseBall.GetDamage();
            var spawnPosition = baseBall.transform.position;
            var heath = baseBall.maxHealth / 2;
            if (heath <= 0)
                heath = 1;
            
            for (int i = 0; i < 2; i++)
            {
                float t = (float)i / 1; // 0 → 1
                float offset = Mathf.Lerp(-half, half, t);
                float rad = (baseAngle + offset) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                var force = dir * Random.Range(randomForce.x, randomForce.y) * 2;
                
                SpawnBall(baseBall.ballType, newBallId, attack, heath, spawnPosition, force);
            }
        }

        public void SpawnNormalBallByMiniBoss(int ballId, Vector2 spawnPosition)
        {
            var aimDir = Vector2.up;
            float baseAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            float half = angle * 0.5f;
            
            for (int i = 0; i < 2; i++)
            {
                float t = (float)i / 1; // 0 → 1
                float offset = Mathf.Lerp(-half, half, t);
                float rad = (baseAngle + offset) * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                var force = dir * (Random.Range(randomForce.x, randomForce.y) * 2);
                
                SpawnBall(BallType.Normal, ballId, 10, 100, spawnPosition, force);
            }
        }
        
        private void CheckClearWave()
        {
            if (_ballAlive.Count == 0)
            {
                _currentWave++;
                SpawnWave().Forget();
            }
        }
    }
}