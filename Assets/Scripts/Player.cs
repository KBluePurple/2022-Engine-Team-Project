using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DG.Tweening;
using Skill;
using UI;
using UnityEngine;
using MoreMountains.Feedbacks;
using KBluePurple.Util;
using Manager;

public class Player : MonoBehaviour, IHitAble
{
    [SerializeField] private SelectPanel selectPanel;
    [SerializeField] private float dashSpeed;
    [SerializeField] private GameObject dashParticle;
    [SerializeField] private float bombRadius = 27.5f;
    [SerializeField] private LayerMask bulletLayer;
    private Renderer playerRenderer;


    [SerializeField] private SkillBase[] skills = new SkillBase[4];
    public ReadOnlyCollection<SkillBase> Skills => new(skills);

    private Camera _mainCamera;

    #region 피드백 관련

    [SerializeField] private MMFeedbacks hitFeedbacks;
    [SerializeField] private MMFeedbacks healFeedbacks;
    [SerializeField] private MMFeedbacks bombFeedbacks;
    [SerializeField] private MMFeedbacks stealthFeedbacks;

    #endregion

    #region 스테이터스 관련

    private int _maxHp = 100;
    private int _nowHp;
    private int _defence = 0;

    private float _invincibilityTime = 0.25f;
    private bool _isInvincibility = false;

    #endregion

    private HpBar _hpBar;

    private void OnDrawGizmos()
    {
        // bomb radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombRadius);
        // Gizmos.DrawWireSphere(transform.position, _bombRadius);
    }

    private void Awake()
    {
        playerRenderer = GetComponent<Renderer>();
        _mainCamera = Camera.main;
        _hpBar = transform.Find("Health Bar Canvas").GetComponent<HpBar>();
        _nowHp = _maxHp;
    }

    private void Start()
    {
        for (var i = 0; i < skills.Length; i++)
        {
            selectPanel.skillPanels[i].SetSkill(skills[i]);
        }
    }

    private IEnumerator InvincibilityCheck()
    {
        _isInvincibility = true;
        yield return new WaitForSeconds(_invincibilityTime);
        _isInvincibility = false;
    }

    private readonly List<Renderer> _renderers = new();
    private static readonly int Surface = Shader.PropertyToID("_Surface");
    private static readonly int Color1 = Shader.PropertyToID("_BaseColor");
    private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            skills[0].UseSkill(this);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            skills[1].UseSkill(this);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            skills[2].UseSkill(this);
        }
        else if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            skills[3].UseSkill(this);
        }

        foreach (var skill in skills)
        {
            skill.Update();
        }

        var transform1 = _mainCamera.transform;
        var position = transform1.position;
        var position1 = transform.position;

        var rayHits = Physics.OverlapCapsule(position, position1, 1, LayerMask.GetMask("Tree"));
        var rendererList = new List<Renderer>();
        foreach (var rayHit in rayHits)
        {
            if (rayHit.GetComponent<Collider>().TryGetComponent(out Renderer renderer))
            {
                rendererList.Add(renderer);
            }
        }

        foreach (var renderer in _renderers)
        {
            if (rendererList.Contains(renderer) && renderer.material.GetFloat(Surface) <= 0)
            {
                renderer.material.SetFloat(Surface, 1);
                renderer.material.SetFloat(ZWrite, 0);

                renderer.material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
            else if (!rendererList.Contains(renderer) && renderer.material.GetFloat(Surface) >= 1)
            {
                renderer.material.SetFloat(Surface, 0);
                renderer.material.SetFloat(ZWrite, 1);
                renderer.material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            }
        }

        _renderers.Clear();
        _renderers.AddRange(rendererList);

        // unlock keys for debug
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            skills[0].Unlock();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skills[1].Unlock();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            skills[2].Unlock();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            skills[3].Unlock();
        }
#endif
    }

    public void Dash()
    {
        PoolManager.Instantiate(dashParticle, transform.position, Quaternion.identity);
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector3(h, 0, v).normalized;
        var direction = _mainCamera.transform.TransformDirection(input);
        direction.y = 0;
        PoolManager.Instantiate(dashParticle, transform.position + direction * dashSpeed, Quaternion.identity);
        transform.DOMove(transform.position + direction * dashSpeed, 0.05f).SetEase(Ease.Linear);
    }

    public void Bomb()
    {
        var col = Physics.OverlapSphere(transform.position, bombRadius, bulletLayer);
        Debug.Log(col.Length);

        foreach (Collider obj in col)
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.DestoryAction();
                GameManager.Instance.nowScore += 0.1f;
            }
        }

        GameManager.Instance.OnScoreUpdate.Invoke();
        bombFeedbacks?.PlayFeedbacks();
    }

    public void Heal()
    {
        _nowHp += 10;
        _nowHp = Mathf.Clamp(_nowHp, 0, _maxHp);

        _hpBar.ChangeHp((float)_nowHp / _maxHp);
        healFeedbacks?.PlayFeedbacks();
    }

    public void Stealth(float activeTime)
    {
        StartCoroutine(StealthCoroutine(activeTime));
        stealthFeedbacks?.PlayFeedbacks();
    }

    private IEnumerator StealthCoroutine(float activeTime)
    {
        _isInvincibility = true;
        var material = playerRenderer.material;
        material.color = Color.gray;
        yield return new WaitForSeconds(activeTime);
        material.color = Color.white;
        _isInvincibility = false;
    }

    public bool Hit(int damage)
    {
        if (_isInvincibility) return false;

        StartCoroutine(InvincibilityCheck());

        _nowHp -= damage - _defence;
        _nowHp = Mathf.Clamp(_nowHp, 0, _maxHp);

        if (_nowHp <= 0)
        {
            _nowHp = 0;
            Die();
        }

        _hpBar.ChangeHp((float)_nowHp / _maxHp);

        hitFeedbacks?.PlayFeedbacks();

        return true;
    }

    private void Die()
    {
        GameManager.Instance.GameOver();
        GetComponent<MeshRenderer>().material.DOFade(0, 1f).SetUpdate(true);
    }
}