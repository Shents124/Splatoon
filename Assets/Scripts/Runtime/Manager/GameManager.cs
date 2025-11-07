using Cysharp.Threading.Tasks;
using Runtime.ConfigData;
using Runtime.Pool;
using Runtime.Stat;
using UnityEngine;

namespace Runtime.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PoolService poolService;
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private WeaponConfig weaponConfig;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private BuffManager buffManager;

        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await poolService.Initialize();
            var weaponStat = new WeaponStat();
            weaponManager.Initialize(weaponStat, weaponConfig);
            buffManager.SetWeaponStat(weaponStat);
            await spawnManager.Initialize();
        }
    }
}