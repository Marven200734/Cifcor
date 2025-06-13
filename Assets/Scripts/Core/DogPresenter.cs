using System.Linq;
using Zenject;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DogsPresenter : IInitializable, IDisposable
{
    private readonly RequestQueueService _requestQueueService;
    private readonly DogsView _dogsView;
    private readonly BreedItemView.Factory _breedItemFactory;
    private readonly PopupView _popupView;

    private const string BREEDS_LIST_URL = "https://dogapi.dog/api/v2/breeds";
    private const string BREEDS_LIST_TAG = "breeds_list";
    private const string BREED_INFO_URL_FORMAT = "https://dogapi.dog/api/v2/breeds/{0}";
    private const string BREED_INFO_TAG = "breed_info";

    private bool _isActive;
    private List<BreedItemView> _spawnedItems = new List<BreedItemView>();

    public DogsPresenter(RequestQueueService requestQueueService, DogsView dogsView, PopupView popupView, BreedItemView.Factory breedItemFactory)
    {
        _requestQueueService = requestQueueService;
        _dogsView = dogsView;
        _popupView = popupView;
        _breedItemFactory = breedItemFactory;
    }

    public void Initialize() { }
    public void Dispose()
    {
        _requestQueueService.CancelRequestByTag(BREEDS_LIST_TAG);
        _requestQueueService.CancelRequestByTag(BREED_INFO_TAG);
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
        _dogsView.gameObject.SetActive(isActive);

        if (_isActive && _spawnedItems.Count == 0)
        {
            RequestBreedsList();
        }
        else if (!_isActive)
        {
            _requestQueueService.CancelRequestByTag(BREEDS_LIST_TAG);
            _requestQueueService.CancelRequestByTag(BREED_INFO_TAG);
        }
    }

    private void RequestBreedsList()
    {
        _dogsView.SetLoading(true);
        var request = new AppRequest(
            BREEDS_LIST_URL,
            BREEDS_LIST_TAG,
            OnBreedsReceived,
            OnBreedsFailed
        );
        _requestQueueService.Enqueue(request);
    }

    private void OnBreedsReceived(string json)
    {

        _dogsView.SetLoading(false);
        var breedsData = JsonUtility.FromJson<BreedsData>(json);

        foreach (var item in _spawnedItems)
        {
            GameObject.Destroy(item.gameObject);
        }
        _spawnedItems.Clear();

        var breedsToShow = breedsData.data.Take(10);

        foreach (var breed in breedsToShow)
        {
            BreedItemView newItem = _breedItemFactory.Create();
            newItem.transform.SetParent(_dogsView.ContentParent, false);
            newItem.Setup(breed.id, breed.attributes.name, OnBreedClicked);
            _spawnedItems.Add(newItem);
        }
    }

    private void OnBreedsFailed(string error)
    {
        _dogsView.SetLoading(false);
        Debug.LogError($"Не удалось получить список пород: {error}");
    }

    private void OnBreedClicked(string breedId)
    {
        _requestQueueService.CancelRequestByTag(BREED_INFO_TAG);

        _popupView.Show("Информация о породе", () =>
        {
            _requestQueueService.CancelRequestByTag(BREED_INFO_TAG);
        });

        string url = string.Format(BREED_INFO_URL_FORMAT, breedId);
        var request = new AppRequest(
            url,
            BREED_INFO_TAG,
            OnBreedInfoReceived,
            OnBreedInfoFailed
        );
        _requestQueueService.Enqueue(request);
    }
    private void OnBreedInfoReceived(string json)
    {
        var breedInfo = JsonUtility.FromJson<BreedInfoData>(json);
        if (breedInfo != null && breedInfo.data != null)
        {
            _popupView.SetDescription(breedInfo.data.attributes.description);
        }
        else
        {
            OnBreedInfoFailed("Не удалось распарсить информацию о породе.");
        }
    }

    private void OnBreedInfoFailed(string error)
    {
        Debug.LogError(error);
        _popupView.SetDescription("Не удалось загрузить описание.");
    }
}
