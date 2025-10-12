using UnityEngine;
using System.Collections;

public class ShopSkinWindowInitializer : IGameInitializerStep
{
    private SkinPanel shopWindow;
    private RewardGemsActivator _rewardGemsActivator;
    private SkinPanel _prefab;
    private Transform _parent;
    private IInput _input;

    public ShopSkinWindowInitializer(SkinPanel prefab, Transform parent, IInput input, RewardGemsActivator rewardGemsActivator)
    {
        _prefab = prefab;
        _parent = parent;
        _input = input;
        _rewardGemsActivator = rewardGemsActivator;
    }

    public IEnumerator Initialize()
    {
        shopWindow = Object.Instantiate(_prefab, _parent);
        shopWindow.Initialize(_input);
        _rewardGemsActivator.InitializeSkinPanel(shopWindow);

        if (shopWindow != null && shopWindow.gameObject.activeSelf)
            shopWindow.gameObject.SetActive(false);
        
        yield break;
    }
    
    public void SetWindowsConnection(GameplayCanvas gameplayCanvas) => gameplayCanvas.DetectShopWindow(shopWindow.GetComponent<ClosableWindow>());
}