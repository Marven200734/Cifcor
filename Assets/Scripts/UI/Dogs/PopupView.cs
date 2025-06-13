using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;

public class PopupView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private GameObject _popupLoader;
    [SerializeField] private Button _closeButton;

    public void Show(string title, Action onClose)
    {
        gameObject.SetActive(true);
        _titleText.text = title;
        _descriptionText.gameObject.SetActive(false);
        _popupLoader.SetActive(true);

        _closeButton.onClick.RemoveAllListeners();
        _closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            onClose?.Invoke();
        });
    }

    public void SetDescription(string description)
    {
        _popupLoader.SetActive(false);
        _descriptionText.gameObject.SetActive(true);
        _descriptionText.text = description;
    }
}
