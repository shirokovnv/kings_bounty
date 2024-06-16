using Assets.Scripts.Combat.Controllers.Resolvers;
using Assets.Scripts.Combat.Events;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat.Controllers
{
    public class GridAnimationController : MonoBehaviour
    {
        private CombatAction action;
        private Queue<CombatAction> actionSequence;
        private GridTile gridTilePrefab;
        private UnitGroup unitGroup;

        public event EventHandler<OnAnimationFinishedEventArgs> OnAnimationFinished;
        public class OnAnimationFinishedEventArgs : EventArgs
        {
            public UnitGroup group;
            public GridTile prefab;
        }

        private bool waitForNextAction;

        private void Awake()
        {
            EventBus.Instance.Register(this);
            waitForNextAction = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (waitForNextAction && actionSequence != null && actionSequence.Count > 0)
            {
                action = actionSequence.Dequeue();
                AnimationResolver.Instance.OnAnimationStart(action, gridTilePrefab, unitGroup);
                waitForNextAction = false;
            }
        }

        public void OnEvent(OnAnimationFinish e)
        {
            if (actionSequence != null)
            {
                if (actionSequence.Count > 0)
                {
                    waitForNextAction = true;
                }
                else
                {
                    waitForNextAction = false;
                    actionSequence = null;

                    OnAnimationFinished?.Invoke(
                        this,
                        new OnAnimationFinishedEventArgs
                        {
                            group = unitGroup,
                            prefab = gridTilePrefab
                        });
                }
            }
        }

        public void SetPrefab(GridTile prefab)
        {
            gridTilePrefab = prefab;
        }

        public void SetUnitGroup(UnitGroup unitGroup)
        {
            this.unitGroup = unitGroup;
        }

        public void PlayAnimationSequence(CombatActionWrapper action)
        {
            actionSequence = action.Actions;
            unitGroup = action.Unit;
            waitForNextAction = true;
        }
    }
}