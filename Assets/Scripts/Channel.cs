using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float m_secDelayBase = 1;
    [SerializeField] private string m_ChannelName;
    [SerializeField] private bool m_isAnnouncementsChannel = false;
    [SerializeField] private List<Rule> m_ChannelRules;

    private ChatscriptData m_chatscriptCurrent;
    private int m_iCommand = 0;

    private float m_timeNextCommand = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_chatscriptCurrent = m_dailyChatscripts[0];
        if (m_isAnnouncementsChannel)
        {
            while (m_chatscriptCurrent && m_iCommand < m_chatscriptCurrent.m_commands.Count)
            {
                ReadNextCommand();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ChannelManager.Instance.GetDayStarted())
        {
            if (m_chatscriptCurrent &&
            m_iCommand < m_chatscriptCurrent.m_commands.Count &&
            Time.time >= m_timeNextCommand)
            {
                ReadNextCommand();
            }
        }
    }

    void ReadNextCommand()
    {
        // Read in next command

        ChatscriptCommand chatscriptCommand = m_chatscriptCurrent.m_commands[m_iCommand++];

        switch (chatscriptCommand.m_type)
        {
            case ChatscriptCommand.TYPE.MESSAGE:
                MessageCommand messageCommand = (MessageCommand)chatscriptCommand;
                PushMessage(messageCommand.m_strUser, ChannelManager.Instance.SpriteForUser(messageCommand.m_strUser), messageCommand.m_strMessage);
                m_timeNextCommand = Time.time + m_secDelayBase;
                break;

            case ChatscriptCommand.TYPE.PAUSE:
                PauseCommand pauseCommand = (PauseCommand)chatscriptCommand;
                m_timeNextCommand = Time.time + pauseCommand.m_secPause;
                break;

            default:
                throw new System.ArgumentException();
        }
    }

    public void PushMessage(string strUser, Sprite spriteProfile, string strMessage)
    {
        // Create new message
        GameObject message = Instantiate(m_MessagePrefab, m_MessagesContainer.transform);
        message.GetComponent<Message>().setMessage(strUser, strMessage, ChannelManager.Instance.CurrentGameTime(), spriteProfile, m_ChannelName);
    }

    public void selectChannel()
    {
        gameObject.SetActive(true);
    }

    public void deselectChannel()
    {
        gameObject.SetActive(false);
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
}
