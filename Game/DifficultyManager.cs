using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty
    {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        VeryHard,
        UhOh
    }

    public Difficulty difficulty;
    public Timer timer;
    public EnemyAttributes enemyAttributes;
    public TMP_Text difficultyText;
    public int maxTimeSeconds;
    [Header("Post Processing")]
    public Volume global;
    private Vignette vignette;
    public Light2D globalLight;

    private void Start()
    {
        timer = gameObject.GetComponent<Timer>();
        enemyAttributes = gameObject.transform.GetChild(0).GetComponent<EnemyAttributes>();
        global.profile.TryGet(out vignette);
    }

    private void FixedUpdate() {
        UpdateDifficulty();
        SetText();
        UpdatePostProcessing();
    }

    private void UpdateDifficulty() {
        float t = timer.currentTime;
        Difficulty newDifficulty;

        if (t >= 15 * 60) {
            newDifficulty = Difficulty.UhOh;
        } else if (t >= 12 * 60) {
            newDifficulty = Difficulty.VeryHard;
        } else if (t >= 8 * 60) {
            newDifficulty = Difficulty.Hard;
        } else if (t >= 4 * 60) {
            newDifficulty = Difficulty.Medium;
        } else if (t >= 2 * 60) {
            newDifficulty = Difficulty.Easy;
        } else {
            newDifficulty = Difficulty.VeryEasy;
        }

        if (newDifficulty != difficulty) {
            difficulty = newDifficulty;
            enemyAttributes.IncreaseDifficulty();
        }
    }

    private void SetText(){
        switch(difficulty){
            case Difficulty.VeryEasy:
                difficultyText.text = "Very Easy";
                break;
            case Difficulty.Easy:
                difficultyText.text = "Easy";
                break;
            case Difficulty.Medium:
                difficultyText.text = "Medium";
                break;
            case Difficulty.Hard:
                difficultyText.text = "Hard";
                break;
            case Difficulty.VeryHard:
                difficultyText.text = "Very Hard";
                break;
            case Difficulty.UhOh:
                difficultyText.text = "Uh Oh";
                break;
        }
    }

    private void UpdatePostProcessing() {
        global.profile.TryGet(out vignette);

        float t = timer.currentTime;
        float intensity = Mathf.Lerp(0f, 0.6f, t / maxTimeSeconds);
        vignette.intensity.value = intensity;

        float lightIntensity = Mathf.Lerp(0.15f, 0.025f, t / maxTimeSeconds);
        globalLight.intensity = lightIntensity;
    }
}
