using Zenject;
using UnityEngine;
using UnityEngine.UI;

public class AppInstaller : MonoInstaller
{
    [SerializeField] private WeatherView _weatherView;
    [SerializeField] private DogsView _dogsView;
    [SerializeField] private Button _weatherButton;
    [SerializeField] private Button _dogsButton;
    [SerializeField] private BreedItemView _breedItemPrefab;
    [SerializeField] private PopupView _popupView;

    public override void InstallBindings()
    {
        Container.Bind<RequestQueueService>()
            .FromNewComponentOn(this.gameObject)
            .AsSingle()
            .NonLazy();

        Container.Bind<WeatherPresenter>().AsSingle();
        Container.BindInterfacesAndSelfTo<NavigationPresenter>().AsSingle();

        Container.Bind<WeatherView>().FromInstance(_weatherView).AsSingle();

        Container.Bind<Button>().WithId("WeatherButton").FromInstance(_weatherButton);
        Container.Bind<Button>().WithId("DogsButton").FromInstance(_dogsButton);

        Container.Bind<DogsView>().FromInstance(_dogsView).AsSingle();
        Container.Bind<DogsPresenter>().AsSingle();

        Container.BindFactory<BreedItemView, BreedItemView.Factory>()
                 .FromComponentInNewPrefab(_breedItemPrefab);

        Container.Bind<PopupView>().FromInstance(_popupView).AsSingle();
    }

}
