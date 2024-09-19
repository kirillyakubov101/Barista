using Barista.Core;
using UnityEngine;

namespace Barista.Food
{
    public class PreparedBeverage : Item, ISelectable
    {
        [SerializeField] private float m_TimeToExpire = 2f;
        [SerializeField] private ParticleSystem m_expiredFoodVFX = null;

        private bool m_Expired = false;

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

            if (m_Expired)
            {
                print("trash it");
            }
            else
            {
                Cart.Instance.PopulateCart(GetFoodType());
            }

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

        private void OnEnable()
        {
            Invoke(nameof(Expire), m_TimeToExpire);
        }

        private void OnDisable()
        {
            m_Expired = false;
            m_expiredFoodVFX.Stop();
            m_outline.OutlineColor = m_outline.DefaultColor;
        }

        private void Expire()
        {
            if (gameObject.activeSelf)
            {
                m_Expired = true;
                m_expiredFoodVFX.Play();
                m_outline.OutlineColor = m_outline.TrashOutline;
                m_outline.UpdateMatsRuntime();
            }
        }
    }
}
