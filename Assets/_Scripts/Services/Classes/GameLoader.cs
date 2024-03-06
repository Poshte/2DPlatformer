using UnityEngine;

public class GameLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        ServiceLocator.Initialize();

        RegisterServices();
    }

    private static void RegisterServices()
    {
        ServiceLocator.Instance.Register<IColorService>(new ColorService());
        ServiceLocator.Instance.Register<IScreenFadeService>(new ScreenFadeService());
    }
}
