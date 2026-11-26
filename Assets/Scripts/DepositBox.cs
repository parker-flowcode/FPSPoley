using UnityEngine;

public class DepositBox : MonoBehaviour
{
    public void DepositCoins()
    {
        if (GameManager.Instance == null) return;

        int deposited = GameManager.Instance.DepositAllCoins();
        if (deposited > 0)
        {
            Debug.Log($"Has depositado {deposited} monedas. Total en baúl: {GameManager.Instance.depositedCoins}");
        }
        else
        {
            GameManager.Instance.ShowMessage("No llevas monedas para depositar");
        }
    }
}
