using Barista.Core;
using Barista.Machines;

namespace Barista.Food
{
    public class PreparedFood : SimpleTake
    {
        public override void DoAction()
        {
            gameObject.SetActive(false);

            if (base.GetFoodType() == FoodType.Croissant)
            {
                Microwave.Instance.CloseMicro();
            }
            else if(base.GetFoodType() is FoodType.Toast)
            {
                Stove.Instance.EnableStove();
            }

           

            Cart.Instance.PopulateCart(GetFoodType());
            DiscardPreparedFood();
            
        }

        public void DiscardPreparedFood()
        {  
            m_outline.DisplayOutline(false);
            base.m_state = ItemState.NORMAL;
        }
    }
}
