using Cysharp.Threading.Tasks;
using Runtime.Service;
using UnityEngine;

namespace Runtime.Manager
{
    public class Init : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Helper.LoadScene().Forget();
        }
    }
}