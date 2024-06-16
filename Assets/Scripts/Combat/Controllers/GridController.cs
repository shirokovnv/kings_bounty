using Assets.Scripts.Combat.Events;
using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Grid;
using Assets.Scripts.Combat.Logic.Systems;
using Assets.Scripts.Combat.UI;
using Assets.Scripts.Loading;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;
using Assets.Scripts.Shared.Data.State;
using Assets.Scripts.Shared.Data.State.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Resources.ScriptableObjects;

namespace Assets.Scripts.Combat.Controllers
{
    public class GridController : MonoBehaviour
    {
        public enum CombatState
        {
            None,
            Start,
            Waiting,
            Checking,
            End,
        }

        public enum UserInputState
        {
            None,
            Shoot,
            Fly,
        }

        private const int SPRITE_SORTING_ORDER = 100;

        [SerializeField] private int width, height;
        [SerializeField] private int scale;
        [SerializeField] private float scaleX, scaleY;
        [SerializeField] private GridTile tilePrefab;
        [SerializeField] private Transform cameraTransform;

        [SerializeField] private GridAnimationController controller;

        [SerializeField] private Sprite[] groundSprites;
        [SerializeField] private Sprite[] cursorSprites;
        [SerializeField] private Sprite[] castleSprites;

        [SerializeField] private Sprite attackSprite;

        private GridTile tile;
        private BattleField battleField;

        private Dictionary<UnitGroup, GridTile> pUnitImages;
        private Dictionary<UnitGroup, GridTile> oUnitImages;

        private CombatState state;
        private UserInputState inputState;

        private UnitGroup.UnitOwner owner;

        private InCombat combatState;

        private void Awake()
        {
            combatState = GameStateManager.Instance().GetState() as InCombat;

            pUnitImages = new Dictionary<UnitGroup, GridTile>();
            oUnitImages = new Dictionary<UnitGroup, GridTile>();

            state = CombatState.None;
            inputState = UserInputState.None;

            owner = UnitGroup.UnitOwner.player;

            EventBus.Instance.Register(this);
        }

        public void Start()
        {
            GenerateGrid();
            LetCameraViewCenter();

            controller.OnAnimationFinished += OnAnimationFinished;
        }

        public void Update()
        {
            switch (state)
            {
                case CombatState.None:
                    state = CombatState.Start;

                    break;

                case CombatState.Start:

                    if (owner == UnitGroup.UnitOwner.player)
                    {
                        WaitForUserInput();
                    }
                    else
                    {
                        CombatActionWrapper actionWrapper = battleField.PrepareActionsForTheNextUnit(owner);

                        if (actionWrapper != null)
                        {
                            controller.PlayAnimationSequence(actionWrapper);
                            controller.SetPrefab(oUnitImages[actionWrapper.Unit]);

                            state = CombatState.Waiting;
                        }
                        else
                        {
                            state = CombatState.Checking;
                        }

                    }

                    break;

                case CombatState.Waiting:
                    break;

                case CombatState.Checking:
                    RemoveDeadUnits();

                    // Recalculate morale
                    MoraleSystem.Instance().CalculateMorale(battleField.PList);
                    MoraleSystem.Instance().CalculateMorale(battleField.OList);

                    if (battleField.IsCombatFinished())
                    {
                        Debug.Log("Combat ends.");
                        state = CombatState.End;
                    }

                    if (state == CombatState.Checking)
                    {
                        if (battleField.HasUnitsToMove(owner) == false)
                        {
                            Debug.Log($"Finishing move {owner}");

                            battleField.PrepareForTheNextTurn(owner);

                            owner = owner == UnitGroup.UnitOwner.player
                                ? UnitGroup.UnitOwner.opponent
                                : UnitGroup.UnitOwner.player;
                        }

                        Debug.Log($"Change state from {state} to {CombatState.Start}");
                        state = CombatState.Start;
                    }

                    break;

                case CombatState.End:
                    Debug.Log("The battle is over.");
                    OnFinishBattle();

                    break;
            }

            UpdateTextMeshes();
        }

        public void OnEvent(OnHideSpellbook e)
        {
            state = CombatState.Checking;
        }

        public void OnEvent(OnChooseSpell e)
        {
            CastSpell(e.SpellTarget);
            battleField.CanCastSpells = false;
        }

        private void CastSpell(CombatSpellTarget target)
        {
            Queue<CombatAction> actions = new();

            switch (target.Source.Name)
            {
                case "Fireball":
                    actions.Enqueue(new DirectSpellDamage(target));
                    break;

                case "Lightning bolt":
                    actions.Enqueue(new DirectSpellDamage(target));
                    break;

                case "Exorcism":
                    actions.Enqueue(new DirectSpellDamage(target));
                    break;

                case "Clone":
                    actions.Enqueue(new PositiveEffect(target));
                    break;

                case "Resurrect":
                    actions.Enqueue(new PositiveEffect(target));
                    break;

                case "Teleport":
                    actions.Enqueue(new Teleport(target));
                    break;

                case "Petrify":
                    actions.Enqueue(new NegativeEffect(target));
                    break;
            }

            CombatActionWrapper actionWrapper = new(target.Target, actions);

            var images = target.Target.Owner == UnitGroup.UnitOwner.player
                ? pUnitImages
                : oUnitImages;

            Debug.Log("fab: " + images[actionWrapper.Unit]);

            controller.PlayAnimationSequence(actionWrapper);
            controller.SetPrefab(images[actionWrapper.Unit]);
        }

        private void UpdateTextMeshes()
        {
            foreach (var item in pUnitImages)
            {
                item.Value.SetText(item.Key.CurrentQuantity().ToString());
            }

            foreach (var item in oUnitImages)
            {
                item.Value.SetText(item.Key.CurrentQuantity().ToString());
            }
        }

        public void OnAnimationFinished(object sender, GridAnimationController.OnAnimationFinishedEventArgs e)
        {
            Debug.Log($"Animation finished for {e.group.Unit.Name} ({e.group.X}, {e.group.Y})");

            state = CombatState.Checking;
        }

        private void RemoveDeadUnits()
        {
            var pDeadUnits = pUnitImages.Where(kv => kv.Key.IsDead());
            foreach (var item in pDeadUnits.ToList())
            {
                pUnitImages.Remove(item.Key);
                Destroy(item.Value.gameObject);
            }

            var oDeadUnits = oUnitImages.Where(kv => kv.Key.IsDead());
            foreach (var item in oDeadUnits.ToList())
            {
                oUnitImages.Remove(item.Key);
                Destroy(item.Value.gameObject);
            }

            battleField.RemoveDeadUnits(UnitGroup.UnitOwner.player);
            battleField.RemoveDeadUnits(UnitGroup.UnitOwner.opponent);
        }

        private GridTile InstantiateSquadImage(UnitScriptableObject unit, Vector2 position, bool flip = false)
        {
            GridTile tile;
            position *= scale;

            tile = Instantiate(tilePrefab, new Vector3(position.x, position.y), Quaternion.identity);
            tile.Init(unit.Sprites[0], false, flip, SPRITE_SORTING_ORDER);
            tile.SetSpritePosition(position);
            tile.SetSpriteScaleRatio(scale, scale);
            tile.name = $"{unit.Name}#{flip}";

            return tile;
        }

        private void GenerateGrid()
        {
            battleField = CombatSystem.Instance().PrepareBattleField(combatState.GetCombatable(), width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var spawnedTile = Instantiate(tilePrefab, new Vector3(x * scale, y * scale), Quaternion.identity);
                    spawnedTile.name = $"tile {x} {y}";

                    var sprite = battleField.Grid.GetValue(x, y).IsObstacle()
                        ? GetBattleFieldSprite(battleField, x, y)
                        : groundSprites[0];

                    spawnedTile.Init(sprite, true);
                    spawnedTile.SetLocalScale(new Vector3(scale * scaleX, scale * scaleY));
                }
            }

            battleField.PList.ForEach(pUnit =>
            {
                tile = InstantiateSquadImage(pUnit.Unit, new Vector2(pUnit.X, pUnit.Y));
                tile.SetText(pUnit.CurrentQuantity().ToString());
                tile.SetSortingOrder(10);

                pUnitImages.Add(pUnit, tile);
            });

            battleField.OList.ForEach(oUnit =>
            {
                tile = InstantiateSquadImage(oUnit.Unit, new Vector2(oUnit.X, oUnit.Y), true);
                tile.SetText(oUnit.CurrentQuantity().ToString());
                oUnitImages.Add(oUnit, tile);
            });

            battleField.PrepareForTheNextTurn(UnitGroup.UnitOwner.player);
            battleField.PrepareForTheNextTurn(UnitGroup.UnitOwner.opponent);

            MoraleSystem.Instance().CalculateMorale(battleField.PList);
            MoraleSystem.Instance().CalculateMorale(battleField.OList);
        }

        private void LetCameraViewCenter()
        {
            cameraTransform.position = new Vector3(
                (float)(width / 2.0f - 0.5f) * scale,
                (float)(height / 2.0f - 0.5f) * scale,
                -10
            );
        }

        private Sprite GetBattleFieldSprite(BattleField battleField, int x, int y)
        {
            return battleField switch
            {
                AdventureBattleField => GetRandomObstacleSprite(),
                CastleBattleField => GetCastlePartSprite(x, y),
                _ => null,
            };
        }

        private Sprite GetRandomObstacleSprite()
        {
            return groundSprites[Random.Range(1, groundSprites.Length)];
        }

        private Sprite GetCastlePartSprite(int x, int y)
        {
            Vector2Int leftBottomCorner = new(0, 0);
            Vector2Int rightBottomCorner = new(width - 1, 0);

            Vector2Int leftGate = new(1, 0);
            Vector2Int rightGate = new(width - 2, 0);

            Vector2Int position = new(x, y);

            if (position.Equals(leftBottomCorner))
            {
                return castleSprites[0];
            }

            if (position.Equals(rightBottomCorner))
            {
                return castleSprites[1];
            }

            if (position.Equals(leftGate))
            {
                return castleSprites[4];
            }

            if (position.Equals(rightGate))
            {
                return castleSprites[5];
            }

            if (position.x == 0)
            {
                return castleSprites[2];
            }

            if (position.x == width - 1)
            {
                return castleSprites[3];
            }

            return null;
        }

        private bool CheckOutOfControl(UnitGroup nextUnit)
        {
            if (nextUnit == null || !nextUnit.IsOutOfControl())
            {
                return false;
            }

            CombatActionWrapper actionWrapper = battleField.PrepareOutOfControlActions(nextUnit);

            if (actionWrapper != null)
            {
                pUnitImages[nextUnit].SetAnimatedSprites(nextUnit.Unit.Sprites);

                controller.PlayAnimationSequence(actionWrapper);
                controller.SetPrefab(pUnitImages[actionWrapper.Unit]);

                state = CombatState.Waiting;

                return true;
            }

            return false;
        }

        private void WaitForUserInput()
        {
            var nextUnit = battleField.Context.NextMoveableUnit();
            var images = pUnitImages;

            if (CheckOutOfControl(nextUnit))
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) || !battleField.HasUnitsToMove(owner))
            {
                if (nextUnit != null)
                {
                    images[nextUnit].EndAnimation();
                }

                battleField
                    .PList
                    .Where(p => !p.IsOutOfControl())
                    .ToList()
                    .ForEach(p => p.CurrentMoves = 0);

                state = CombatState.Checking;

                return;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                if (!battleField.CanCastSpells)
                {
                    CombatConsoleScript.Instance.PushMessage("Can cast only one spell per turn.");

                    return;
                }

                state = CombatState.Waiting;

                if (nextUnit != null)
                {
                    images[nextUnit].EndAnimation();
                }

                EventBus.Instance.PostEvent(new OnViewSpellbook
                {
                    BattleField = battleField,
                    SourceX = nextUnit.X,
                    SourceY = nextUnit.Y,
                });

                return;
            }

            if (nextUnit != null)
            {
                controller.SetUnitGroup(nextUnit);
                controller.SetPrefab(images[nextUnit]);

                if (!images[nextUnit].IsAnimated() && inputState == UserInputState.None)
                {
                    images[nextUnit].BeginAnimation(nextUnit.Unit.Sprites);
                }

                CheckInput(nextUnit);
            }
        }

        private void CheckInput(UnitGroup nextUnit)
        {
            switch (inputState)
            {
                case UserInputState.None:
                    CheckMoveInput(nextUnit);
                    break;

                case UserInputState.Shoot:
                    CheckShootInput(nextUnit);
                    break;

                case UserInputState.Fly:
                    CheckFlyInput(nextUnit);
                    break;
            }
        }

        private void CheckShootInput(UnitGroup nextUnit)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inputState = UserInputState.None;
                CursorController.Instance.Deactivate();
                pUnitImages[nextUnit].BeginAnimation(nextUnit.Unit.Sprites);

                return;
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                int targetX = CursorController.Instance.GetCursorX();
                int targetY = CursorController.Instance.GetCursorY();

                // Check OUT of CONTROL
                if (battleField.BelongsTo(targetX, targetY, UnitGroup.UnitOwner.player))
                {
                    var targetOutOfControl = battleField
                        .PList
                        .Where(group => group.X == targetX && group.Y == targetY && group.IsOutOfControl())
                        .FirstOrDefault();

                    if (targetOutOfControl != null)
                    {
                        inputState = UserInputState.None;
                        CursorController.Instance.Deactivate();

                        Shoot(nextUnit, targetOutOfControl);
                    }
                }

                if (battleField.BelongsTo(targetX, targetY, UnitGroup.UnitOwner.opponent))
                {
                    var target = battleField
                        .OList
                        .Where(group => group.X == targetX && group.Y == targetY).FirstOrDefault();

                    if (target != null)
                    {
                        inputState = UserInputState.None;
                        CursorController.Instance.Deactivate();

                        Shoot(nextUnit, target);
                    }
                }
            }
        }

        private void CheckFlyInput(UnitGroup nextUnit)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inputState = UserInputState.None;
                CursorController.Instance.Deactivate();
                pUnitImages[nextUnit].BeginAnimation(nextUnit.Unit.Sprites);

                return;
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                int targetX = CursorController.Instance.GetCursorX();
                int targetY = CursorController.Instance.GetCursorY();

                if (!battleField.Grid.GetValue(targetX, targetY).IsObstacle() &&
                    !battleField.BelongsTo(targetX, targetY, UnitGroup.UnitOwner.player) &&
                    !battleField.BelongsTo(targetX, targetY, UnitGroup.UnitOwner.opponent)
                    )
                {
                    inputState = UserInputState.None;
                    CursorController.Instance.Deactivate();

                    Fly(nextUnit, targetX, targetY);
                }
            }
        }

        private void CheckMoveInput(UnitGroup nextUnit)
        {
            if (
                Input.GetKeyDown(KeyCode.S) &&
                nextUnit.CurrentShoots > 0 &&
                !AIPathFinder.IsCellBlocked(nextUnit.X, nextUnit.Y, battleField.OList)
                )
            {
                inputState = UserInputState.Shoot;
                pUnitImages[nextUnit].EndAnimation();
                CursorController.Instance.ActivateWithPosition(nextUnit.X, nextUnit.Y, width, height);

                return;
            }

            if (Input.GetKeyDown(KeyCode.F) &&
                nextUnit.IsFlyer()
                )
            {
                inputState = UserInputState.Fly;
                pUnitImages[nextUnit].EndAnimation();
                CursorController.Instance.ActivateWithPosition(nextUnit.X, nextUnit.Y, width, height);

                return;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Wait(nextUnit);

                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Pass(nextUnit);

                return;
            }

            int pX = nextUnit.X;
            int pY = nextUnit.Y;

            int dX = -1;
            int dY = -1;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                dX = pX - 1;
                dY = pY;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                dX = pX + 1;
                dY = pY;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                dX = pX;
                dY = pY + 1;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                dX = pX;
                dY = pY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                dX = pX - 1;
                dY = pY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                dX = pX + 1;
                dY = pY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                dX = pX - 1;
                dY = pY + 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                dX = pX + 1;
                dY = pY + 1;
            }

            // check edges
            if (dX < 0 || dY < 0 || dX >= width || dY >= height)
            {
                return;
            }

            var combatCell = battleField.Grid.GetValue(dX, dY);

            if (battleField.BelongsTo(dX, dY, UnitGroup.UnitOwner.player))
            {
                // Check OUT of CONTROL
                var targetOutOfControl = battleField
                    .PList
                    .Where(group => group.X == dX && group.Y == dY && group.IsOutOfControl())
                    .FirstOrDefault();

                if (targetOutOfControl != null)
                {
                    Attack(nextUnit, targetOutOfControl);
                }

                return;
            }

            if (battleField.BelongsTo(dX, dY, UnitGroup.UnitOwner.opponent))
            {
                // attack
                var defender = battleField.OList.Where(group => group.X == dX && group.Y == dY).FirstOrDefault();

                if (defender != null)
                {
                    Attack(nextUnit, defender);
                }

                return;
            }

            // move
            if (!combatCell.IsObstacle())
            {
                string message = $"Move {nextUnit.Message()} to {dX}, {dY}";
                CombatConsoleScript.Instance.PushMessage(message);

                nextUnit.X = dX;
                nextUnit.Y = dY;
                nextUnit.CurrentMoves--;

                Vector2Int position = new(dX, dY);
                position *= scale;

                pUnitImages[nextUnit].SetSpritePosition(position);

                if (nextUnit.CurrentMoves == 0)
                {
                    pUnitImages[nextUnit].EndAnimation();
                }
            }
        }

        private void Attack(UnitGroup attacker, UnitGroup defender)
        {
            pUnitImages[attacker].EndAnimation();

            Queue<CombatAction> actions = new Queue<CombatAction>();
            actions.Enqueue(new Attack(attacker, defender));

            if (!defender.IsDead() && defender.CurrentCounterstrikes > 0)
            {
                actions.Enqueue(new CounterAttack(defender, attacker));
            }

            CombatActionWrapper actionWrapper = new CombatActionWrapper(attacker, actions);

            controller.PlayAnimationSequence(actionWrapper);

            state = CombatState.Waiting;
        }

        private void Shoot(UnitGroup attacker, UnitGroup defender)
        {
            pUnitImages[attacker].EndAnimation();

            Queue<CombatAction> actions = new Queue<CombatAction>();
            actions.Enqueue(new Shoot(attacker, defender));

            CombatActionWrapper actionWrapper = new CombatActionWrapper(attacker, actions);

            controller.PlayAnimationSequence(actionWrapper);

            state = CombatState.Waiting;
        }

        private void Pass(UnitGroup nextUnit)
        {
            pUnitImages[nextUnit].EndAnimation();

            Queue<CombatAction> actions = new Queue<CombatAction>();
            actions.Enqueue(new Pass(nextUnit));

            CombatActionWrapper actionWrapper = new CombatActionWrapper(nextUnit, actions);

            controller.PlayAnimationSequence(actionWrapper);

            state = CombatState.Waiting;

            battleField.Context.NextIndex();
        }

        private void Wait(UnitGroup nextUnit)
        {
            pUnitImages[nextUnit].EndAnimation();

            Queue<CombatAction> actions = new Queue<CombatAction>();
            actions.Enqueue(new Wait(nextUnit));

            CombatActionWrapper actionWrapper = new CombatActionWrapper(nextUnit, actions);

            controller.PlayAnimationSequence(actionWrapper);

            state = CombatState.Waiting;

            battleField.Context.NextIndex();
        }

        private void Fly(UnitGroup nextUnit, int targetX, int targetY)
        {
            pUnitImages[nextUnit].EndAnimation();

            CombatConsoleScript.Instance.PushMessage(
            $"Fly {nextUnit.Message()} to {targetX}, {targetY}"
            );

            nextUnit.X = targetX;
            nextUnit.Y = targetY;

            pUnitImages[nextUnit].SetSpritePosition(new Vector3(targetX * scale, targetY * scale));
        }

        private void OnFinishBattle()
        {
            InCombat combatState = GameStateManager.Instance().GetState() as InCombat;

            CheckFinishedWithOutOfControlGroups();

            if (battleField.IsPlayerWins())
            {
                Debug.Log("Player wins the match");

                GameStateManager.Instance().SetState(new WinCombat(
                    combatState.GetCombatable(),
                    combatState.GetTotalQuantity()
                    ));
            }

            if (battleField.IsOpponentWins())
            {
                Debug.Log("Opponent wins the match");

                GameStateManager.Instance().SetState(new LoseCombat(
                    combatState.GetCombatable(),
                    combatState.GetTotalQuantity()
                    ));
            }

            SceneLoader.Load(SceneLoader.Scene.AdventureScene);
        }

        private void CheckFinishedWithOutOfControlGroups()
        {
            var outOfControlGroups = battleField
                .PList
                .Where(pUnit => pUnit.IsOutOfControl())
                .ToList();

            outOfControlGroups.ForEach(group =>
            {
                if (battleField.OList.Count < 5)
                {
                    group.Owner = UnitGroup.UnitOwner.opponent;

                    battleField.OList.Add(group);
                }

                battleField.PList.Remove(group);
            });
        }
    }
}