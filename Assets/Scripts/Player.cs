using System;
using System.Collections;
using Skill;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using MoreMountains.Feedbacks;
using KBluePurple.Util;

public class Player : MonoBehaviour, HitAble
{
    [SerializeField] private SelectPanel selectPanel;
    [SerializeField] private float dashSpeed;
    [SerializeField] private GameObject dashParticle; 
    private float bombRadius = 27.5f;
    [SerializeField] private LayerMask bulletLayer;


    [FormerlySerializedAs("_skills")] [SerializeField]
    private SkillBase[] skills = new SkillBase[3];

    private Camera _mainCamera;

    #region 피드백 관련
    [SerializeField] private MMFeedbacks hitFeedbacks;
    [SerializeField] private MMFeedbacks healFeedbacks;
    [SerializeField] private MMFeedbacks bombFeedback;
    #endregion

    #region 스테이터스 관련
    private int maxHp = 100;
    private int nowHp;
    private int defence = 0;

    private float invincivilityTime = 0.25f;
    private bool isInvincivility = false;
    #endregion

    private HpBar hpBar;

    private void Awake()
    {
        _mainCamera = Camera.main;
        hpBar = transform.Find("Health Bar Canvas").GetComponent<HpBar>();
        nowHp = maxHp;
    }

    private void Start()
    {
        for (var i = 0; i < skills.Length; i++)
        {
            selectPanel.skillPanels[i].SetSkill(skills[i]);
        }

        StartCoroutine(CoolTime());
    }

    IEnumerator InvincivilityCheck()
    {
        isInvincivility = true;
        yield return new WaitForSeconds(invincivilityTime);
        isInvincivility = false;
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
        Collider[] col = Physics.OverlapSphere(transform.position, bombRadius, bulletLayer);
        
        foreach(Collider obj in col)
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
        nowHp += 10;
        nowHp = Mathf.Clamp(nowHp, 0, maxHp);

        hpBar.ChangeHp((float)nowHp / maxHp);
        healFeedbacks?.PlayFeedbacks();
    }

    public void Hide()
    {

    }

    public void Hit(int damage)
    {
        if (isInvincivility == true) return;

        StartCoroutine(InvincivilityCheck());

        nowHp -= damage - defence;
        nowHp = Mathf.Clamp(nowHp, 0, maxHp);
        
        hpBar.ChangeHp((float)nowHp / maxHp);
        hitFeedbacks?.PlayFeedbacks();
    }
}