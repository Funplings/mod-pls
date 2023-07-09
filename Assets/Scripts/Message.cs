using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private string m_username;
    private string m_message;
    private string m_timestamp;
    private Sprite m_profilePicture;

    private string m_channelName;

    [SerializeField]
    private TextMeshProUGUI m_usernameText;

    [SerializeField]
    private TextMeshProUGUI m_messageText;

    [SerializeField]
    private TextMeshProUGUI m_timestampText;

    [SerializeField]
    private Image m_profilePictureImage;

    public bool m_violatesRules;

    private bool m_isModMessage;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(onClickMessage);
    }

    public void setMessage(string username, string message, string timestamp, Sprite profilePicture, string channelName)
    {
        this.m_username = username;
        this.m_message = message;
        this.m_timestamp = timestamp;
        this.m_profilePicture = profilePicture;
        this.m_channelName = channelName;

        UserData userData = ChannelManager.Instance.m_allUsers.User(m_username);
        if (userData != null && userData.m_isMod)
        {
            m_isModMessage = true;
        }

        validateMessage();
        renderMessage();
        AudioManager.instance.PlaySFX("New_Message");

        // Auto-delete

        if (username == GameManager.instance.m_strPlayer && m_violatesRules)
        {
            ChannelManager.Instance.Delete(false);
            button.interactable = false;
            m_timestampText.text = "<Deleted by Jr_lover<3>";
        }
    }

    void renderMessage()
    {
        m_usernameText.text = m_username;
        m_messageText.text = m_message;
        m_timestampText.text = m_timestamp;
        m_profilePictureImage.sprite = m_profilePicture;

        // Check if user is a mod; if so, set usernameText color to red
        if (m_isModMessage)
        {
            m_usernameText.color = Color.red;
        }
    }

    void onClickMessage()
    {
        if (m_violatesRules)
        {
            ChannelManager.Instance.Delete(true);
        }
        else
        {
            ChannelManager.Instance.Delete(false);
        }

        button.interactable = false;

        string label; 
        if (ChannelManager.Instance.m_day == 4)
        {
            label = "Banned by";
            AudioManager.instance.PlaySFX("Ban_Message");
        } else
        {
            label = "Deleted by";
            AudioManager.instance.PlaySFX("Delete_Message");
        }
        m_timestampText.text = string.Format("<{0} {1}>", label, GameManager.instance.m_strPlayer);
    }

    void validateMessage()
    {
        // Check if user is a mod AND it is not the final day; if so, it doesn't violate the rules
        if (m_isModMessage && ChannelManager.Instance.m_day != 4)
        {
            m_violatesRules = false;
            return;
        }
        m_violatesRules = RulesManager.Instance.ValidateMessage(this);
    }

    public string GetChannelName()
    {
        return m_channelName;
    }

    public string GetMessage()
    {
        return m_message;
    }
}
