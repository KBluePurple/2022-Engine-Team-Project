using System;
using System.Collections;
using Skill;
using UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SelectPanel selectPanel;
    [SerializeField] private float dashSpeed;

    [SerializeField] private SkillBase[] skills = new SkillBase[3];

    private Camera _mainCamera;
    public Action<Vector3> OnPlayerMoved = delegate { };

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        for (var i = 0; i < skills.Length; i++)
        {
            selectPanel.skillPanels[i].SetSkill(skills[i]);
        }

        StartCoroutine(CoolTime());
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

        OnPlayerMoved?.Invoke(transform.position);
    }

    public void Dash()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector3(h, 0, v).normalized;

        var direction = _mainCamera.transform.TransformDirection(input);
        direction.y = 0;
        transform.position += direction * dashSpeed;
    }

    public void Bomb()
    {
    }

    public void Heal()
    {
    }
}