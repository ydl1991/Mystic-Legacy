
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradePanelController : MonoBehaviour
{
    public Text m_unitDescriptionText;
    public Text m_unitUpgradeText;
    public Button m_upgradeButton;

    public void SetButtonCallback(UnityEngine.Events.UnityAction callback)
    {
        m_upgradeButton.onClick.AddListener(callback);
    }

    public void ClearButtonCallbacks()
    {
        m_upgradeButton.onClick.RemoveAllListeners();
    }

    public void SetDescriptionText(string description)
    {
        m_unitDescriptionText.text = description;
    }

    public void SetUpgradeText(string upgradeText)
    {
        m_unitUpgradeText.text = upgradeText;
    }
}
