using Zenject;
using UnityEngine.UI;
using UnityEngine;

public class NavigationPresenter : IInitializable, ITickable
{
    private readonly WeatherPresenter _weatherPresenter;
    private readonly DogsPresenter _dogsPresenter;

    private readonly Button _weatherButton;
    private readonly Button _dogsButton;

    public NavigationPresenter(
        WeatherPresenter weatherPresenter,
        DogsPresenter dogsPresenter,
        [Inject(Id = "WeatherButton")] Button weatherButton,
        [Inject(Id = "DogsButton")] Button dogsButton)
    {
         _weatherPresenter = weatherPresenter;
        _dogsPresenter = dogsPresenter;
        _weatherButton = weatherButton;
        _dogsButton = dogsButton;
    }

    public void Initialize()
    {
        _weatherButton.onClick.AddListener(ShowWeatherTab);
        _dogsButton.onClick.AddListener(ShowdogsTab);

        ShowWeatherTab();
    }

    public void Tick()
    {
        _weatherPresenter.Tick();
    }

    private void ShowWeatherTab()
    {
        _weatherPresenter.SetActive(true);
        _dogsPresenter.SetActive(false);
    }
    private void ShowdogsTab()
    {
        _weatherPresenter.SetActive(false);
        _dogsPresenter.SetActive(true);
    }
}
