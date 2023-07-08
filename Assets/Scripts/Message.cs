using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private string username { get; set; }
    private string message { get; set; }
    private string timestamp { get; set; }
    private Sprite profilePicture { get; set; }

    [SerializeField]
    private TextMeshProUGUI usernameText;

    [SerializeField]
    private TextMeshProUGUI messageText;

    [SerializeField]
    private TextMeshProUGUI timestampText;

    [SerializeField]
    private Image profilePictureImage;

    // Start is called before the first frame update
    void Start()
    {
        renderMessage();
    }

    public void setMessage(string username, string message, string timestamp, Sprite profilePicture)
    {
        this.username = username;
        this.message = message;
        this.timestamp = timestamp;
        this.profilePicture = profilePicture;
        renderMessage();
    }

    void renderMessage()
    {
        usernameText.text = username;
        messageText.text = message;
        timestampText.text = timestamp;
        profilePictureImage.sprite = profilePicture;
    }
}
