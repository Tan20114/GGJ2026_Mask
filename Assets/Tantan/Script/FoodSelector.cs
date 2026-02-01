using UnityEngine;

public class FoodSelector : MonoBehaviour
{
    public void SetChicken() => GameManager.Instance.productToSell = Product.Chicken;
    public void SetBeef() => GameManager.Instance.productToSell = Product.Beef;
    public void SetLamb() => GameManager.Instance.productToSell = Product.Lamb;
    public void SetIllegal() => GameManager.Instance.productToSell = Product.Illegal;
}
