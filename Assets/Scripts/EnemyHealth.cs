using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Texture2D frame;
    public Rect framePosition;
    public Texture2D healthBar;
    public Rect healthBarPosition;
    //1920x1080
    //w: 1000 h:500 y:-200
    float adjustXpos = 0.925f;
    float adjustWidth = 1.14f;

    public Combat player;
    public Mob target;
    public float healthPercentage;

    void Start()
    {

    }

    void Update()
    {
        if (player.opponent != null)
        {
            target = player.opponent.GetComponent<Mob>();
            healthPercentage = (float)target.health / target.maxHealth;
            //Debug.Log("Enemy health is at " + healthPercentage);
        }
        else
        {
            target = null;
            healthPercentage = 0;
        }
    }

    public void OnGUI()
    {
        if (target != null && player.countDown > 0)
        {
            drawFrame();
            drawBar();
        }
    }

    void drawFrame()
    {
        float width = 0.52f;
        framePosition.width = Screen.width * width;
        float height = 0.46f;
        framePosition.height = Screen.height * height;
        float yPos = -framePosition.width / 5f;
        framePosition.y = yPos;
        framePosition.x = (Screen.width - framePosition.width) / 2;
        GUI.DrawTexture(framePosition, frame);
    }

    void drawBar()
    {
        healthBarPosition.x = framePosition.x * adjustXpos;
        healthBarPosition.y = framePosition.y;
        healthBarPosition.width = framePosition.width * adjustWidth * healthPercentage;
        healthBarPosition.height = framePosition.height;
        //Debug.Log("Health bar width is " + healthBarPosition.width + " Health bar x pos is " + healthBarPosition.x + " Health bar y pos is " + healthBarPosition.y);

        GUI.DrawTexture(healthBarPosition, healthBar);
    }

}
