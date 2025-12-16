using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IGameSystem
{
    [SerializeField] private Transform m_playerHealthRoot;
    [SerializeField] private GameObject m_playerHealthPrefab;
    [SerializeField] private GameObject m_winScreen;
    [SerializeField] private GameObject m_loseScreen;

    [SerializeField] private TMP_Text m_score;
    [SerializeField] private TMP_Text m_level;

    private Game.GameStateData m_cachedState;
    private GameObject[] m_healthObjects;

    public void Startup()
    {
        Debug.Log($"{nameof(HUD)} StartUp");
        TearDown();
        var playerData = Game.Instance.PlayerController.CurrentPlayerData;
        m_healthObjects = new GameObject[playerData.Health];
        for (int i = 0; i < playerData.Health; i++)
        {
            var icon = GameObject.Instantiate(m_playerHealthPrefab, m_playerHealthRoot);
            icon.GetComponent<Image>().sprite = playerData.PlayerIconAlive; //todo remove expensive getcomponent call
            m_healthObjects[i] = icon;
        }
        m_score.text = "0";
        m_level.text = "0";
    }

    public void ShutDown()
    {
        Debug.Log($"{nameof(HUD)} ShutDown");
        TearDown();
        m_winScreen.SetActive(false);
        m_loseScreen.SetActive(false);
    }

    private void TearDown()
    {
        Utils.DestoryAllChildren(m_playerHealthRoot);
    }

    public void UpdateHUD(Game.GameStateData m_gameStateData) //todo break up as callbacks instead.
    {
        if (m_cachedState.Score != m_gameStateData.Score)
        {
            m_score.text = m_gameStateData.Score.ToString();
        }

        if (m_cachedState.Level != m_gameStateData.Level)
        {
            m_level.text = m_gameStateData.Level.ToString();
        }

        if (m_cachedState.PlayerHealthRemaining != m_gameStateData.PlayerHealthRemaining)
        {
            var playerData = Game.Instance.PlayerController.CurrentPlayerData;
            for (int i = 0; i < m_healthObjects.Length; i++)
            {
                var alive = m_gameStateData.PlayerHealthRemaining > i;
                var icon = alive ? playerData.PlayerIconAlive : playerData.PlayerIconDead;
                m_healthObjects[i].GetComponent<Image>().sprite = icon; //too better
            }
        }

        m_cachedState = m_gameStateData;
    }

    public void Action_Replay()
    {
        Game.Instance.Replay();
    }

    public void ShowWonScreen()
    {
        m_winScreen.SetActive(true);
    }

    public void HideWonScreen()
    {
        m_winScreen.SetActive(false);
    }

    public void ShowLostScreen()
    {
        m_loseScreen.SetActive(true);
    }

    public void HideLostScreen()
    {
        m_loseScreen.SetActive(false);
    }
}
