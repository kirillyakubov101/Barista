using Barista.Core;
using Barista.Machines;

namespace Barista.Food
{
    public class OvenFood : SimpleTake
    {
        public override void DoAction()
        {
            base.m_state = ItemState.REFILL;
            m_graphics.SetActive(false);
            m_placeHolder.SetActive(true);

            //Send to Stove
            Stove.Instance.AddFoodToStove();
        }

    }
}
