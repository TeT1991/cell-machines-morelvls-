using UnityEngine;
using UnityEngine.UI;

public class MenuLevelPagesSwiper : MonoBehaviour
{
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private GameObject[] pages;

    private int _currentPage;

    public void NextPage()
    {
        if (_currentPage == pages.Length - 1) return;

        pages[_currentPage].SetActive(false);
        _currentPage += 1;
        pages[_currentPage].SetActive(true);

        if (_currentPage == pages.Length - 1) nextButton.interactable = false;
    }

    public void PreviousPage()
    {
        if (_currentPage == 0) return;

        pages[_currentPage].SetActive(false);
        _currentPage -= 1;
        pages[_currentPage].SetActive(true);

        if (_currentPage == 0) previousButton.interactable = false;
    }
}
