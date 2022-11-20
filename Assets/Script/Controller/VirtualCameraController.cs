using System;
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

    private void FixedUpdate()
    {
        FovCalc();
    }

    // ReSharper disable once InconsistentNaming
    public TweenerCore<float, float, FloatOptions> DOFieldOfView(float endValue, float duration)
    {
        return DOTween.To(() => FOV, x => FOV = x, endValue, duration);
    }    
    
    private Vector3 _lastPosition = Vector3.zero;
    
    private void FovCalc()
    {
        var position = transform.position;
        var distance = Vector3.Distance(position, _lastPosition);
        var fov = 60 + distance * 10;
        fov = Mathf.Clamp(fov, 60, 100);
        _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(_virtualCamera.m_Lens.FieldOfView,
            fov, 100 * Time.deltaTime);
        _lastPosition = position;
    }
}