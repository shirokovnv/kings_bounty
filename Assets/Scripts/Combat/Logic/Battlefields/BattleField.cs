using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Combat;
using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions;
using Assets.Scripts.Combat.Logic.AI.Grid;
using Assets.Scripts.Combat.UI;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;

abstract public class BattleField
{
    public TileGrid<CombatGridCell> Grid { get; protected set; }
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public DecisionContext Context { get; protected set; }
    public List<UnitGroup> PList { get; protected set; }
    public List<UnitGroup> OList { get; protected set; }

    public bool CanCastSpells;

    public BattleField(
        int width,
        int height,
        List<UnitGroup> pList,
        List<UnitGroup> oList
        )
    {
        Width = width;
        Height = height;
        PList = pList;
        OList = oList;

        Grid = new TileGrid<CombatGridCell>(
            width,
            height,
            (TileGrid<CombatGridCell> g, int x, int y) => new CombatGridCell(x, y)
            );

        FillTheGrid();
        PrepareSquadPositions();

        Context = MakeDecisionContext(0, PList, OList);
        CanCastSpells = false;
    }

    abstract protected void FillTheGrid();
    abstract protected void PrepareSquadPositions();

    public bool BelongsTo(int x, int y, UnitGroup.UnitOwner owner)
    {
        var list = owner == UnitGroup.UnitOwner.player ? PList : OList;

        var unitGroup = list.Where(group => group.X == x && group.Y == y).FirstOrDefault();

        return unitGroup != null;
    }

    public bool IsCombatFinished()
    {
        return
            PList.Count == 0 ||
            (OList.Count == 0 && PList.Where(p => p.IsOutOfControl()).Count() == 0) ||
            (OList.Count == 0 && PList.Where(p => p.IsOutOfControl()).Count() == PList.Count);
    }

    public bool IsPlayerWins()
    {
        return PList.Count > 0 && OList.Count == 0;
    }

    public bool IsOpponentWins()
    {
        return OList.Count > 0 && PList.Count == 0;
    }

    public bool HasUnitsToMove(UnitGroup.UnitOwner owner)
    {
        List<UnitGroup> units = owner == UnitGroup.UnitOwner.player ? PList : OList;

        foreach (UnitGroup unit in units)
        {
            if (unit.CurrentMoves > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void PrepareForTheNextTurn(UnitGroup.UnitOwner owner, bool isFirstTurn = false)
    {
        CanCastSpells = true;

        List<UnitGroup> units = (owner == UnitGroup.UnitOwner.player) ? PList : OList;

        units.ForEach(unit =>
        {
            unit.ResetMovement();
            unit.ResetCounterstrikes();

            if (isFirstTurn)
            {
                unit.ResetShoots();
                unit.ResetInitialQuantity();
            }
        });

        Context.PGroupIndex = 0;

        // CHECK REGENERATION
        var targetList = (owner == UnitGroup.UnitOwner.player)
            ? OList
            : PList;

        targetList
            .Where(t => t.Unit.Ability == SpecialAbility.Regenerate)
            .ToList()
            .ForEach(t =>
            {
                t.RegenerateToFullHP();
                CombatConsoleScript.Instance.PushMessage($"{t.Message()} regenerates to full HP.");
            });
    }

    public CombatActionWrapper PrepareActionsForTheNextUnit(UnitGroup.UnitOwner owner)
    {
        List<UnitGroup> units = (owner == UnitGroup.UnitOwner.player) ? PList : OList;

        var unit = units.FirstOrDefault(unit => unit.CurrentMoves != 0);

        if (unit != null)
        {
            Context.PGroupIndex = units.IndexOf(unit);

            if (owner == UnitGroup.UnitOwner.opponent)
            {
                SwapPlayersInDecisionContext();
            }

            DecisionChain chain = DecisionChainFactory.CreateByUnitType(Context);
            Queue<CombatAction> actions = chain.Traverse();

            if (owner == UnitGroup.UnitOwner.opponent)
            {
                SwapPlayersInDecisionContext();
            }

            return new CombatActionWrapper(unit, actions);
        }

        return null;
    }

    public CombatActionWrapper PrepareOutOfControlActions(UnitGroup nextUnit)
    {
        if (!nextUnit.IsOutOfControl())
        {
            throw new System.Exception("Incorrect out of control group.");
        }

        List<UnitGroup> friendlyUnits = PList.Where(pUnit => pUnit != nextUnit).ToList();
        List<UnitGroup> enemyUnits = OList;

        List<UnitGroup> current = new() { nextUnit };

        List<UnitGroup> allUnitsExceptCurrent = new();
        allUnitsExceptCurrent.AddRange(enemyUnits);
        allUnitsExceptCurrent.AddRange(friendlyUnits);

        DecisionContext outOfControlContext = new(
            0,
            current,
            allUnitsExceptCurrent,
            Grid
            );

        DecisionChain chain = DecisionChainFactory.CreateByUnitType(outOfControlContext);
        Queue<CombatAction> actions = new();
        actions.Enqueue(new OutOfControl(nextUnit));
        chain.Traverse().ToList().ForEach(action => actions.Enqueue(action));

        return new CombatActionWrapper(nextUnit, actions);
    }

    public void RemoveDeadUnits(UnitGroup.UnitOwner owner)
    {
        List<UnitGroup> units = (owner == UnitGroup.UnitOwner.player) ? PList : OList;

        units.RemoveAll(unit => unit.IsDead());
    }

    public void SwapPlayersInDecisionContext()
    {
        (OList, PList) = (PList, OList);
        Context.Swap();
    }

    protected DecisionContext MakeDecisionContext(int current, List<UnitGroup> pList, List<UnitGroup> oList)
    {
        return new DecisionContext(
            current,
            pList,
            oList,
            Grid
            );
    }
}
