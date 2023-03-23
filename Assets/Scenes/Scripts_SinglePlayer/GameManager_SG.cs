using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_SG : MonoBehaviour
{
    public Dog_SG[] dogs;
    public Cat_SG cat;
    public Transform foods;
    public Text Score_txt;
    //-------New adding--------
    public bool energyEaten;
    public bool ItemAEaten;
    public bool ItemBEaten;
    private int noItemA = 0;
    private int noItemB = 0;
    //-----------------------
    public GameObject winScene;
    public GameObject loseScene;
    public GameObject Joystick;
    public Text Score_win;
    public Text Score_lose;

    public int score;

    public int lives { get; private set; }


    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        score = 0;
        SetScore(score);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {//-----Display only original dog
        foreach (Transform foods in this.foods)
        {
            foods.gameObject.SetActive(true);
        }
        for (int i = 0; i < this.dogs.Length; i++)
        {
            if (dogs[i].gameObject.name == "Dog_Red_SG")
            {
                dogs[i].transform.position = new Vector3(-12.5f, 14.42f, -5f);
                dogs[i].gameObject.SetActive(true);
            }
            else if (dogs[i].gameObject.name == "Dog_Green_SG")
            {
                dogs[i].transform.position = new Vector3(-12.4f, -14.46f, -5f);
                dogs[i].gameObject.SetActive(true);
            }
            else if (dogs[i].gameObject.name == "Dog_Blue_SG")
            {
                dogs[i].transform.position = new Vector3(10.9f, 14.58f, -5f);
                dogs[i].gameObject.SetActive(true);
            }
            else if (dogs[i].gameObject.name == "Dog_Yellow_SG")
            {
                dogs[i].transform.position = new Vector3(10.65f, -14.42f, -5f);
                dogs[i].gameObject.SetActive(true);
            }
        }
        RespawnCat();
    }

    private void GameOver()
    {
        for (int i = 0; i < this.dogs.Length; i++)
        {
            this.dogs[i].gameObject.SetActive(false);
        }
        this.cat.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        Score_txt.text = "Score : " + score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void DogEaten(Dog_SG dog)
    {
        SoundManager_SG.PlaySound("DogDied");
        dog.gameObject.SetActive(false);
        score += dog.points;
        SetScore(score);
        StartCoroutine(RespawnDog(dog));
    }

    public void CatEaten()
    {
        SoundManager_SG.PlaySound("CatDied");
        this.cat.gameObject.SetActive(false);
        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke("RespawnCat", 2.0f);
        }
        else
        {
            Case("lose");
        }
    }

    public void NormalFoodEaten(NormalFood_SG normal_food)
    {
        normal_food.gameObject.SetActive(false);
        score += normal_food.points;
        SetScore(score);

        if (CheckingAllFoods())
        {
            SoundManager_SG.PlaySound("Victory");
            this.cat.gameObject.SetActive(false);
            Case("win");
        }
    }

    public void CatEnergyEaten(CatEnergy_SG cat_energy)
    {
        SoundManager_SG.PlaySound("CatEnergyEaten");
        NormalFoodEaten(cat_energy);
        energyEaten = true;
        Invoke("ResetEnergy", 8.0f);
    }
    //---------------------------ItemEaten----------------------------
    public void ItemAAEaten(ItemA itemA)
    {
        SoundManager_SG.PlaySound("CatEnergyEaten");
        NormalFoodEaten(itemA);
        ItemAEaten = true;
        noItemA += 1;
        DecreaseeDog();
        itemA.gameObject.SetActive(false);
    }

    public void ItemBBEaten(ItemB itemB)
    {
        SoundManager_SG.PlaySound("CatEnergyEaten");
        NormalFoodEaten(itemB);
        ItemBEaten = true;
        noItemB += 1;
        IncreaseDog();
        itemB.gameObject.SetActive(false);
    }
    public void ItemCCEaten(ItemC itemC)
    {
        SoundManager_SG.PlaySound("CatEnergyEaten");
        NormalFoodEaten(itemC);
        HoldTheDog();
        itemC.gameObject.SetActive(false);
    }
    //-------------------Function-For-EachItem-----------------
    public void HoldTheDog()
    {
        for (int i = 0; i < this.dogs.Length; i++)
        {
            dogs[i].movement.speed = 0;
        }
        Invoke("ResetSpeedOfDog", 3.0f);
    }
    void ResetSpeedOfDog() {
        for (int i = 0; i < this.dogs.Length; i++)
        {
            dogs[i].movement.speed = 5;
        }
    }

    public void IncreaseDog() { //-------Increase dog by one
        if (noItemB ==1) { 
            dogs[4].gameObject.SetActive(true);
            dogs[4].transform.position = new Vector3(-0.38f, -14.47f, -5f);
        }
        else if (noItemB == 2)
        {
            dogs[5].gameObject.SetActive(true);
            dogs[5].transform.position = new Vector3(-0.38f, -14.47f, -5f);
        }
        else { 
            dogs[6].gameObject.SetActive(true);
            dogs[6].transform.position = new Vector3(-0.38f, -14.47f, -5f);
        }

    }
    public void DecreaseeDog() { //-------Decrease dog by one
        if (noItemA == 1)
        {
            dogs[0].gameObject.SetActive(false);
        }
        else if (noItemA == 2)
        {
            dogs[1].gameObject.SetActive(false);
        }
        else if (noItemA == 3)
        {
            dogs[2].gameObject.SetActive(false);
        }
        else if (noItemA == 4)
        {
            dogs[3].gameObject.SetActive(false);
        }
        else if (noItemA == 5)
        {
            dogs[4].gameObject.SetActive(false);
        }
    }
    //---------------------------------------------------------
    private bool CheckingAllFoods()
    {
        foreach (Transform foods in this.foods)
        {
            if (foods.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
    //--------------------Reset-EachItem------------------------
    private void ResetEnergy()
    {
        energyEaten = false;
    }
    private void ResetEnergyAA()
    {
        ItemAEaten = false;
    }
    private void ResetEnergyBB()
    {
        ItemBEaten = false;
    }
    //------------------------------------------------------

    private IEnumerator RespawnDog(Dog_SG dog)
    {
        yield return new WaitForSeconds(5);
        if (dog.gameObject.name == "Dog_Red_SG")
        {
            dog.transform.position = new Vector3(-12.5f, 14.42f, -5f);
        }
        else if (dog.gameObject.name == "Dog_Green_SG")
        {
            dog.transform.position = new Vector3(-12.4f, -14.46f, -5f);
        }
        else if (dog.gameObject.name == "Dog_Blue_SG")
        {
            dog.transform.position = new Vector3(10.9f, 14.58f, -5f);
        }
        else if (dog.gameObject.name == "Dog_Yellow_SG")
        {
            dog.transform.position = new Vector3(10.65f, -14.42f, -5f);
        }
        //------new adding
        else if (dog.gameObject.name == "Dog_Daze_SG_(0) ")
        {
            dog.transform.position = new Vector3(-0.38f, -14.47f, -5f);
        }
        else if (dog.gameObject.name == "Dog_Daze_SG_(1)")
        {
            dog.transform.position = new Vector3(10.57f, 6.53f, -5f);
        }
        else if (dog.gameObject.name == "Dog_Daze_SG_(2)")
        {
            dog.transform.position = new Vector3(0.4f, 14.7f, -5f);
        }
        dog.gameObject.SetActive(true);
    }

    public void RespawnCat()
    {
        this.cat.transform.position = new Vector3(-0.37f, -0.72f, -5f);
        this.cat.gameObject.SetActive(true);
    }

    private void Case(string casename)
    {
        Time.timeScale = 0f;
        Joystick.SetActive(false);
        if (casename == "win")
        {
            Debug.Log("win");
            winScene.SetActive(true);
            Score_win.text = "Score : " + score;
        }
        else if (casename == "lose")
        {
            Debug.Log("Lose");
            loseScene.SetActive(true);
            Score_lose.text = "Score : " + score;
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
        Joystick.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
        Joystick.SetActive(true);
        Time.timeScale = 1f;
    }
}

