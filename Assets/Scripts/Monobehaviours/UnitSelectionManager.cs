using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour {

    public static UnitSelectionManager instance {get; private set;}

    public event EventHandler OnSelectionAreaStart;
    public event EventHandler OnSelectionAreaEnd;

    private Vector2 selectionStartMousePosition;


    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            selectionStartMousePosition = Input.mousePosition;
            OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
            Debug.Log(selectionStartMousePosition);
        }
        if(Input.GetMouseButtonUp(0)){
            Vector2 selectionEndMousePosition = Input.mousePosition;
            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);
            Debug.Log(selectionEndMousePosition);
        }
        if(Input.GetMouseButtonDown(1)){
            Vector3 mouseWorldPosition = MouseWorldPosition.instance.GetPosition();
            
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMovement, Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);

            NativeArray<UnitMovement> unitMovementArray = entityQuery.ToComponentDataArray<UnitMovement>(Allocator.Temp);

            for (int i = 0; i < unitMovementArray.Length; i++ ){
                UnitMovement unitMovement = unitMovementArray[i];
                unitMovement.targetPosition = mouseWorldPosition;
                unitMovementArray[i] = unitMovement;
            }
            entityQuery.CopyFromComponentDataArray(unitMovementArray);
        }
    }

    public Rect GetSelectionAreaRect(){
        Vector2 selectionEndMousePosition = Input.mousePosition;

        Vector2 lowerLeftCorner = new Vector2(
            Mathf.Min(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Min(selectionStartMousePosition.y, selectionEndMousePosition.y)
        );
        Vector2 upperRightCorner = new Vector2(
            Mathf.Max(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Max(selectionStartMousePosition.y, selectionEndMousePosition.y)
        );

        return new Rect(lowerLeftCorner, upperRightCorner - lowerLeftCorner);
    }

}
