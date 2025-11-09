using Cysharp.Threading.Tasks;
using Runtime.Service;
using UnityEngine.UI;
using ZBase.UnityScreenNavigator.Core.Modals;

namespace Runtime.UI
{
    public class ModalLose : Modal
    {
        public Button restartButton;

        protected override void Awake()
        {
            base.Awake();
            restartButton.onClick.AddListener(OnClickedRestart);
        }

        private void OnClickedRestart()
        {
            Helper.RestartGame().Forget();
            UiService.CloseModal();
        }
    }
}