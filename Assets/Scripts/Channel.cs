using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Channel : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject m_MessagePrefab;

    [Header("References")]
    [SerializeField]
    private ChatscriptData[] m_dailyChatscripts = new ChatscriptData[Constants.DAYS];
    [SerializeField]
    private GameObject m_MessagesContainer;
    [SerializeField]
    private GameObject m_ChannelButtonObject;

    [Header("Configuration")]
    [SerializeField] private string m_ChannelName;
    [SerializeField] public bool m_isAnnouncementsChannel = false;
    [SerializeField] private List<Rule> m_ChannelRules;

    private ChatscriptData m_chatscriptCurrent;
    private int m_iMessage = 0;

    private float m_timeNextOverride = 0; // Used for announcements while time is frozen

    // Update is called once per frame
    void Update()
    {
        bool announcementsMessage = m_chatscriptCurrent && m_isAnnouncementsChannel && Time.time >= m_timeNextOverride && m_iMessage < m_chatscriptCurrent.m_messages.Count;

        if (announcementsMessage)
        {
            MessageCommand messageCommand = m_chatscriptCurrent.m_messages[m_iMessage++];
            SendMessage(messageCommand);
            m_timeNextOverride = Time.time + 4.0f;
            return;
        }

        if (m_chatscriptCurrent && m_iMessage < m_chatscriptCurrent.m_messages.Count)
        {
            MessageCommand messageCommand = m_chatscriptCurrent.m_messages[m_iMessage];

            float hourCurrent = ChannelManager.Instance.CurrentGameTime();

            if (!ChannelManager.Instance.IsTimeFrozen() && hourCurrent > messageCommand.m_hourSent)
            {
                SendMessage(messageCommand);
                m_iMessage++;
            }
        }
    }

    void SendMessage(MessageCommand messageCommand)
    {
        PushMessage(messageCommand.m_strUser, ChannelManager.Instance.SpriteForUser(messageCommand.m_strUser), messageCommand.m_strMessage);
    }

    public void PushMessage(string strUser, Sprite spriteProfile, string strMessage)
    {
        // Create new message
        GameObject message = Instantiate(m_MessagePrefab, m_MessagesContainer.transform);
        message.GetComponent<Message>().setMessage(strUser, strMessage, ChannelManager.Instance.CurrentGameTimeString(), spriteProfile, m_ChannelName);
    }

    public void selectChannel()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void deselectChannel()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public string getChannelName()
    {
        return m_ChannelName;
    }

    public GameObject getChannelButtonObject()
    {
        return m_ChannelButtonObject;
    }

    public List<Rule> getChannelRules()
    {
        return m_ChannelRules;
    }

    public void SetupForDay(int day)
    {
        m_chatscriptCurrent = m_dailyChatscripts[day];
        m_iMessage = 0;

        if (m_chatscriptCurrent)
        {
            m_timeNextOverride = 0;
        }

        getChannelButtonObject().SetActive(m_chatscriptCurrent);
        gameObject.SetActive(m_chatscriptCurrent);
    }
}
