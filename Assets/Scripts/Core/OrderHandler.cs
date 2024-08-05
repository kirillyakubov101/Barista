using Barista.Clients;
using Barista.Food;
using Barista.Menu;
using Barista.Sounds;
using Barista.UI;
using MyUtils;
using System;
using System.Collections.Generic;

namespace Barista.Order
{
    public class OrderHandler : Singleton<OrderHandler>
    {
        public event Action OnOrderGenerated;
        public event Action<bool> OnOrderComplete;

        public ClientPawn m_currentClient = null;

        public void SubmitOrderToClient(List<FoodType> PickedFoodTypes)
        {
            if(IsCorrectRecipe(PickedFoodTypes))
            {
                SoundHandler.Instance.PlayCorrectSound(true);
                OnOrderComplete?.Invoke(true);
                m_currentClient.RecieveOrder(true);
            }
            else
            {
                ErrorSystem.Instance.DisplayError(ErrorType.WrongOrder);
                OnOrderComplete?.Invoke(false);
                m_currentClient.RecieveOrder(false);
            }

            m_currentClient = null;
        }

        //Client ordered food
        public void TakeOrderFromClient(ClientPawn client)
        {
            m_currentClient = client;
            OnOrderGenerated?.Invoke();
        }

     
        public bool IsCorrectRecipe(List<FoodType> PickedFoodTypes)
        {
            var recipe = MenuFactory.Instance.CurrentRecipe;

            for (int i = 0; i < PickedFoodTypes.Count; i++)
            {
                FoodType key = PickedFoodTypes[i];
                if (recipe.ContainsKey(key))
                {
                    MenuFactory.Instance.RemoveItemsFromRecipe(key);
                }
                else
                {
                    //no key like that (too many of these item on the cart)
                    return false;
                }
            }

            return recipe.Count == 0;
        }

        //Override the singleton destroy
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnOrderGenerated = null;
            OnOrderComplete = null;

        }

    }
}
