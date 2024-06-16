using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TopUIScript : MonoBehaviour
{
    public static TopUIScript Instance;

    [SerializeField] private GameObject textObject;

    private void Awake()
    {
        Instance = this;

        EventBus.Instance.Register(this);
    }

    private void Start()
    {
        UpdateTextMessage(BuildTopTextMessage());
    }

    public void OnEvent(OnTick e)
    {
        UpdateTextMessage(BuildTopTextMessage());
    }

    public void OnEvent(OnCastSpell e)
    {
        UpdateTextMessage(BuildTopTextMessage());
    }

    public void UpdateTextMessage(string text)
    {
        textObject.GetComponent<Text>().text = text;
    }

    private string BuildTopTextMessage()
    {
        var bonuses = TimeSystem.Instance().GetActiveBonuses();

        string text = System.String.Join(", ", bonuses.Select(b => b.Message()).ToArray());
           
        if (text.Length > 0)
        {
            return text;
        }

        string continentName = ContinentSystem
            .Instance()
            .GetContinentAtNumber(Player.Instance().ContinentNumber)
            .GetName();

        int daysLeft = TimeSystem.Instance().DaysLeft();

        return $"{continentName}, \t Days Left: {daysLeft}, \t (O) - Options";
    }
}
