using System;
using Cysharp.Threading.Tasks;
using Runtime.Constant;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Runtime.Service
{
    public static class Helper
    {
        private static AsyncOperationHandle<SceneInstance> loadHandle;
        
        public static Color GetColorByBuffRarity(BuffRarity buffRarity)
        {
            switch (buffRarity)
            {
                case BuffRarity.Normal:
                    return Color.white;
                case BuffRarity.Legendary:
                    return Color.yellow;
                case BuffRarity.Mythic:
                    return Color.red;
            }
            
            return Color.white;
        }

        public static async UniTask LoadScene()
        {
            loadHandle = Addressables.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
            await loadHandle;
            SceneManager.SetActiveScene(loadHandle.Result.Scene);
        }
        
        public static async UniTask RestartGame()
        {
            await Addressables.UnloadSceneAsync(loadHandle);
            await LoadScene();
            Time.timeScale = 1;
        }
    }
}