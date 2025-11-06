using System;
using AssetLoader;
using Runtime.ConfigData;
using Runtime.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Ball
{
    public class MiniBoss : BaseEnemy
    {
        public float moveSpeed = 2f;              // Tốc độ ngang cơ bản
        public float floatAmplitude = 1f;         // Biên độ nhấp nhô
        public float floatFrequency = 1f;         // Tần số dao động
        public float randomnessScale = 0.5f;      // Mức nhiễu random

        public float minX = -4.8f;
        public float maxX = 4.8f;
        public float minY = 4f;
        public float maxY = 8f;
        
        private Rigidbody2D _rb;
        private float _noiseSeedY;
        [SerializeField]
        private int directionX = 1; // 1 = phải, -1 = trái

        private SpawnNormalBallConfig _spawnConfig;
        private float _timSpawn;
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f; // Không rơi
            _rb.freezeRotation = true;
        }

        public override void Initialize(SpawnManager spawnManager, BallData ballData, string key, Action<BaseEnemy> onDead)
        {
            base.Initialize(spawnManager, ballData, key, onDead);
            // Mỗi bóng 1 seed khác nhau
            _noiseSeedY = Random.Range(0f, 1000f);
            floatAmplitude = Random.Range(0.6f, 1.2f);
            floatFrequency = Random.Range(0.6f, 1.2f);
            moveSpeed = Random.Range(1.8f, 2.5f);
            var x = Random.Range(0, 2);
            directionX = x > 0 ? 1 : -1;
            var config = AssetLoaderService.LoadCsv<MiniBossSpawnConfig>();
            _spawnConfig = config.GetConfig(ballData.id);
            _timSpawn = 0;
        }

        private void Update()
        {
            _timSpawn += Time.deltaTime;
            if (_timSpawn >= _spawnConfig.spawnInterval)
            {
                _timSpawn = 0;
                spawnManager.SpawnNormalBallByMiniBoss(_spawnConfig.ballId, transform.position);
            }
        }

        void FixedUpdate()
        {
            float time = Time.time;

            // Dao động theo trục Y (nhấp nhô + nhiễu)
            float waveY = Mathf.Sin(time * floatFrequency) * floatAmplitude * Time.deltaTime;
            float noiseY = (Mathf.PerlinNoise(_noiseSeedY, time * 0.5f) - 0.5f) * randomnessScale;
            float targetY = Mathf.Clamp(_rb.position.y + waveY + noiseY, minY, maxY);

            // Xử lý giới hạn X
            float targetX = _rb.position.x + moveSpeed * directionX * Time.fixedDeltaTime;

            if (targetX >= maxX)
            {
                targetX = maxX;
                directionX = -1; // quay đầu sang trái
            }
            else if (targetX <= minX)
            {
                targetX = minX;
                directionX = 1; // quay đầu sang phải
            }

            Vector2 nextPos = new Vector2(targetX, targetY);
            _rb.MovePosition(nextPos);
        }
    }
}