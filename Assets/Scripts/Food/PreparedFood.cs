using Barista.Core;
using Barista.Machines;

namespace Barista.Food
{
    public class PreparedFood : SimpleTake
    {
        public override void DoAction()
        {
            if (base.GetFoodType() == FoodType.Croissant)
            {
                Microwave.Instance.CloseMicro();
            }
            else if (base.GetFoodType() is FoodType.Toast)
            {
                Stove.Instance.EnableStove();
            }

            Cart.Instance.PopulateCart(GetFoodType());
            DiscardPreparedFood();
        }

        public void DiscardPreparedFood()
        {
            base.Deselect();
            gameObject.SetActive(false);
        }

        public void ShowPreparedFood()
        {
            gameObject.SetActive(true);
        }
    }
}
