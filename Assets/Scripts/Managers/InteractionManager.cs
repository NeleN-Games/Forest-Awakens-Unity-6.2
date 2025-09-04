
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI toolTipText;
    [SerializeField] private Transform cameraTransform;
    
    public static InteractionManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        HideTooltip();
    }
    public void ShowTooltip(string text, Vector3 position)
    {
        toolTipText.gameObject.SetActive(true);
        toolTipText.transform.position = position;
        toolTipText.transform.rotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
        toolTipText.text = text;
    }

    public void HideTooltip()
    {
        if (toolTipText != null)
            toolTipText.gameObject.SetActive(false);
    }
}
