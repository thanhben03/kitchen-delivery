using System.Collections.Generic;
using UnityEngine;

public class QueueCustomerManager : MonoBehaviour
{
    public static QueueCustomerManager Instance { get; private set; }
    [Header("Queue Settings")]
    public Transform[] queuePositions; // Mảng vị trí hàng đợi (kéo từ Inspector, ví dụ 5 vị trí)
    public Transform exitPosition; // Vị trí khách hàng đi ra (kéo từ Inspector)
    public GameObject customerPrefab; // Prefab khách hàng
    public int maxQueueLength = 5; // Giới hạn số khách hàng
    public Transform spawnPosition; // Giới hạn số khách hàng

    public float spawnCustomerTimer = 0;
    public float spawnCustomerTimerMax = 4;

    private List<Customer> customerQueue = new List<Customer>(); // Danh sách khách hàng hiện tại


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //SpawnCustomer();
        DeliveryManager.Instance.OnRecipeSuccess += ServeNextCustomer;
    }

    private void Update()
    {
        //if (!KitchenGameManager.Instance.IsGamePlaying())
        //{
        //    return;
        //}
        spawnCustomerTimer -= Time.deltaTime;
        if (spawnCustomerTimer <= 0f)
        {
            spawnCustomerTimer = spawnCustomerTimerMax;
            if (customerQueue.Count >= maxQueueLength)
            {
                Debug.Log("Hàng đợi đầy!");
                return;
            }
            GameObject newCustomerObj = Instantiate(customerPrefab, spawnPosition.position, Quaternion.identity);
            Customer newCustomer = newCustomerObj.GetComponent<Customer>();
            newCustomer.queueIndex = customerQueue.Count;
            newCustomer.MoveToPosition(customerQueue.Count); // Đặt vị trí ban đầu
            newCustomer.RequestRecipe();
            customerQueue.Add(newCustomer);

            Debug.Log($"Đã spawn khách hàng tại vị trí {customerQueue.Count - 1}");
        }
        

        
    }

    public void SpawnCustomer()
    {
        
    }

    // Giao đồ ăn cho khách hàng đầu hàng (gọi khi người chơi hoàn thành nấu ăn)
    public void ServeNextCustomer()
    {
        if (customerQueue.Count == 0)
        {
            Debug.Log("Không có khách hàng nào!");
            return;
        }

        Customer frontCustomer = customerQueue[0];
        frontCustomer.ServeFoodAndLeave(); // Khách hàng di chuyển đi
        // customerQueue.RemoveAt(0); // Sẽ được gọi trong RemoveCustomer để tránh lỗi index
    }

    // Loại bỏ khách hàng và shift queue (gọi từ Customer.ServeFoodAndLeave)
    public void RemoveCustomer(Customer leavingCustomer)
    {
        if (!customerQueue.Contains(leavingCustomer))
        {
            return;
        }
        int removeIndex = leavingCustomer.queueIndex;

        // 1. Xóa khách ra khỏi list trước
        customerQueue.Remove(leavingCustomer);


        for (int i = removeIndex; i < customerQueue.Count; i++)
        {
            customerQueue[i].queueIndex = i;           // Cập nhật index mới
            customerQueue[i].MoveToPosition(i);        // Di chuyển về vị trí tương ứng
        }
        Destroy(leavingCustomer.gameObject, 2f);
    }

    // Getter để UI biết có khách hàng không
    public bool HasCustomers() => customerQueue.Count > 0;
    public Customer GetFrontCustomer() => customerQueue.Count > 0 ? customerQueue[0] : null;
}