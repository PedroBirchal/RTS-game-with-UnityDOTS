using UnityEngine;

public class MouseWorldPosition : MonoBehaviour{

    public static MouseWorldPosition instance { get; private set; }
    [SerializeField] private float maxRayDistance = 100.0f;
    [SerializeField] private LayerMask layerMask;


    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }


    public Vector3 GetPosition(){
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out RaycastHit hitInfo, maxRayDistance, layerMask)) {
            return hitInfo.point;
        }
        else{
            return Vector3.zero;
        }
    }

}
