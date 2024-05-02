using UnityEngine;
using Barista.MyCamera;

namespace Barista.Core
{
    public class Selector : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask layer;
        [SerializeField] private float m_clickCd = 0.3f;
        [SerializeField] private float m_RayCastDistance = 100f;

        private bool isClicked = false;
        private ISelectable m_selected;
        private CameraController m_CamController;

        private void Awake()
        {
            m_CamController = GetComponent<CameraController>();
        }

        float timer = 0f;
        private void Update()
        {
            if(timer < m_clickCd + 5)
            {
                timer += Time.deltaTime;
                timer = Mathf.Clamp(timer, 0f, 10f);
            }
            
          
            if (Input.GetMouseButtonDown(0))
            {
                isClicked = true;
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
            _camera = cam;
        }

        
        private void FixedUpdate()
        {
            if (isClicked && timer >= m_clickCd)
            {
                bool hit;
                RaycastHit hitInfo;
                RayCastFromMouse(out hit, out hitInfo);

                if (hit)
                {
                    //food
                    if (hitInfo.transform.TryGetComponent(out ISelectable item))
                    {
                        FoodSelectionProcess(item);
                    }
                    //machines
                    else if (hitInfo.transform.TryGetComponent(out Machines.BeverageMachine machine))
                    {
                        machine.MakeBeverage();
                    }
                    else if(hitInfo.transform.TryGetComponent(out Cart cart))
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
            isClicked = false;
            timer = 0f;
            Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);

            hit = Physics.Raycast(mouseRay.origin, mouseRay.direction, out hitInfo, m_RayCastDistance, layer);
        }
    }
}
