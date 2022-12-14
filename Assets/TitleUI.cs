using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TitleUI : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _root;
    private Label _titleLabel;

    public string Title
    {
        get => _titleLabel.text;
        set => _titleLabel.text = value;
    }

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _root = _document.rootVisualElement;

        _titleLabel = _root.Q<Label>("title");

        _root.Q<Button>("startButton").clicked += OnStartButtonClicked;
        _root.Q<Button>("settingsButton").clicked += OnSettingsButtonClicked;
        _root.Q<Button>("exitButton").clicked += OnExitButtonClicked;
        _root.Q<Slider>("volumeSlider").RegisterValueChangedCallback(OnVolumeSliderValueChanged);
        _root.Q<Slider>("effectSlider").RegisterValueChangedCallback(OnEffectSliderValueChanged);
        _root.Q<Button>("closeSettingsButton").clicked += OnCloseSettingsButtonClicked;
    }

    private void OnCloseSettingsButtonClicked()
    {
        _root.Q<VisualElement>("setting-panel").RemoveFromClassList("show");
    }

    private void OnVolumeSliderValueChanged(ChangeEvent<float> evt)
    {
        Debug.Log(evt.newValue);
    }

    private void OnEffectSliderValueChanged(ChangeEvent<float> evt)
    {
        Debug.Log(evt.newValue);
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start");
    }

    private void OnSettingsButtonClicked()
    {
        _root.Q<VisualElement>("setting-panel").AddToClassList("show");
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit");
    }
}