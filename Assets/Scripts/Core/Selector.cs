using UnityEngine;
using Barista.MyCamera;
using Barista.Sounds;
using UnityEngine.InputSystem;
using Barista.Input;

namespace Barista.Core
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private LayerMask m_Layer;
        [SerializeField] private float m_RayCastDistance = 100f;

        private ISelectable m_selected;
        private CameraController m_CamController;
        private bool m_hasHit;
        private RaycastHit m_Hitinfo;

        private void Awake()
        {
            m_CamController = GetComponent<CameraController>();
        }


        private void OnEnable()
        {
            CameraController.OnCameraSwitch += AssignCamera;
            InputMasterControls.Instance.OnPrimaryAction += HandlePrimaryActionEvent;
        }

        private void OnDestroy()
        {
            CameraController.OnCameraSwitch -= AssignCamera;
            if (InputMasterControls.Instance)
            {
                InputMasterControls.Instance.OnPrimaryAction -= HandlePrimaryActionEvent;
            }
            
        }

        private void AssignCamera(Camera cam)
        {
            m_Camera = cam; 
        }

        private void HandlePrimaryActionEvent()
        {
            SoundHandler.Instance.PlayEmptyClick(false);
            RayCastFromMouse(out m_hasHit, out m_Hitinfo);
            if (m_hasHit)
            {
                //food
                if (m_Hitinfo.transform.TryGetComponent(out ISelectable item))
                {
                    FoodSelectionProcess(item);
                }
                //machines
                else if (m_Hitinfo.transform.TryGetComponent(out Machines.BeverageMachine machine))
                {
                    machine.MakeBeverage();
                }
                //cart
                else if (m_Hitinfo.transform.TryGetComponent(out Cart cart))
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

        private void RayCastFromMouse(out bool hit, out RaycastHit hitInfo)
        {
            Ray mouseRay = m_Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            hit = Physics.Raycast(mouseRay.origin, mouseRay.direction, out hitInfo, m_RayCastDistance, m_Layer);
        }
    }
}
