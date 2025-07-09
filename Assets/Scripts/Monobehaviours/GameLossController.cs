using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLossController : MonoBehaviour
{
    [SerializeField] private Text unitsLeft;
    [SerializeField] private Text timeSinceStart;
    [SerializeField] private Button resetButton;
    const string unitsLeftText = "Unidades restando : ";
    const string timeSinceStartText = "Tempo Decorrido : ";
    private float timeCounter = 0.0f;
    private int unitsCounter;


    private void Start(){
        resetButton.onClick.AddListener(LoseGame);   
        resetButton.gameObject.SetActive(false);
    }

    private void Update(){

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Unit>().WithPresent<Selected>().Build(entityManager);
        NativeArray<Unit> unitNativeArray = entityQuery.ToComponentDataArray<Unit>(Allocator.Temp);

        unitsCounter = unitNativeArray.Length;

        if(unitsCounter > 0){
            timeCounter += Time.deltaTime;
        }
        else{
            if(!resetButton.gameObject.activeInHierarchy){
                resetButton.gameObject.SetActive(true);
            }
        }

        unitsLeft.text = unitsLeftText + unitsCounter;
        timeSinceStart.text = timeSinceStartText + timeCounter;
    }

    public void LoseGame(){
        SceneManager.LoadScene("GameScene");
        resetButton.gameObject.SetActive(false);
    }

}
