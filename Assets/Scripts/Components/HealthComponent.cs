using System;
using System.Collections;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float m_flashGap;
    [SerializeField] private SpriteRenderer m_icon;
    [SerializeField] private Color m_flashColour;

    public Action<int> HealthRecovered;
    public Action<int> DamageTaken;
    public Action HealthDepleted;

    private float m_initialHealth;
    private float m_invunerableTime;
    private float m_lifeTime;
    private float m_recoveryTime;
    private float m_currentHealth;
    private bool IsInvunerable;
    private WaitForSeconds m_flashGapSeconds;
    private Color m_originalColour;
    private float m_invunerableTimer;
    private WaitForSeconds m_lifeTimeSeconds;
    private Coroutine m_lifeTimeCountdown;
    private WaitForSeconds m_recoverytimeSeconds;
    private Coroutine m_recoveryHealthCoroutine;

    private bool IsAlive => m_currentHealth > 0;

    public void Setup(int health, float invunerableTime, float lifeTime = -1, float recoveryTime = -1f)
    {
        m_lifeTimeCountdown = null;
        m_initialHealth = health;
        m_currentHealth = m_initialHealth;
        m_invunerableTime = invunerableTime;
        m_originalColour = m_icon.color;
        m_flashGapSeconds = new WaitForSeconds(m_flashGap);
        IsInvunerable = true; // so they dont die on spawn.
        m_lifeTime = lifeTime;
        m_lifeTimeSeconds = new WaitForSeconds(m_lifeTime);
        m_recoveryTime = recoveryTime;
        m_recoverytimeSeconds = new WaitForSeconds(m_recoveryTime);     
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
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        if (m_lifeTimeCountdown != null)
        {
            StopCoroutine(m_lifeTimeCountdown);
        }
        if (m_recoveryHealthCoroutine != null)
        {
            StopCoroutine(m_recoveryHealthCoroutine);
        }
        if (m_lifeTime > 0)
        {
            m_lifeTimeCountdown = StartCoroutine(CountDownLife());
        }

        if (m_recoveryTime > 0)
        {
            m_recoveryHealthCoroutine = StartCoroutine(RecoverHealth());
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

    private IEnumerator RecoverHealth()
    {
        while (true)
        {
            yield return m_recoverytimeSeconds;
            if (this.gameObject != null || this.isActiveAndEnabled)
            {
                if (m_currentHealth + 1 > m_initialHealth)
                    continue;
                m_currentHealth++;
                HealthRecovered?.Invoke(1);
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

    internal void Setup()
    {
        throw new NotImplementedException();
    }
}
