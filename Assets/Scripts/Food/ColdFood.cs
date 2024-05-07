using Barista.Core;
using Barista.Machines;

namespace Barista.Food
{
    public class ColdFood : SimpleTake
    {
        public override void DoAction()
        {
            if (!Microwave.Instance.CanMicro)
            {
                base.Deselect();
                return;
            }
            base.m_state = ItemState.REFILL;
            m_graphics.SetActive(false);
            m_placeHolder.SetActive(true);

           Microwave.Instance.AddFoodToMicro();
        }


    }
}
