using System;
using System.Collections;
using System.Collections.ObjectModel;
using Skill;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using MoreMountains.Feedbacks;
using KBluePurple.Util;

public class Player : MonoBehaviour, IHitAble
{
    [SerializeField] private SelectPanel selectPanel;
    [SerializeField] private float dashSpeed;
    [SerializeField] private GameObject dashParticle; 
    [SerializeField] private float bombRadius = 27.5f;
    [SerializeField] private LayerMask bulletLayer;


    [SerializeField] private SkillBase[] skills = new SkillBase[4];
    public ReadOnlyCollection<SkillBase> Skills => new(skills);

    private Camera _mainCamera;

    #region 피드백 관련
    [SerializeField] private MMFeedbacks hitFeedbacks;
    [SerializeField] private MMFeedbacks healFeedbacks;
    [SerializeField] private MMFeedbacks bombFeedback;
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

        StartCoroutine(CoolTime());
    }

    private IEnumerator InvincibilityCheck()
    {
        _isInvincibility = true;
        yield return new WaitForSeconds(_invincibilityTime);
        _isInvincibility = false;
    }

    private IEnumerator CoolTime()
    {
        while (true)
        {
            foreach (var skill in skills)
            {
                if (!skill.IsUnlock) continue;

                skill.CoolTimeLeft -= Time.deltaTime;
                if (skill.CoolTimeLeft <= 0)
                {
                    skill.CoolTimeLeft = 0;
                }
            }

            yield return null;
        }
    }

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

        // unlock keys for debug
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
    }

    public void Dash()
    {
        PoolManager.Instantiate(dashParticle, transform.position, Quaternion.identity);

        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector3(h, 0, v).normalized;
        
        var direction = _mainCamera.transform.TransformDirection(input);
        direction.y = 0;
        transform.position += direction * dashSpeed;
        PoolManager.Instantiate(dashParticle, transform.position, Quaternion.identity);
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
            }
        }
        bombFeedback?.PlayFeedbacks();
    }

    public void Heal()
    {
        _nowHp += 10;
        _nowHp = Mathf.Clamp(_nowHp, 0, _maxHp);

        _hpBar.ChangeHp((float)_nowHp / _maxHp);
        healFeedbacks?.PlayFeedbacks();
    }

    public void Stealth()
    {
        StartCoroutine(StealthCoroutine());
    }

    private IEnumerator StealthCoroutine()
    {
        _isInvincibility = true;
        yield return new WaitForSeconds(3f);
        _isInvincibility = false;
    }

    public bool Hit(int damage)
    {
        if (_isInvincibility) return false;

        StartCoroutine(InvincibilityCheck());

        _nowHp -= damage - _defence;
        _nowHp = Mathf.Clamp(_nowHp, 0, _maxHp);

        _hpBar.ChangeHp((float)_nowHp / _maxHp);
        StartCoroutine(InvincibilityCheck());

        return true;
    }
}