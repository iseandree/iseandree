using UnityEngine;

public class ProductivityUnit : Unit
{
    // Private variables
    private ResourcePile currentPile = null;
    private float productivityModifier = 2.0f;


    protected override void BuildingInRange()
    {
        if(currentPile == null)
        {
            ResourcePile pile = m_Target as ResourcePile;

            if(pile != null)
            {
                currentPile = pile;
                currentPile.ProductionSpeed *= productivityModifier;
            }
        }
    }

    private void ResetProduction()
    {
        if(currentPile != null)
        {
            currentPile.ProductionSpeed /= productivityModifier;
            currentPile = null;
        }
    }

    public override void GoTo(Building target)
    {
        ResetProduction();
        base.GoTo(target);
    }

    public override void GoTo(Vector3 position)
    {
        ResetProduction();
        base.GoTo(position);
    }
}
