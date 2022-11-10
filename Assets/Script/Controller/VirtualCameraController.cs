using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraController : MonoBehaviour
{
    public float FOV
    {
        get => _virtualCamera.m_Lens.FieldOfView;
        set => _virtualCamera.m_Lens.FieldOfView = value;
    }

    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    
    // ReSharper disable once InconsistentNaming
    public TweenerCore<float, float, FloatOptions> DOFieldOfView(float endValue, float duration)
    {
        DOTween.Kill(this);
        return DOTween.To(() => FOV, x => FOV = x, endValue, duration);
    }
}