using UnityEngine;

namespace QFramework
{
    public class AudioKitWithAddressablesInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            AudioKit.Config.AudioLoaderPool = new AddressablesAudioLoaderPool();
        }
    }
}


