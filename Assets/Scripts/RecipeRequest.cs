
using System;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

[Serializable]
public class RecipeRequest {
    
    [SerializeField] private CompleteRecipeSO recipe;
    [SerializeField] private float timeToComplete;
    [SerializeField] private float remainingTimeToComplete;
    [SerializeField] private bool wasCompleted = false;
    [SerializeField] private int id;
    
    public CompleteRecipeSO Recipe => recipe;
    public float RemainingTimeToComplete => remainingTimeToComplete;
    public float TimeToComplete => timeToComplete;
    public float Value => recipe.Value;
    public bool WasCompleted => wasCompleted;
    public int ID => id;
    public bool HasExpired => !wasCompleted && remainingTimeToComplete <= 0;
    
    public event EventHandler OnRequestExpired;
    public event EventHandler OnRequestCompleted;
    
    public static event EventHandler<RecipeRequest> OnAnyRequestExpired; 
    public static event EventHandler<RecipeRequest> OnAnyRequestCompleted; 
    
    public RecipeRequest(CompleteRecipeSO recipe, float timeToComplete, int id) {
        this.recipe = recipe;
        this.timeToComplete = timeToComplete;
        this.id = id;
        remainingTimeToComplete = timeToComplete;
    }
    
    public void UpdateTime(float deltaTime) {
        if(wasCompleted || HasExpired)
            return;
        
        remainingTimeToComplete -= deltaTime;
        
        if(remainingTimeToComplete <= 0) {
            OnRequestExpired?.Invoke(this, EventArgs.Empty);
            OnAnyRequestExpired?.Invoke(this, this);
        }
    }
    
    
    public void Complete() {
        if(wasCompleted)
            throw new Exception("Request already completed");
        
        wasCompleted = true;
        OnRequestCompleted?.Invoke(this, EventArgs.Empty);
        OnAnyRequestCompleted?.Invoke(this, this);
    }
}
