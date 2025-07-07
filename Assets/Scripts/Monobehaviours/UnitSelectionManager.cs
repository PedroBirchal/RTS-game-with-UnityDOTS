using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class UnitSelectionManager : MonoBehaviour {

    public static UnitSelectionManager instance {get; private set;}

    public event EventHandler OnSelectionAreaStart;
    public event EventHandler OnSelectionAreaEnd;

    private Vector2 selectionStartMousePosition;
    [SerializeField] private float minimumSelectionAreaSize = 40.0f;
    [SerializeField] private int unitsLayer;


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
        }
        if(Input.GetMouseButtonUp(0)){
            Vector2 selectionEndMousePosition = Input.mousePosition;

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Deselecting units:
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            foreach( Entity entity in entityArray){
                entityManager.SetComponentEnabled<Selected>(entity, false);
            }

            // Selecting Units inside the selection rect area:
            Rect selectionAreaRect = GetSelectionAreaRect();
            float selectionAreaSize = selectionAreaRect.width + selectionAreaRect.height;
            if(selectionAreaSize < minimumSelectionAreaSize){
                // Single unit selection:
                Debug.Log("Single unit selection");
                entityQuery = entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
                UnityEngine.Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastInput raycastInput = new RaycastInput{
                    Start = cameraRay.GetPoint(0f),
                    End = cameraRay.GetPoint(1000f),
                    Filter = new CollisionFilter{
                        BelongsTo = ~0u,
                        CollidesWith = 1u << unitsLayer,
                        GroupIndex = 0,
                    }
                };
                if(collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit)){
                    if(entityManager.HasComponent<Unit>(raycastHit.Entity)){
                        entityManager.SetComponentEnabled<Selected>(raycastHit.Entity, true);
                    }
                }
            }
            else{
                // Select multiple units at once:
                entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>().WithPresent<Selected>().Build(entityManager);

                entityArray = entityQuery.ToEntityArray(Allocator.Temp);
                NativeArray<LocalTransform> localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                for(int i = 0 ; i < localTransformArray.Length; i++){
                    LocalTransform localTransform = localTransformArray[i];
                    Vector2 unitScreenPosition = Camera.main.WorldToScreenPoint(localTransform.Position);
                    if(selectionAreaRect.Contains(unitScreenPosition)){
                        entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                    }
                }
            }

            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);
        }
        if(Input.GetMouseButtonDown(1)){
            Vector3 mouseWorldPosition = MouseWorldPosition.instance.GetPosition();
            
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMovement, Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<UnitMovement> unitMovementArray = entityQuery.ToComponentDataArray<UnitMovement>(Allocator.Temp);
            NativeArray<float3> movePositionArray = GenerateMovePositionArray(mouseWorldPosition, entityArray.Length);
            for (int i = 0; i < unitMovementArray.Length; i++ ){
                UnitMovement unitMovement = unitMovementArray[i];
                unitMovement.targetPosition = movePositionArray[i];
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

    private NativeArray<float3> GenerateMovePositionArray(float3 targetPosition, int positionCount){
        NativeArray<float3> positionArray = new NativeArray<float3>(positionCount, Allocator.Temp);
        if(positionCount == 0) return positionArray;
        positionArray[0] = targetPosition;
        if(positionCount == 1) return positionArray;

        float ringSize = 2.2f;
        int ring = 0;
        int positionIndex = 1;

        while (positionIndex < positionCount) {
            int ringPositionCount = 3 + ring * 2;

            for(int i = 0; i < ringPositionCount; i++) {
                float angle = i * (math.PI2 / ringPositionCount);
                float3 ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1), 0, 0));
                float3 ringPosition = targetPosition + ringVector;

                positionArray[positionIndex] = ringPosition;
                positionIndex++;

                if(positionIndex >= positionCount){
                    break;
                }
            }
            ring++;
        }
        return positionArray;
    }

}
