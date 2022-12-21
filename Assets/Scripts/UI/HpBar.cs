using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image fillArea;

    public void ChangeHp(float hpPercentage)
    {
        fillArea.fillAmount = hpPercentage;
    }
}