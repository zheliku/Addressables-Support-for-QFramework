using UnityEngine;

namespace QFramework
{
    public class UIKitWithAddressablesInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            UIKit.Config.PanelLoaderPool = new AddressablesPanelLoaderPool();
        }
    }
}


