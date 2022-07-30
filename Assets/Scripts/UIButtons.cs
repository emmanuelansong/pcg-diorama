using sc.terrain.vegetationspawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIButtons : MonoBehaviour
{
   
    static bool isOrthographic = false;
    PostProcessVolume ppv;
    //switch betwen orthographic and perspective
    private void Start()
    {
        ppv = Camera.main.GetComponent<PostProcessVolume>();

        ppv.profile.TryGetSettings(out MotionBlur mb);

        mb.shutterAngle.value = 30;
    }
    public void ChangePerspective()
    {
        ppv = Camera.main.GetComponent<PostProcessVolume>();

        ppv.profile.TryGetSettings(out MotionBlur mb);
        //turn perspective mode on
        if (isOrthographic)
        {
            Camera.main.orthographic = false;
            isOrthographic = false;
            mb.shutterAngle.value = 30;

        }

        //orthographic mode
        else
        {
            Camera.main.orthographic = true;
            isOrthographic = true;

            mb.shutterAngle.value = 0;

        }
    }

    public void Regenerate()
    {
        
        GameObject treeGen = GameObject.Find("TreeGen");
        GameObject rockGen = GameObject.Find("RockGen");
        //List<GameObject> rockGenChildren = rockGen.transform.chil

        Terrain.activeTerrain.GetComponent<TerrainGeneration>().generate();
        VegetationSpawner spawner = GameObject.Find("VegetationGen").GetComponent<VegetationSpawner>();

        spawner.RandomizeSeed();
        spawner.RespawnTerrain();

        for (int i = 0; i < rockGen.transform.childCount; i++)
        {
            Destroy(rockGen.transform.GetChild(i).gameObject);
        }

       rockGen.GetComponent<RockGen>().ChooseAndPlaceRandomRock();
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    

}
