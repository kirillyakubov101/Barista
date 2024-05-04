using UnityEngine;
using Barista.MyCamera;

namespace Barista.Core
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Camera m_Camera;
        [SerializeField] private LayerMask m_Layer;
        [SerializeField] private float m_clickCd = 0.3f;
        [SerializeField] private float m_RayCastDistance = 100f;

        private bool m_isClicked = false;
        private ISelectable m_selected;
        private CameraController m_CamController;
        private bool m_hasHit;
        private RaycastHit m_Hitinfo;
        private float m_Timer = 0f;

        private void Awake()
        {
            m_CamController = GetComponent<CameraController>();
        }

       
        private void Update()
        {
            if(m_Timer < m_clickCd + 5)
            {
                m_Timer += Time.deltaTime;
                m_Timer = Mathf.Clamp(m_Timer, 0f, 10f);
            }
            
          
            if (Input.GetMouseButtonDown(0))
            {
                m_isClicked = true;
            }
        }

        private void OnEnable()
        {
            CameraController.OnCameraSwitch += AssignCamera;
        }

        private void OnDestroy()
        {
            CameraController.OnCameraSwitch -= AssignCamera;
        }

        private void AssignCamera(Camera cam)
        {
            m_Camera = cam;
        }

        
        private void FixedUpdate()
        {
            if (m_isClicked && m_Timer >= m_clickCd)
            {
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
                    else if(m_Hitinfo.transform.TryGetComponent(out Cart cart))
                    {
                        cart.SubmitCartOrder();
                    }
                }
                else
                {
                    if (m_selected != null) { m_selected.Deselect(); m_selected = null; }
                }
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
            m_isClicked = false;
            m_Timer = 0f;
            Ray mouseRay = m_Camera.ScreenPointToRay(Input.mousePosition);

            hit = Physics.Raycast(mouseRay.origin, mouseRay.direction, out hitInfo, m_RayCastDistance, m_Layer);
        }
    }
}
