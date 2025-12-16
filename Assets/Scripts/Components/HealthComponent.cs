using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    //todo clean ups
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

    private float m_lifeTime;
    private WaitForSeconds m_lifeTimeSeconds;
    private Coroutine m_lifeTimeCountdown;

    public void Setup(int health, float invunerableTime, float lifeTime = -1)
    {
        m_lifeTimeCountdown = null;
        m_initialHealth = health;
        m_currentHealth = m_initialHealth;
        m_invunerableTime = invunerableTime;
        m_originalColour = m_icon.color;
        m_flashGapSeconds = new WaitForSeconds(m_flashGap);
        IsInvunerable = true; // give grace 
        m_lifeTime = lifeTime;
        m_lifeTimeSeconds = new WaitForSeconds(m_lifeTime);
    }

    private void ForceKill()
    {
        m_currentHealth = 0;
        HealthDepleted?.Invoke();
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
        if (m_lifeTimeCountdown != null)
        {
            StopCoroutine(m_lifeTimeCountdown);
        }
    }

    private void OnEnable()
    {
        if (m_lifeTimeCountdown != null)
        {
            StopCoroutine(m_lifeTimeCountdown);
        }
        if (m_lifeTime > 0)
        {
            m_lifeTimeCountdown = StartCoroutine(CountDownLife());
        }
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

    private IEnumerator CountDownLife()
    {
        yield return m_lifeTimeSeconds;
        if (this.gameObject != null || this.isActiveAndEnabled)
        {
            ForceKill();
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
