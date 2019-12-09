using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Sprite[] Presentation;
    public GameObject SlidePrefab;

    // TODO: Set private
    public int _currentSlideOffset = 0;

    // The order of the List is reversed compared to Presentation (for priority in canvas)
    private List<SlideController> _slides = new List<SlideController>();

    void Awake()
    {
        // Validate settings
        if (Presentation.Length == 0) throw new System.Exception("CanvasController: No slides loaded");
        if (!SlidePrefab) throw new System.Exception("CanvasController: No reference to \"Slide.prefab\"");

        // Instantiate Slides
        for (int i=0; i < Presentation.Length; i++)
        {
            GameObject slide = Instantiate(SlidePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            slide.transform.SetParent(this.transform, false);
            SlideController slideController = slide.GetComponent<SlideController>();
            slideController.LoadSprite(Presentation[Presentation.Length - i - 1]);
            _slides.Add(slideController);
        }
    }

    void Update()
    {
        // Next slide: Left mouse button OR Right arrow
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
            NextSlide();

        // Previous slide: Right mouse button OR Left arrow
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousSlide();
    }

    private void NextSlide()
    {
        // Keep slides in bounds
        if (_currentSlideOffset <= 0) return;

        // Hide current slide
        _slides[_currentSlideOffset].Hide();

        // Move to next slide
        _currentSlideOffset--;
    }

    private void PreviousSlide()
    {
        // Keep slides in bounds
        if (_currentSlideOffset + 1 >= Presentation.Length) return;

        // Move to previous slide
        _currentSlideOffset++;

        // Show previous slide
        _slides[_currentSlideOffset].Show();
    }
}
