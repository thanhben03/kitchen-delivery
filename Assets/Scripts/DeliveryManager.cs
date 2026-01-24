using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event Action OnRecipeSpawned;
    public event Action OnRecipeCompleted;
    public event Action OnRecipeSuccess;
    public event Action OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 7;
    private int successfulRecipesAmount;

    public RecipeListSO RecipeListSO => recipeListSO;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    public void RequestRecipe(RecipeSO waitingRecipeSO)
    {

        if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
        {
            waitingRecipeSOList.Add(waitingRecipeSO);
            OnRecipeSpawned?.Invoke();
        }
        
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {

        RecipeSO waitingRecipeSO = waitingRecipeSOList[0];

        if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
        {
            bool plateContentsMatchesRecipe = true;
            foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
            {
                bool ingredientFound = false;
                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                {
                    if (plateKitchenObjectSO == recipeKitchenObjectSO)
                    {
                        ingredientFound = true;
                        break;
                    }
                }
                if (!ingredientFound)
                {
                    plateContentsMatchesRecipe = false;
                }
            }

            if (plateContentsMatchesRecipe)
            {
                successfulRecipesAmount++;
                waitingRecipeSOList.RemoveAt(0);
                OnRecipeCompleted?.Invoke();
                OnRecipeSuccess?.Invoke();
                return;
            }
        }
        
        Debug.Log("No matches delivery");
        OnRecipeFailed?.Invoke();
    }

    //public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    //{
    //    for (int i = 0; i < waitingRecipeSOList.Count; i++)
    //    {
    //        RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

    //        if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
    //        {
    //            bool plateContentsMatchesRecipe = true;
    //            foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
    //            {
    //                bool ingredientFound = false;
    //                foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
    //                {
    //                    if (plateKitchenObjectSO == recipeKitchenObjectSO)
    //                    {
    //                        ingredientFound = true;
    //                        break;
    //                    }
    //                }
    //                if (!ingredientFound)
    //                {
    //                    plateContentsMatchesRecipe = false;
    //                }
    //            }

    //            if (plateContentsMatchesRecipe)
    //            {
    //                successfulRecipesAmount++;
    //                waitingRecipeSOList.RemoveAt(i);
    //                OnRecipeCompleted?.Invoke();
    //                OnRecipeSuccess?.Invoke();
    //                return;
    //            }
    //        }
    //    }
    //    Debug.Log("No matches delivery");
    //    OnRecipeFailed?.Invoke();
    //}

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }

    public RecipeSO GetRandomRecipeSO()
    {
        return recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
        
    }
}
