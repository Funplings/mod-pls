using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatscriptData", menuName = "mod-pls/ChatscriptData", order = 0)]
public class ChatscriptData : ScriptableObject
{
    [SerializeField] 
    [SerializeReference]
    public List<MessageCommand> m_messages = new List<MessageCommand>();
}

[System.Serializable]
public abstract class ChatscriptCommand
{
    public enum TYPE
    {
        MESSAGE,
        HOUR
    }

    public TYPE m_type;
}

[System.Serializable]
public class MessageCommand : ChatscriptCommand
{
    public MessageCommand(string strUser, string strMessage)
    {
        m_type = TYPE.MESSAGE;

        m_strUser = strUser;
        m_strMessage = strMessage;
    }

    public string m_strUser;
    public string m_strMessage;
    public float m_hourSent;
}

// Intermediate command

[System.Serializable]
public class HourCommand : ChatscriptCommand
{
    public HourCommand(float hour)
    {
        m_type = TYPE.HOUR;

        m_hour = hour;
    }

    public float m_hour;
}