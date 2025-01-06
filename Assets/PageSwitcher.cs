using TMPro;
using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] Pages;
    [SerializeField] TMP_Text PageLabel; 

    private int _currentPage = 0;
    public int currentPage
    {
        get
        {
            return _currentPage;
        }
        set
        {
            if(value < 0)
            {
                _currentPage = Pages.Length - 1;
            }
            else if (value >= Pages.Length)
            {
                _currentPage = 0;
            }
            else 
            {
                _currentPage = value;
            }
        }
    }

    void Start()
    {
        for(int i = 0; i < Pages.Length; i++)
        {
            Pages[i].SetActive(i == currentPage);
        }
        UpdatePageLabel();
    }

    public void NextPage()
    {
        Pages[currentPage].SetActive(false);
        currentPage++;
        Pages[currentPage].SetActive(true);
        UpdatePageLabel();
    }

    public void PreviousPage()
    {
        Pages[currentPage].SetActive(false);
        currentPage--;
        Pages[currentPage].SetActive(true);
        UpdatePageLabel();
    }

    private void UpdatePageLabel()
    {
        PageLabel.text = $"{currentPage + 1}/{Pages.Length}";
    }


}
