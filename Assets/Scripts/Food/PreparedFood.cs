using Barista.Core;
using Barista.Machines;
using UnityEngine;

namespace Barista.Food
{
    public class PreparedFood : SimpleTake
    {
        [SerializeField] private float m_TimeToExpire = 2f;
        [SerializeField] private ParticleSystem m_expiredFoodVFX = null;

        private bool m_Expired = false;

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

            if(!m_Expired)
            {
                Cart.Instance.PopulateCart(GetFoodType());
            }
           
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

        private void OnEnable()
        {
            Invoke(nameof(Expire), m_TimeToExpire);
        }

        private void OnDisable()
        {
            m_Expired = false;
            m_expiredFoodVFX.Stop();
        }

        private void Expire()
        {
            if(gameObject.activeSelf)
            {
                m_Expired = true;
                m_expiredFoodVFX.Play();
            }
        }
    }
}
