using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
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

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        
        var bossInfo = _root.Q<VisualElement>("boss-info");
        _bossName = bossInfo.Q<TextElement>(".name");
        _bossHealthBar = bossInfo.Q<VisualElement>(".health-bar");
        _bossHealthBarFill = _bossHealthBar.Q<VisualElement>(".fill");
        
        var playerInfo = _root.Q<VisualElement>("player-info");
        _playerName = playerInfo.Q<TextElement>(".name");
        _playerHealthBar = playerInfo.Q<VisualElement>(".health-bar");
        _playerHealthBarFill = _playerHealthBar.Q<VisualElement>(".fill");
        _playerProfilePicture = _root.Q<VisualElement>("profile-picture");
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
}
