using Cysharp.Threading.Tasks;
using Runtime.ConfigData;
using Runtime.Constant;
using Runtime.Pool;
using Runtime.Stat;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ModalSelectWeapon modalSelectWeapon;
        [SerializeField] private PoolService poolService;
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private BuffManager buffManager;
        [SerializeField] private PlayerManager playerManager;

        private void Start()
        {
            modalSelectWeapon.gameObject.SetActive(true);
            modalSelectWeapon.SetSelected((weaponConfig) =>
            {
                modalSelectWeapon.gameObject.SetActive(false);
                InitializeAsync(weaponConfig).Forget();
            });
        }

        private async UniTask InitializeAsync(WeaponConfig weaponConfig)
        {
            await UniTask.Delay(500);
            await poolService.Initialize();
            var weaponStat = new WeaponStat();
            weaponManager.Initialize(weaponStat, weaponConfig);
            buffManager.SetWeaponStat(weaponStat);
            playerManager.Initialize(weaponStat, weaponConfig.weaponType);
            await spawnManager.Initialize();
        }
    }
}