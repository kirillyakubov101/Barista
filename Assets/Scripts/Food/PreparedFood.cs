using Barista.Core;
using Barista.Machines;

namespace Barista.Food
{
    public class PreparedFood : SimpleTake
    {
        public override void DoAction()
        {
            base.m_state = ItemState.REFILL;
            m_graphics.SetActive(false);
            if (m_placeHolder != null)
            {
                m_placeHolder.SetActive(true);
            }

            Microwave.Instance.CloseMicro();
            Cart.Instance.PopulateCart(GetFoodType());
            Refil();
            gameObject.SetActive(false);
        }

        public override void Refil()
        {
            base.m_state = ItemState.NORMAL;
            m_graphics.SetActive(true);

            if (m_placeHolder != null)
            {
                m_placeHolder.SetActive(false);
            }

            m_outline.DisplayOutline(false);
        }
    }
}
