using Assets.Scripts.Combat.Events;
using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Events;
using UnityEngine;

namespace Assets.Scripts.Combat.Controllers.Resolvers
{
    public class AnimationResolver : MonoBehaviour
    {
        [SerializeField] private int scale;

        protected float timer = 0;
        protected float updateRatio = 1.0f;
        protected bool isAnimated = false;

        protected CombatAction combatAction;
        protected GridTile gridTilePrefab;
        protected UnitGroup unitGroup;

        public static AnimationResolver Instance;

        private void Awake()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (isAnimated && combatAction != null)
            {
                timer += Time.deltaTime;

                if (timer > updateRatio)
                {
                    OnAnimationEnd();
                }
            }
        }

        public void OnAnimationStart(CombatAction combatAction, GridTile gridTilePrefab, UnitGroup unitGroup)
        {
            isAnimated = true;
            timer = 0;
            this.combatAction = combatAction;
            this.gridTilePrefab = gridTilePrefab;
            this.unitGroup = unitGroup;

            switch (combatAction)
            {
                case MoveToPosition:
                    gridTilePrefab.BeginAnimation(unitGroup.Unit.Sprites);
                    break;

                case FlyToPosition:
                    gridTilePrefab.BeginAnimation(unitGroup.Unit.Sprites);
                    break;

                case Preparation:
                    gridTilePrefab.BeginAnimation(unitGroup.Unit.Sprites);
                    break;

                case Attack:
                    AttackAnimationController.Instance.BeginAnimation(combatAction as Attack);
                    break;

                case Shoot:
                    AttackAnimationController.Instance.BeginAnimation(combatAction as Shoot);
                    break;

                case CounterAttack:
                    AttackAnimationController.Instance.BeginAnimation(combatAction as CounterAttack);
                    break;

                case Teleport:
                    gridTilePrefab.SetAnimatedSprites(unitGroup.Unit.Sprites);
                    break;

                case PositiveEffect:
                    gridTilePrefab.SetAnimatedSprites(unitGroup.Unit.Sprites);
                    break;

                case NegativeEffect:
                    gridTilePrefab.SetAnimatedSprites(unitGroup.Unit.Sprites);
                    break;

                case DirectSpellDamage:

                    gridTilePrefab.SetAnimatedSprites(unitGroup.Unit.Sprites);
                    AttackAnimationController.Instance.BeginAnimation(combatAction as DirectSpellDamage);
                    break;
            }
        }

        public void OnAnimationEnd()
        {
            combatAction.Execute();

            EventBus.Instance.PostEvent(new OnActionExecute { CombatAction = combatAction });

            Vector2 position;

            switch (combatAction)
            {
                case MoveToPosition:
                    position = new Vector2(unitGroup.X, unitGroup.Y);
                    position *= scale;

                    gridTilePrefab.SetSpritePosition(position);

                    break;

                case FlyToPosition:
                    position = new Vector2(unitGroup.X, unitGroup.Y);
                    position *= scale;

                    gridTilePrefab.SetSpritePosition(position);

                    break;

                case Preparation:
                    break;

                case Teleport:
                    position = new Vector2(unitGroup.X, unitGroup.Y);
                    position *= scale;

                    gridTilePrefab.SetSpritePosition(position);
                    break;
            }

            if (combatAction.IsCompleted())
            {
                gridTilePrefab.EndAnimation();

                AttackAnimationController.Instance.EndAnimation();

                combatAction = null;
                isAnimated = false;

                // wait for next animation
                EventBus.Instance.PostEvent(new OnAnimationFinish());
            }

            timer = 0;
        }
    }
}