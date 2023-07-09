using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TypeInput : MonoBehaviour
{
    private TMP_InputField m_InputField;
    public UnityEvent<string> SubmitEvent;

    private void Start()
    {
        m_InputField = this.GetComponent<TMP_InputField>();
        m_InputField.onSubmit.AddListener(Submit);

    }

    private void Submit(string strInput)
    {
        if (!string.IsNullOrWhiteSpace(strInput) && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitEvent.Invoke(strInput);
            m_InputField.text = "";
        }
    }
}
