using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatscriptData", menuName = "mod-pls/ChatscriptData", order = 0)]
public class ChatscriptData : ScriptableObject
{
    [SerializeField] 
    [SerializeReference]
    public List<ChatscriptCommand> m_commands = new List<ChatscriptCommand>();
}

[System.Serializable]
public abstract class ChatscriptCommand
{
    public enum TYPE
    {
        MESSAGE,
        PAUSE
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
}

[System.Serializable]
public class PauseCommand : ChatscriptCommand
{
    public PauseCommand(float secPause)
    {
        m_type = TYPE.PAUSE;

        m_secPause = secPause;
    }

    public float m_secPause;
}