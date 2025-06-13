using UnityEngine;

public class DogsView : MonoBehaviour
{
    [SerializeField] private GameObject _loader;
    [SerializeField] private GameObject _scrollView;
    [SerializeField] private RectTransform _contentParent; 

    public RectTransform ContentParent => _contentParent;

    public void SetLoading(bool isLoading)
    {
        _loader.SetActive(isLoading);
        _scrollView.SetActive(!isLoading);
    }
}
