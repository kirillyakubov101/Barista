using UnityEngine;
using Barista.Sounds;
using Barista.Input;

namespace Barista.Core
{
    public class Selector : MonoBehaviour
    {
        private ISelectable m_selected;
        private Raycaster m_Raycaster;

        private void Awake()
        {
            m_Raycaster = GetComponent<Raycaster>();
        }


        private void OnEnable()
        {
            InputMasterControls.Instance.OnPrimaryAction += HandlePrimaryActionEvent;
        }

        private void OnDestroy()
        {
            if (InputMasterControls.Instance)
            {
                InputMasterControls.Instance.OnPrimaryAction -= HandlePrimaryActionEvent;
            }
            
        }

        private void HandlePrimaryActionEvent()
        {
            SoundHandler.Instance.PlayEmptyClick(false);
           
            if (m_Raycaster.HasHit)
            {
                //food
                if (m_Raycaster.Hitinfo.transform.TryGetComponent(out ISelectable item))
                {
                    FoodSelectionProcess(item);
                }
                //machines
                else if (m_Raycaster.Hitinfo.transform.TryGetComponent(out Machines.BeverageMachine machine))
                {
                    machine.MakeBeverage();
                }
                //cart
                else if (m_Raycaster.Hitinfo.transform.TryGetComponent(out Cart cart))
                {
                    cart.SubmitCartOrder();
                }

            }
            else
            {
                if (m_selected != null) { m_selected.Deselect(); m_selected = null; }

                SoundHandler.Instance.PlayEmptyClick(true);
            }
        }
        private void FoodSelectionProcess(ISelectable item)
        {
            //if no selected
            if (m_selected == null) { m_selected = item; }
            //if there is a selected item already
            else if (m_selected != null && m_selected != item) { m_selected.Deselect(); m_selected = item; }

            switch (item.State())
            {
                case ItemState.NORMAL:
                    item.Select();
                    break;

                case ItemState.SELECTED:
                    item.DoAction();
                    m_selected = null;
                    break;

                case ItemState.REFILL:
                    item.Refil();
                    m_selected = null;
                    break;
            }
        }
    }
}
