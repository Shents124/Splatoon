using Cysharp.Threading.Tasks;
using Runtime.UI;
using ZBase.UnityScreenNavigator.Core.Modals;

namespace Runtime.Service
{
    public enum ModalType
    {
        ModalShowBuff,
    }
    
    public static class UiService
    {
        public static async UniTask OpenModalAsync(
            ModalType modalType,
            bool playAnimation = true,
            bool? closeWhenClickOnBackDrop = true,
            float? backDropAlpha = 0.9f,
            string modalLayer = UIConstant.Modals,
            params object[] args)
        {
            var container = ModalContainer.Find(modalLayer);
            if (container.IsInTransition)
                return;
            
            string resourcePath = modalType.ToString();
            var modalOption = new ModalOptions(resourcePath, playAnimation, backdropAlpha: backDropAlpha,
                closeWhenClickOnBackdrop: closeWhenClickOnBackDrop);
            await container.PushAsync(modalOption, args);
        }

        public static void CloseModal(bool playAnimation = true, bool expectNewUI = false)
        {
            ModalContainer.Find(UIConstant.Modals).Pop(playAnimation);
        }
    }
}