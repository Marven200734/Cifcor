using System;
using Zenject;
using UnityEngine;

public class WeatherPresenter : IInitializable, IDisposable
{
    private readonly RequestQueueService _requestQueueService;
    private readonly WeatherView _weatherView;


    private const string WEATHER_API_URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
    private const string WEATHER_REQUEST_TAG = "weather_request";
    private const float REFRESH_INTERVAL_SECONDS = 5.0f;

    private float _timer;
    private bool _isActive;

    public WeatherPresenter(RequestQueueService requestQueueService, WeatherView weatherView)
    {
        _requestQueueService = requestQueueService;
        _weatherView = weatherView;
    }

    public void Initialize()
    {
        Debug.Log("WeatherPresenter Initialized");
    }

    public void Dispose()
    {
        _requestQueueService.CancelRequestByTag(WEATHER_REQUEST_TAG);
    }

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
        _weatherView.gameObject.SetActive(isActive);

        if(_isActive)
        {
            _timer = REFRESH_INTERVAL_SECONDS;
            _weatherView.ShowLoading();
        }
        else
        {
            _requestQueueService.CancelRequestByTag(WEATHER_REQUEST_TAG);
        }
    }

    public void Tick()
    {
        if(!_isActive) return;

        _timer += Time.deltaTime;
        if(_timer >= REFRESH_INTERVAL_SECONDS)
        {
            _timer = 0f;
            RequestWeather();
        }
    }

    public void RequestWeather()
    {
        Debug.Log("WeatherPresenter: Запрашиваем погоду...");
        _weatherView.ShowLoading();

        var request = new AppRequest(
            url: WEATHER_API_URL,
            tag: WEATHER_REQUEST_TAG,
            onSuccess: OnWeatherReceived,
            onFailure: OnWeatherFailed
            );
        _requestQueueService.Enqueue(request);
    }
    private void OnWeatherReceived(string json)
    {
        try
        {
            var parsedData = JsonUtility.FromJson<ContextWrapper>(json);

            var todayForecast = parsedData.properties.periods[0];

            _weatherView.SetWeatherData(todayForecast.temperature.ToString(), todayForecast.temperatureUnit, todayForecast.name);
        }
        catch (Exception e)
        {
            OnWeatherFailed($"Ошибка парсинга JSON: {e.Message}\nOriginal JSON: {json}");
        }
    }

    private void OnWeatherFailed(string error)
    {
        Debug.LogError($"Не удалось получить погоду: {error}");
    }
}
