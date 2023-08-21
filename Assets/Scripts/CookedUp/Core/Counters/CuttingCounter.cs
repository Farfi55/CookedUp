using System;
using System.Linq;
using CookedUp.Core.KitchenObjects;
using CookedUp.Core.KitchenObjects.Container;
using CookedUp.Core.Players;
using Shared;
using UnityEngine;

namespace CookedUp.Core.Counters
{
    [RequireComponent(typeof(KitchenObjectsContainer))]
    [RequireComponent(typeof(ProgressTracker))]
    public class CuttingCounter : BaseCounter, IRecipeProvider {

        public ProgressTracker ProgressTracker { get; private set; }
        public KitchenObjectsContainer Container { get; private set; }

        public CuttingRecipeSO CurrentCuttingRecipe { get; private set; }
        public BaseRecipeSO CurrentRecipe => CurrentCuttingRecipe;

        [SerializeField] private CuttingRecipeSO[] cuttingRecipes;


        [SerializeField] private float chopCooldown = 0.3f;
        private float remainingChopCooldown = 0;
         
        
    
        public event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;
        public event EventHandler<Player> OnChop;
        public static event EventHandler<Player> OnAnyChop;

        private void Awake() {
            ProgressTracker = GetComponent<ProgressTracker>();

            Container = GetComponent<KitchenObjectsContainer>();
            Container.OnKitchenObjectsChanged += OnKitchenObjectChanged;
        }

        private void Start() {
            ProgressTracker.OnProgressComplete += (sender, e) => OnCuttingCompleted();
        }

        private void Update() {
            remainingChopCooldown -= Time.deltaTime;
        }


        private void OnKitchenObjectChanged(object sender, KitchenObjectsChangedEvent e) {
            ProgressTracker.ResetTotalWork();
            var oldRecipe = CurrentCuttingRecipe;

            if (e.NextKitchenObject == null) {
                CurrentCuttingRecipe = null;
            }
            else {
                CurrentCuttingRecipe = GetRecipeFor(e.NextKitchenObject.KitchenObjectSO);
                if (CurrentCuttingRecipe != null)
                    ProgressTracker.SetTotalWork(CurrentCuttingRecipe.TimeToCut);
            }

            OnRecipeChanged?.Invoke(this, new ValueChangedEvent<BaseRecipeSO>(oldRecipe, CurrentCuttingRecipe));
        }


        public override void Interact(Player player) {
            if (Container.IsEmpty()) {
                if (player.HasKitchenObject() && CanCut(player.CurrentKitchenObject.KitchenObjectSO)) {
                    player.CurrentKitchenObject.SetContainer(Container);
                    InvokeOnInteract(new InteractableEvent(player));
                }
            }
            else if(Container.HasAny()) {
                if (player.Container.IsEmpty()) {
                    Container.KitchenObject.SetContainer(player.Container);
                    InvokeOnInteract(new InteractableEvent(player));
                }
                else if(player.Container.HasAny()) {
                    if (player.CurrentKitchenObject.InteractWith(Container.KitchenObject)) {
                        InvokeOnInteract(new InteractableEvent(player));
                    }
                }
            }

        }

        public override void InteractAlternateContinuous(Player player) {
            if (Container.IsEmpty())
                return;

            if (!CanCut()) {
                return;
            }

            if (remainingChopCooldown <= 0f) {
                remainingChopCooldown = chopCooldown;
                OnChop?.Invoke(this, player);
                OnAnyChop?.Invoke(this, player);
            }
            
            ProgressTracker.AddWorkDone(Time.deltaTime);
            InvokeOnInteractAlternate(new InteractableEvent(player));
        }

        private void OnCuttingCompleted() {
            if (!CanCut())
                return;

            KitchenObjectSO outputKOSO = CurrentCuttingRecipe.Output;
            
            KitchenObject.ConvertKitchenObject(Container.KitchenObject, outputKOSO);
        }

        public CuttingRecipeSO GetRecipeFor(KitchenObjectSO kitchenObjectSO) {
            return cuttingRecipes.FirstOrDefault(recipe => recipe.Input == kitchenObjectSO);
        }

        public bool CanCut() => CurrentCuttingRecipe != null;
        public bool CanCut(KitchenObjectSO kitchenObjectSO) => GetRecipeFor(kitchenObjectSO) != null;

        public bool TryGetRecipeFor(KitchenObjectSO kitchenObjectSO, out CuttingRecipeSO recipe) {
            recipe = GetRecipeFor(kitchenObjectSO);
            return recipe != null;
        }

    }
}
