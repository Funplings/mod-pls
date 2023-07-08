using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private ChatscriptData[] m_dailyChatscripts = new ChatscriptData[Constants.DAYS];

    [Header("Configuration")]
    [SerializeField] private float m_secDelayBase = 1;

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
        print(string.Format("User {0} in Channel {1} sent message \"{2}\"", strUser, this.name, strMessage));
    }
}
