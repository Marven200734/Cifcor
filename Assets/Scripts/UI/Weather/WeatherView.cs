using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weatherText;
    [SerializeField] private Image _weatherIcon;

    public void SetWeatherData(string temperature, string unit, string forecastName)
    {
        _weatherText.text = $"{forecastName} - {temperature}{unit}";
    }

    public void ShowLoading()
    {
        _weatherText.text = "Обновление погоды...";
    }
}
