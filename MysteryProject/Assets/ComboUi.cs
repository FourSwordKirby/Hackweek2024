using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-30)]
public class ComboUi : MonoBehaviour
{
    [SerializeField]
    private GameObject m_comboLabelPrefab;
    [SerializeField]
    private GameObject m_comboLabelContainer;
    [SerializeField]
    private Color[] m_labelColors = { Color.white };
    [SerializeField]
    private int m_maxLabelCount = 7;

    private readonly Queue<GameObject> m_comboLabels = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CardManager.instance.OnComboCompleted.AddListener(OnComboCompleted);
        CardManager.instance.OnComboTimeout.AddListener(OnComboTimeout);
    }

    void OnDestroy()
    {
        CardManager.instance.OnComboCompleted.RemoveListener(OnComboCompleted);
        CardManager.instance.OnComboTimeout.RemoveListener(OnComboTimeout);
    }

    private void OnComboCompleted(IEnumerable<ComboTracker> successfulCombos)
    {
        foreach (var combo in successfulCombos)
        {
            var labelInstance = Instantiate(m_comboLabelPrefab, m_comboLabelContainer.transform);
            labelInstance.SetActive(true);
            labelInstance.transform.SetAsFirstSibling();
            var comboLabel = labelInstance.GetComponentInChildren<TextMeshProUGUI>();
            var comboBackground = labelInstance.GetComponentInChildren<RawImage>();
            m_comboLabels.Enqueue(labelInstance);

            comboLabel.text = $"{combo.Name} X{combo.ComboCount}";
            comboBackground.color = m_labelColors[UnityEngine.Random.Range(0, m_labelColors.Length)];
        }

        while (m_comboLabels.Count > m_maxLabelCount)
        {
            var comboLabel = m_comboLabels.Dequeue();
            Destroy(comboLabel);
        }
    }

    private void OnComboTimeout()
    {
        foreach (var label in m_comboLabels)
        {
            Destroy(label);
        }
        m_comboLabels.Clear();
    }
}
