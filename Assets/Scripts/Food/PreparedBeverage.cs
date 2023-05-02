using Barista.Core;

namespace Barista.Food
{
    public class PreparedBeverage : Item, ISelectable
    {
        public void Select()
        {
            base.m_state = ItemState.SELECTED;
            m_outline.DisplayOutline(true);
        }
        public void Deselect()
        {
            base.m_state = ItemState.NORMAL;
            m_outline.DisplayOutline(false);
        }

        public void DoAction()
        {
            base.m_state = ItemState.NORMAL;
            Cart.Instance.PopulateCart(GetFoodType());
            m_outline.DisplayOutline(false);
            gameObject.SetActive(false);
        }

        public void Refil()
        {
            //
        }


        public ItemState State()
        {
            return base.m_state;
        }
    }
}
