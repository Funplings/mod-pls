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
    [SerializeField] private List<Rule> m_ChannelRules;

    private ChatscriptData m_chatscriptCurrent;
    private int m_iCommand = 0;

    private float m_timeNextCommand = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_chatscriptCurrent = m_dailyChatscripts[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_chatscriptCurrent &&
            m_iCommand < m_chatscriptCurrent.m_commands.Count &&
            Time.time >= m_timeNextCommand)
        {
            // Read in next command

            ChatscriptCommand chatscriptCommand = m_chatscriptCurrent.m_commands[m_iCommand++];

            switch (chatscriptCommand.m_type)
            {
                case ChatscriptCommand.TYPE.MESSAGE:
                    MessageCommand messageCommand = (MessageCommand) chatscriptCommand;
                    PushMessage(messageCommand.m_strUser, messageCommand.m_strMessage);
                    m_timeNextCommand = Time.time + m_secDelayBase;
                    break;

                case ChatscriptCommand.TYPE.PAUSE:
                    PauseCommand pauseCommand = (PauseCommand) chatscriptCommand;
                    m_timeNextCommand = Time.time + pauseCommand.m_secPause;
                    break;

                default:
                    throw new System.ArgumentException();
            }
        }
    }

    void PushMessage(string strUser, string strMessage)
    {
        // Create new message
        GameObject message = Instantiate(m_MessagePrefab, m_MessagesContainer.transform);
        message.GetComponent<Message>().setMessage(strUser, strMessage, "4:00 AM", null, m_ChannelName);
    }

    public void selectChannel()
    {
        m_MessagesContainer.SetActive(true);
    }

    public void deselectChannel()
    {
        m_MessagesContainer.SetActive(false);
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
        Debug.Log("HOO AH");
        return m_ChannelRules;
    }
}
