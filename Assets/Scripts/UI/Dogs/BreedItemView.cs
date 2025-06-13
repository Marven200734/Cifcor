using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Zenject;

public class BreedItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Button _button;

    private string _breedId;
    private Action<string> _onClickAction;

    private void Start()
    {
        _button.onClick.AddListener(OnItemClick);
    }

    public void Setup(string breedId, string name, Action<string> onClickAction)
    {
        _breedId = breedId;
        _nameText.text = name;
        _onClickAction = onClickAction;
    }

    private void OnItemClick()
    {
        _onClickAction?.Invoke(_breedId);
    }

    public class Factory : PlaceholderFactory<BreedItemView>
    {
    }
}
