using AchromaticDev.Util;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoSingleton<InGameUI>
{
    private UIDocument _uiDocument;
    private VisualElement _root;

    private TextElement _bossName;
    private VisualElement _bossHealthBar;
    private VisualElement _bossHealthBarFill;

    private TextElement _playerName;
    private VisualElement _playerHealthBar;
    private VisualElement _playerHealthBarFill;
    private VisualElement _playerProfilePicture;

    private VisualElement _playerChargeBar;
    private VisualElement _playerChargeBarFill;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        VisualElement bossInfo = _root.Q<VisualElement>("boss-info");
        _bossName = bossInfo.Q<TextElement>(null, "name");
        _bossHealthBar = bossInfo.Q<VisualElement>(null, "health-bar");
        _bossHealthBarFill = _bossHealthBar.Q<VisualElement>(null, "fill");

        VisualElement playerInfo = _root.Q<VisualElement>("player-info");
        _playerName = playerInfo.Q<TextElement>(null, "name");
        _playerHealthBar = playerInfo.Q<VisualElement>(null, "health-bar");
        _playerHealthBarFill = _playerHealthBar.Q<VisualElement>(null, "fill");
        _playerProfilePicture = _root.Q<VisualElement>("profile-picture");

        _playerChargeBar = _root.Q<VisualElement>("charge-bar");
        _playerChargeBarFill = _playerChargeBar.Q<VisualElement>(null, "fill");
    }

    public void SetBossInfo(string bossName, float health, float maxHealth)
    {
        _bossName.text = bossName;
        _bossHealthBarFill.style.width = health / maxHealth * 100;
    }

    public void UpdateBossHealth(float health, float maxHealth)
    {
        _bossHealthBarFill.style.width = health / maxHealth * 100;
    }

    public void SetPlayerInfo(string playerName, float health, float maxHealth, Texture2D profilePicture)
    {
        _playerName.text = playerName;
        _playerHealthBarFill.style.width = health / maxHealth * 100;
        _playerProfilePicture.style.backgroundImage = new StyleBackground(profilePicture);
    }

    public void UpdatePlayerHealth(float health, float maxHealth)
    {
        _playerHealthBarFill.style.width = health / maxHealth * 100;
    }

    public void UpdateChargeGage(float charge, float maxCharge)
    {
        _playerChargeBarFill.style.height = Length.Percent(charge / maxCharge * 100);
    }
}