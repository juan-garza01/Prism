using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorStats : MonoBehaviour
{
    private int bar1, bar2, bar3, b1count, b2count, b3count;
    public int red, blue, yellow;
    public int changeState = 0;
    public float count = 0.0f;
    private Transform b1, b2, b3;
    private GameObject swordIcon, shieldIcon, bowIcon;
    public Image bar1pic, bar2pic, bar3pic, icon1, icon2, icon3, iconsw, iconsh, iconbw, iconswColor, iconshColor, iconbwColor;
    public Sprite first, middle, last;
    public Sprite[] spriteArray;

    [SerializeField] private AudioSource iconChange;
    [SerializeField] private AudioSource colorbarFill1, colorbarFill2, colorbarFill3;


    // Start is called before the first frame update
    void Start()
    {
        b1 = GameObject.Find("bigbar1").gameObject.transform;
        b2 = GameObject.Find("bigbar2").gameObject.transform;
        b3 = GameObject.Find("bigbar3").gameObject.transform;

        bar1pic = GameObject.Find("firstColor").GetComponent<Image>();
        bar2pic = GameObject.Find("secondColor").GetComponent<Image>();
        bar3pic = GameObject.Find("thirdColor").GetComponent<Image>();

        swordIcon = GameObject.Find("SwordIcon");
        shieldIcon = GameObject.Find("ShieldIcon");
        bowIcon = GameObject.Find("BowIcon");

        icon1 = swordIcon.GetComponent<Image>();
        icon2 = shieldIcon.GetComponent<Image>();
        icon3 = bowIcon.GetComponent<Image>();

        iconsw = swordIcon.gameObject.transform.GetChild(1).GetComponent<Image>();
        iconsh = shieldIcon.gameObject.transform.GetChild(1).GetComponent<Image>();
        iconbw = bowIcon.gameObject.transform.GetChild(1).GetComponent<Image>();

        iconswColor = swordIcon.gameObject.transform.GetChild(2).GetComponent<Image>();
        iconshColor = shieldIcon.gameObject.transform.GetChild(2).GetComponent<Image>();
        iconbwColor = bowIcon.gameObject.transform.GetChild(2).GetComponent<Image>();

        iconswColor.enabled = false;
        iconshColor.enabled = false;
        iconbwColor.enabled = false;

        icon1.sprite = spriteArray[6];
        iconsw.sprite = spriteArray[18];
        iconswColor.sprite = spriteArray[19];

    }

    void Update()
    {
        count += Time.deltaTime;

        if (count >= 0.25f)
        {
            if(b1count != bar1)
            {
                colorbarFill1.Play();
                b1count = fill(b1, b1count, bar1);
            }
            if(b2count != bar2)
            {
                colorbarFill2.Play();
                b2count = fill(b2, b2count, bar2);
            }
            if(b3count != bar3)
            {
                colorbarFill3.Play();
                b3count = fill(b3, b3count, bar3);
            }

            count = 0.0f;
        }
    }

    private int fill(Transform b, int bcount, int bar){
        if (bcount < bar)
        {
            b.GetChild(bcount).gameObject.SetActive(true);
            bcount++;
        }
        else if (bcount > bar)
        {
            bcount--;
            b.GetChild(bcount).gameObject.SetActive(false);
        }
        return bcount;
    }

    private int check(int color){
        if (color <= 0){
            color = 0;

            switch(changeState){
            case 0:
                iconswColor.enabled = false;
                break;
            case 1:
                iconshColor.enabled = false;
                break;
            case 2:
                iconbwColor.enabled = false;
                break;
            }
        }
        else if (color > 10){
            color = 10;
        }

        return color;
    }
    
    public void addRed(int amount){
        if(red == 0)
            iconswColor.enabled = true;
        red += amount;
        red = check(red);
        switch(changeState){
            case 0:
                bar1 = red;
                break;
            case 1:
                bar3 = red;
                break;
            case 2:
                bar2 = red;
                break;
        }

    }
    public void addBlue(int amount){
        if(blue == 0)
            iconshColor.enabled = true;
        blue += amount;
        blue = check(blue);
        switch(changeState){
            case 0:
                bar2 = blue;
                break;
            case 1:
                bar1 = blue;
                break;
            case 2:
                bar3 = blue;
                break;
        }
    }
    public void addYellow(int amount){
        if(yellow == 0)
            iconbwColor.enabled = true;
        yellow += amount;
        yellow = check(yellow);
        switch(changeState){
            case 0:
                bar3 = yellow;
                break;
            case 1:
                bar2 = yellow;
                break;
            case 2:
                bar1 = yellow;
                break;
        }
    }

   public void lessAmmo(){
        bar1--;
        if (bar1 >= 0){
            switch(changeState){
                case 0:
                    red--;
                    break;
                case 1:
                    blue--;
                    break;
                case 2:
                    yellow--;
                    break;
            }
        }
        bar1 = check(bar1);
   }

   public void colorSwapFoward(bool foward){
        iconChange.Play();
        switch(changeState){
            case 0:
                if (foward)
                    newChanges(1, blue, yellow, red, 1, 5, 3, foward, 9, 7, 11, 12, 13, 20, 21, 16, 17);
                else   
                    newChanges(2, yellow, red, blue, 2, 3, 4, foward, 9, 10, 8, 12, 13, 14, 15, 22, 23);
                break;
            case 1:
                if (foward)
                    newChanges(2, yellow, red, blue, 2, 3, 4, foward, 9, 10, 8, 12, 13, 14, 15, 22, 23);
                else   
                    newChanges(0, red, blue, yellow, 0, 4, 5, foward, 6, 10, 11, 18, 19, 14, 15, 16, 17);
                break;
            case 2:
                if (foward)
                    newChanges(0, red, blue, yellow, 0, 4, 5, foward, 6, 10, 11, 18, 19, 14, 15, 16, 17);
                else   
                    newChanges(1, blue, yellow, red, 1, 5, 3, foward, 9, 7, 11, 12, 13, 20, 21, 16, 17);
                break;
        }
   }

   void newChanges(int change, int b1color, int b2color, int b3color, int f, int m, int l, bool foward, int iconImage1, int iconImage2, int iconImage3, int sw, int swcolor, int sh, int shcolor, int bw, int bwcolor)
   {
        changeState = change;
        bar1 = b1color;
        bar2 = b2color;
        bar3 = b3color;
        first = spriteArray[f];
        middle = spriteArray[m];
        last = spriteArray[l];
        icon1.sprite = spriteArray[iconImage1];
        icon2.sprite = spriteArray[iconImage2];
        icon3.sprite = spriteArray[iconImage3];
        iconsw.sprite = spriteArray[sw];
        iconsh.sprite = spriteArray[sh];
        iconbw.sprite = spriteArray[bw];
        iconswColor.sprite = spriteArray[swcolor];
        iconshColor.sprite = spriteArray[shcolor];
        iconbwColor.sprite = spriteArray[bwcolor];
        ChangeSprite(first, middle, last, foward);
   }
   
void ChangeSprite(Sprite first, Sprite middle, Sprite last, bool foward)
    {
        
        
        if(foward)
        {
            (b1count, b2count) = (b2count, b1count);
            (bar1pic.sprite, bar3pic.sprite) = (bar3pic.sprite, bar1pic.sprite);
        }
        else
        {
            (b1count, b3count) = (b3count, b1count);
            (bar2pic.sprite, bar3pic.sprite) = (bar3pic.sprite, bar2pic.sprite);
        }
        (b2count, b3count) = (b3count, b2count);
        (bar1pic.sprite, bar2pic.sprite) = (bar2pic.sprite, bar1pic.sprite);


        for(int i = 0; i < 10; i++){
            b1.GetChild(i).GetComponent<Image>().sprite = first;
            b2.GetChild(i).GetComponent<Image>().sprite = middle;
            b3.GetChild(i).GetComponent<Image>().sprite = last;

            if(i < b1count)
                b1.GetChild(i).gameObject.SetActive(true);
            else if (i >= b1count)
                b1.GetChild(i).gameObject.SetActive(false);
            if(i < b2count)
                b2.GetChild(i).gameObject.SetActive(true);
            else if (i >= b2count)
                b2.GetChild(i).gameObject.SetActive(false);
            if(i < b3count)
                b3.GetChild(i).gameObject.SetActive(true);
            else if (i >= b3count)
                b3.GetChild(i).gameObject.SetActive(false);
        }
    }
}
