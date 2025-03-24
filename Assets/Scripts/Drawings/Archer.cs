public class Archer : Drawing
{
    protected override void BehaviourOfActivatedObject()
    {
        base.BehaviourOfActivatedObject();
        if(effectPrefab != null)
        {
            effectGameObject=Instantiate(effectPrefab, transform.position, effectPrefab.transform.rotation); 
            effectGameObject.GetComponent<Effect>().createdFigure = this.gameObject;
        }
    }
}
