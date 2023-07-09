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

    private bool m_violatesRules;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(onClickMessage);
        renderMessage();
    }

    public void setMessage(string username, string message, string timestamp, Sprite profilePicture, string channelName)
    {
        this.m_username = username;
        this.m_message = message;
        this.m_timestamp = timestamp;
        this.m_profilePicture = profilePicture;
        this.m_channelName = channelName;
        validateMessage();
        renderMessage();
    }

    void renderMessage()
    {
        m_usernameText.text = m_username;
        m_messageText.text = m_message;
        m_timestampText.text = m_timestamp;
        m_profilePictureImage.sprite = m_profilePicture;
    }

    void onClickMessage()
    {
        if (m_violatesRules)
        {
            Debug.Log("This message violated the rules");
        }
        else
        {
            Debug.Log("This message is fine");
        }

        button.interactable = false;
        m_timestampText.text = string.Format("<Deleted by {0}>", GameManager.instance.m_strPlayer);
    }

    void validateMessage()
    {
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
