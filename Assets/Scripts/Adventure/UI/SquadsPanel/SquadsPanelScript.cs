using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Combat;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadsPanelScript : MonoBehaviour
{
    private const int MAX_SQUADS = 5;

    protected const float frameRatio = 0.2f;

    protected List<Sprite[]> sprites;

    protected int[] currentFrame;
    protected float timer;

    private void Awake()
    {
        GetComponent<Image>().color = Color.white;

        sprites = new List<Sprite[]>();
        currentFrame = new int[MAX_SQUADS];
        gameObject.SetActive(false);

        EventBus.Instance.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > frameRatio)
        {
            timer -= frameRatio;

            for (int i = 0; i < sprites.Count; i++)
            {
                currentFrame[i] = (currentFrame[i] + 1) % sprites[i].Length;

                transform
                    .Find("Slot" + i)
                    .Find("SquadImage" + i)
                    .GetComponent<Image>()
                    .sprite = sprites[i][currentFrame[i]];
            }
        }
    }

    public void OnEvent(OnViewSquads e)
    {
        var squads = PlayerSquads.Instance().GetSquads();
        sprites.Clear();

        for (int i = 0; i < squads.Count; i++)
        {
            currentFrame[i] = 0;
        }

        squads.ForEach(squad =>
        {
            sprites.Add(squad.Unit.Sprites);
        });

        for (int i = 0; i < MAX_SQUADS; i++)
        {
            var panelColor = (i >= squads.Count)
                ? Colors.transparent
                : GetPanelColor(squads[i].Unit.DwellingType);

            Transform slot = transform.Find("Slot" + i);
            Transform infoWrapper = slot.Find("SquadInfoWrapper");
            Transform squadImage = slot.Find("SquadImage" + i);

            slot.GetComponent<Image>().color = Colors.transparent;

            //squadImage.GetComponent<Image>().color = Colors.white;

            if (i < squads.Count)
            {
                var info = BuildSquadsInfo(squads[i]);

                squadImage.gameObject.SetActive(true);
                infoWrapper.gameObject.SetActive(true);
                infoWrapper.GetComponent<Image>().color = GetPanelColor(squads[i].Unit.DwellingType);

                infoWrapper.Find("TextSlot1").GetComponent<Text>().text = info.Item1;
                infoWrapper.Find("TextSlot2").GetComponent<Text>().text = info.Item2;
            }
            else
            {
                squadImage.gameObject.SetActive(false);
                infoWrapper.gameObject.SetActive(false);
            }

        }

        gameObject.SetActive(true);
    }

    public void OnEvent(OnHideSquads e)
    {
        gameObject.SetActive(false);
    }

    private (string, string) BuildSquadsInfo(UnitGroup squad)
    {
        string part1 = squad.Unit.Name + ": " + squad.CurrentQuantity() + "\r\n";
        part1 += "Skill level: " + squad.Unit.Level + ", moves: " + squad.CurrentMoves + "\r\n";
        part1 += "Morale: " + squad.Morale;

        int minDamage = squad.CurrentMinDamage();
        int maxDamage = squad.CurrentMaxDamage();

        string part2 = "Hit points: " + squad.CurrentHP + "\r\n";
        part2 += "Damage: " + minDamage + " - " + maxDamage + "\r\n";
        part2 += "Weekly cost: " + squad.GetTax();

        return (part1, part2);
    }

    private Color32 GetPanelColor(Dwelling.DwellingType dwellingType)
    {
        return dwellingType switch
        {
            Dwelling.DwellingType.plains => Colors.darkgray,
            Dwelling.DwellingType.forest => Colors.darkgreen,
            Dwelling.DwellingType.dungeon => Colors.darkblue,
            Dwelling.DwellingType.cavern => Colors.darkred,
            Dwelling.DwellingType.castle => Colors.purple,
            _ => Colors.black,
        };
    }
}
