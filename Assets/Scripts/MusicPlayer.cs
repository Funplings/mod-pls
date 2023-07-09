using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_tmpTitle;

    void Update()
    {
        m_tmpTitle.text = string.Format("Now Playing: <i>{0}</i>", AudioManager.instance.CurrentSong());
    }

    public void Prev()
    {
        AudioManager.instance.PlayPrev();
    }

    public void Skip()
    {
        AudioManager.instance.PlayNext();
    }
}
