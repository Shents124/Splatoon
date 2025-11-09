using Runtime.Service;
using UnityEngine;

namespace Runtime.Manager
{
    public class Init : MonoBehaviour
    {
        private void Start()
        {
            Helper.LoadScene();
        }
    }
}