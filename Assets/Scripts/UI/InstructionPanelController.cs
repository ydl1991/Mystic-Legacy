using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPanelController : MonoBehaviour
{
    public delegate void DelegateShowMessage(string message);
    public static DelegateShowMessage showMessage;

    public float m_maxMessageDisplayTime;
    private float m_messageDisplayTime;

    public Text m_messageText;
    public GameObject m_messagePanel;

    // Start is called before the first frame update
    void Awake()
    {
        m_messageDisplayTime = m_maxMessageDisplayTime;
        m_messagePanel.SetActive(false);
        showMessage = ShowMessage;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_messageDisplayTime > 0f)
        {
            m_messageDisplayTime -= Time.deltaTime;
            if (m_messageDisplayTime <= 0f)
                m_messagePanel.SetActive(false);
        }    
    }

    public void ShowMessage(string message)
    {
        m_messageText.text = message;
        m_messageDisplayTime = m_maxMessageDisplayTime;
        m_messagePanel.SetActive(true);
    }
}
