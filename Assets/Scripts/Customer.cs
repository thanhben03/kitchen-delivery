using UnityEngine;

public class Customer : MonoBehaviour
{
    public int queueIndex = -1;
    public float moveSpeed = 2f;
    [SerializeField] private Vector3 targetPosition;

    private RecipeSO currentRecipeSO;

    void Start()
    {
        //targetPosition = transform.position; // Ban đầu đứng yên
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void ServeFoodAndLeave()
    {

        targetPosition = QueueCustomerManager.Instance.exitPosition.position; // Vị trí exit
        QueueCustomerManager.Instance.RemoveCustomer(this);
    }

    public void MoveToPosition(int newIndex)
    {
        queueIndex = newIndex;
        targetPosition = QueueCustomerManager.Instance.queuePositions[newIndex].position;
    }

    public void RequestRecipe()
    {
        currentRecipeSO = DeliveryManager.Instance.GetRandomRecipeSO();

        DeliveryManager.Instance.RequestRecipe(currentRecipeSO);
    }

    public void OutOfTimeDelivery()
    {
        targetPosition = QueueCustomerManager.Instance.exitPosition.position;
        QueueCustomerManager.Instance.RemoveCustomer(this);
        DeliveryManager.Instance.GetWaitingRecipeSOList().Remove(currentRecipeSO);

    }
}