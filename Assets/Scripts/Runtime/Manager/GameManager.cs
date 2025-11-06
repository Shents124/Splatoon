using Cysharp.Threading.Tasks;
using Runtime.ConfigData;
using Runtime.Pool;
using UnityEngine;

namespace Runtime.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PoolService poolService;
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private WeaponConfig weaponConfig;
        [SerializeField] private SpawnManager spawnManager;

        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await poolService.Initialize();
            weaponManager.Initialize(weaponConfig);
            await spawnManager.SpawnWave();
        }
    }
}