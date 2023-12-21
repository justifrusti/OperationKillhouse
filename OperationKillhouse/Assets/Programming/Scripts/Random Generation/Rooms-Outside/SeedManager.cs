using UnityEngine.UI;
using UnityEngine;
using TMPro;
using static Generator;

public class SeedManager : MonoBehaviour
{
    public string stringSeed = "seed string";
    public bool useStringSeed;
    public int seed;
    public bool randomizeSeed;
    public Toggle useFillInSeed;
    public TMP_InputField seedInputField;
    public TMP_InputField usedSeedField;
    public TMP_Dropdown deficultyselector;

    public Generator generator;

    [Space]
    [Tooltip("If true a new dungeon will be generated")] public bool reset;
    private void Awake()
    {
        generator.canGenarate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
            ResetGenerator();
    }

    public void ResetGenerator()
    {
        setSeed();

        if (generator.dungeonRooms.Count > 0)
        {
            for (int i = 0; i < generator.dungeonRooms.Count; i++)
            {
                Destroy(generator.dungeonRooms[i]);
                generator.dungeonRooms.RemoveAt(i);
            }
        }

        if (generator.dungeonRooms.Count == 0)
        {
            generator.firstRoom.GetComponent<RoomManager>().RoomReset();

            generator.DifficultySelect(generator.difficulty);

            generator.SetRetryAmount(generator.retryAmount);
            generator.dungeonGenerationComplete = false;
            generator.firstRoom.GetComponent<RoomManager>().spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;

            generator.canGenarate = true;
            reset = false;
        }
    }

    public void ResetGen()
    {
        reset = true;
    }

    public void setSeed()
    {
        if (useFillInSeed.isOn)
        {
            useStringSeed = true;
            randomizeSeed = false;
        }
        else
        {
            useStringSeed = false;
            randomizeSeed = true;
        }

        if (useStringSeed)
        {
            stringSeed = seedInputField.text;
            usedSeedField.text = stringSeed;
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed)
        {
            seed = Random.Range(0, 99999);
            usedSeedField.text = seed.ToString();
        }

        Random.InitState(seed);
    }

    public void dificultySelect()
    {
        if (deficultyselector.value == 0)
        {
            generator.DifficultySelect(Difficulty.Easy);
        }
        else if(deficultyselector.value == 1)
        {
            generator.DifficultySelect(Difficulty.Medium);
        }
        else if (deficultyselector.value == 2)
        {
            generator.DifficultySelect(Difficulty.Hard);
        }
    }
}
