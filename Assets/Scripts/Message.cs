using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private string m_username { get; set; }
    private string m_message { get; set; }
    private string m_timestamp { get; set; }
    private Sprite m_profilePicture { get; set; }

    [SerializeField]
    private TextMeshProUGUI m_usernameText;

    [SerializeField]
    private TextMeshProUGUI m_messageText;

    [SerializeField]
    private TextMeshProUGUI m_timestampText;

    [SerializeField]
    private Image m_profilePictureImage;


    [SerializeField]
    private Button m_clickable;

    // Start is called before the first frame update
    void Start()
    {
        m_clickable.onClick.AddListener(onClickMessage);
        renderMessage();
    }

    public void setMessage(string username, string message, string timestamp, Sprite profilePicture)
    {
        this.m_username = username;
        this.m_message = message;
        this.m_timestamp = timestamp;
        this.m_profilePicture = profilePicture;
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
        Debug.Log("clicked");
        Utils.setImageAlpha(m_clickable.image, 255);
    }
}
