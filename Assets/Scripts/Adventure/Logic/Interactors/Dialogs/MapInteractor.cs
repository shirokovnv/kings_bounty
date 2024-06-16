using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using Assets.Scripts.Shared.Utility;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Scripts.Adventure.Logic.Continents;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class MapInteractor : BaseDialogInteractor<Adventuring, ViewMap>
    {
        private const int SIZE = 64;
        private const int PIXELS_PER_UNIT = 16;
        private const float cycleHz = 8.0f;
        private static readonly Texture2D texture = new(SIZE, SIZE, TextureFormat.ARGB32, false);
        private static float dtime = 0;
        private static bool fullMode;

        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                fullMode = false;

                var continent = ContinentSystem
                    .Instance()
                    .GetContinentAtNumber(Player.Instance().ContinentNumber);

                DrawMap(continent);

                Rect rect = new()
                {
                    x = 0,
                    y = 0,
                    width = continent.GetGrid().GetWidth(),
                    height = continent.GetGrid().GetHeight()
                };

                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0, 0), PIXELS_PER_UNIT);

                string text = "X = " + Player.Instance().X + ", Y = " + Player.Instance().Y;

                if (continent.HasFullMap())
                {
                    text += ", (space) - toggle full map";
                }

                DialogUI.Instance.ShowDialogUI(continent.GetName(), text, sprite);

                GameStateManager.Instance().SetState(new ViewMap());
            }
        }

        public override void WaitForFinish()
        {
            texture.SetPixel(Player.Instance().X, Player.Instance().Y, FlickeringPixel());
            texture.Apply();

            var continent = ContinentSystem
                    .Instance()
                    .GetContinentAtNumber(Player.Instance().ContinentNumber);

            if (Input.GetKeyDown(KeyCode.Space) && continent.HasFullMap())
            {
                DrawMap(continent, fullMode = !fullMode);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DialogUI.Instance.HideDialog();

                GameStateManager.Instance().SetState(new Adventuring());
            }
        }

        private void DrawMap(Continent continent, bool fullMode = false)
        {
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            for (int x = 0; x < continent.GetGrid().GetWidth(); x++)
            {
                for (int y = 0; y < continent.GetGrid().GetHeight(); y++)
                {
                    Color32 color = GetTileColor(
                        continent.GetGrid().GetValue(x, y),
                        x,
                        y,
                        continent.GetConfig().GetNumber(),
                        fullMode
                    );
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
        }

        private static Color32 GetTileColor(
            ContinentTile tile,
            int x,
            int y,
            int continentNumber,
            bool fullMode = false
        )
        {
            if (!tile.IsRevealed() && !fullMode)
            {
                return Colors.black;
            }

            // check boat
            if (
                x == Boat.Instance().X &&
                y == Boat.Instance().Y &&
                continentNumber == Boat.Instance().ContinentNumber
            )
            {
                return Colors.violet;
            }

            if (tile.ObjectLayer != null)
            {
                return tile.ObjectLayer.GetObjectType() switch
                {
                    ObjectType.city => Colors.orchid,
                    ObjectType.castleWall => Colors.white,
                    ObjectType.castleGate => Colors.black,
                    ObjectType.chest => Colors.magentauchsia,
                    ObjectType.sign => Colors.lightgray,
                    ObjectType.dwelling => Colors.purple,
                    ObjectType.artifact => Colors.red,
                    ObjectType.captain => Colors.red,
                    _ => Colors.black,
                };
            }

            return tile.BiomeLayer.GetBiomeType() switch
            {
                BiomeType.water => Colors.royalblue,
                BiomeType.grass => Colors.lightgreen,
                BiomeType.forest => Colors.darkgreen,
                BiomeType.mountain => Colors.sandybrown,
                BiomeType.desert => Colors.yellow,
                _ => Colors.black
            };
        }

        private static Color32 FlickeringPixel()
        {
            dtime += Time.deltaTime;

            float wave = Mathf.Sin(dtime * 2.0f * Mathf.PI * cycleHz);

            if (wave > 0.0f)
            {
                return Colors.white;
            }

            if (wave == 0.0f)
            {
                dtime = 0.0f;
            }

            return Colors.black;
        }
    }
}