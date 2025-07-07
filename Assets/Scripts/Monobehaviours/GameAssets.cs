using UnityEngine;

public class GameAssets : MonoBehaviour {

    public const int UNITS_LAYER = 7; 


    public static GameAssets Instance;


    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }    

}
