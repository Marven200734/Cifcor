using System;
using Zenject;
using UnityEngine;

public class RequestQueueTester : MonoBehaviour
{
    private RequestQueueService _requestQueueService;

    [Inject]
    public void Construct(RequestQueueService requestQueueService)
    {
        _requestQueueService = requestQueueService;
        Debug.Log("RequestQueueTester: Сервис RequestQueueService успешно внедрен!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Нажата W. Ставим в очередь запрос на погоду.");
            var weatherRequest = new AppRequest(
                url: "https://api.weather.gov/gridpoints/TOP/32,81/forecast",
                tag: "weather",
                onSuccess: (json) => Debug.Log($"<color=green>ПОГОДА ПОЛУЧЕНА! JSON: {json.Substring(0, 200)}...</color>"),
                onFailure: (error) => Debug.Log($"<color=red>ОШИБКА ПОГОДЫ: {error}</color>")
                );
            _requestQueueService.Enqueue(weatherRequest);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Нажата D. Ставим в очередь запрос на собак.");
            var dogsRequest = new AppRequest(
                url: "https://dogapi.dog/api/v2/breeds",
                tag: "breeds_list",
                onSuccess: (json) => Debug.Log($"<color=green>СОБАКИ ПОЛУЧЕНЫ! JSON: {json.Substring(0, 200)}...</color>"),
                onFailure: (error) => Debug.Log($"<color=red>ОШИБКА СОБАК: {error}</color>")
                );
            _requestQueueService.Enqueue(dogsRequest);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Нажата C. Отменяем запросы с тегом 'weather'.");
            _requestQueueService.CancelRequestByTag("weather");
        }
    }
}
