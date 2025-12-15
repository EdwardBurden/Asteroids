using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float m_flashGap;
    [SerializeField] private SpriteRenderer m_icon;
    [SerializeField] private Color m_flashColour;
    private float m_initialHealth;
    private float m_invunerableTime;
    private float m_currentHealth;
    private bool IsAlive => m_currentHealth > 0;
    private bool IsInvunerable;
    public Action<int> DamageTaken;
    public Action HealthDepleted;

    private WaitForSeconds m_flashGapSeconds;
    private Color m_originalColour;
    private float m_invunerableTimer;

    public void Setup(int health, float invunerableTime)
    {
        m_initialHealth = health;
        m_currentHealth = m_initialHealth;
        m_invunerableTime = invunerableTime;
        IsInvunerable = false;
        m_originalColour = m_icon.color;
        m_flashGapSeconds =  new WaitForSeconds(m_flashGap);
        IsInvunerable = true; // give grace 
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive || IsInvunerable)
            return;

        if (m_currentHealth - amount < 0)
        {
            m_currentHealth = 0;
        }
        else
        {
            m_currentHealth -= amount;
        }

        DamageTaken?.Invoke(amount);

        if (!IsAlive)
        {
            HealthDepleted?.Invoke();
            return;
        }
        IsInvunerable = true;
        StartCoroutine(InvunerableFlashAnim());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (!IsAlive)
            return;

        if (IsInvunerable)
        {
            m_invunerableTimer += Time.deltaTime;
            if (m_invunerableTimer > m_invunerableTime)
            {
                IsInvunerable = false;
                m_invunerableTimer = 0;
            }
        }
    }

    IEnumerator InvunerableFlashAnim()
    {
        bool flashToggle = false;
        while (IsInvunerable)
        {  
            flashToggle = !flashToggle;
            m_icon.color = flashToggle ? m_originalColour : m_flashColour;
            yield return m_flashGapSeconds;
        }
        m_icon.color = m_originalColour;
    }
}
