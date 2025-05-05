using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{

    private int currentMenu = 0;
    private int currentStep = 0;
    //private bool[] stepChecks = new bool[99];
    [SerializeField] private GameObject buttons;
    [SerializeField] private checkClass[] allChecks;
    [SerializeField] private subMenu[] SubMenus;
    [System.Serializable]
    public class subMenu
    {
        public GameObject menuObject;
        public GameObject[] shownObjects;
        public int[] requiredChecks;
        public Color32 buttonUnlocked = new Color32(135, 148, 241, 255);
    }

    [System.Serializable]
    public class checkClass
    {
        public Toggle[] tickboxes;
        public bool complete;
    }

    void Awake()
    {
        for(int i = 0; i < allChecks.Length; i++)
        {
            allChecks[i].complete = false;
            allChecks[i].complete = false;
        }
    }

    void Start()
    {
        for(int i = 0; i < SubMenus.Length; i++)
        {
            for (int j = 0; j < SubMenus[i].shownObjects.Length; j++)
            {
                if (SubMenus[i].shownObjects[j].activeSelf)
                {
                    SubMenus[i].shownObjects[j].SetActive(false);
                    Debug.Log("Hidden " + SubMenus[i].shownObjects[j]);
                }
            }
        }
        SetMenu(0);
    }
    public void SetMenu(int menuID)
    {
        Debug.Log("Set menu to " + menuID);

        if(SubMenus.Length < menuID + 1)
        {
            Debug.Log("Invalid Menu id!! ID: " + menuID);
            SetMenu(0);
            return;
        }

        if (SubMenus[menuID].requiredChecks.Length >= 1)
        {
            foreach (int check in SubMenus[menuID].requiredChecks)
            {
                if (!allChecks[check].complete)
                {
                    Debug.Log("check id " + check + " is not true!");
                    return;
                }
            }
        }

        currentMenu = menuID;

        foreach (var menu in SubMenus)
        {
            if(menu.menuObject.activeSelf)
                menu.menuObject.SetActive(false);
        }

        SubMenus[currentMenu].menuObject.SetActive(true);

        foreach(GameObject obj in SubMenus[currentMenu].shownObjects)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                Debug.Log("Shown " + obj);
            }
        }

    }

    public void NextMenu()
    {
        Debug.Log("Total menus: " + SubMenus.Length + "  Current menu: " + currentMenu + "  Next menu: " + (currentMenu + 1));

        if(SubMenus.Length >= currentMenu + 1)
        SetMenu(currentMenu + 1);
    }

    public void SetCheck(int ID)
    {
        allChecks[ID].complete = true;

        foreach(Toggle toggle in allChecks[ID].tickboxes)
        {

            toggle.isOn = true;
        }

        Debug.Log("Set check " + ID + " to true");

        CheckMenus();

    }

    public bool GetCheck(int ID)
    {
        return allChecks[ID].complete;
    }

    private void CheckMenus()
    {
        for (int i = 0; i < SubMenus.Length && i < buttons.transform.childCount; i++)
        {
            //If the button is active by default, dont change the colour
            if (CheckMenu(i) && SubMenus[i].requiredChecks.Length > 0)
            {
                buttons.transform.GetChild(i).GetComponent<Image>().color = SubMenus[i].buttonUnlocked;
                Debug.Log("Unlocked menu ID " + i);
            }
        }
    }

    private bool CheckMenu(int ID)
    {
        for (int i = 0; i < SubMenus[ID].requiredChecks.Length; i++)
        {
            if (!allChecks[SubMenus[ID].requiredChecks[i]].complete)
            {
                return false;
            }
        }

        return true;
    }

}
