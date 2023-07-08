using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeInput : MonoBehaviour
{
    private TMP_InputField m_InputField;

    private void Start()
    {
        m_InputField = this.GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (m_InputField.isFocused && Input.GetKeyUp(KeyCode.Return))
        {
            if (m_InputField.text == "yes")
            {
                ChannelManager.Instance.StartDay();
            }
            m_InputField.text = "";
        }
    }
}
