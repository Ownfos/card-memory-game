using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardSelector : MonoBehaviour
{
    // Event that triggers whenever a valid card selection happens
    public event EventHandler<Card> OnCardSelect;

    // Click detection method (mouse / touch screen / replay)
    public IClickMethod clickMethod;

    // Sound effect to play when a card is selected
    [SerializeField] private AudioClip selectSound;

    private void Awake()
    {
        Initialize();
    }

    // Configure input method and enable sound effect on card select
    private void Initialize()
    {
        RegisterSoundEffectHandler();
        clickMethod = ChooseClickMethod();
    }

    // Decide whether we should use regular click method (mouse/touch)
    // or click simulator from ReplayManager.
    // If we selected regular click method, attach a handler so that
    // we can record click events happening from now on to ReplayManager.
    private IClickMethod ChooseClickMethod()
    {
        var replayManager = GameObject.FindGameObjectWithTag("ReplayManager").GetComponent<ReplayManager>();
        if (replayManager.IsReplayRunning)
        {
            // Use replay buffer as input simulator
            var replayBuffer = replayManager.GetClickSimulator();
            replayBuffer.StartSimulatingInput();

            return replayBuffer;
        }
        else
        {
            // Use default click method and start recording to ReplayManager
            replayManager.StartRecording();
            OnCardSelect += (sender, card) => replayManager.RecordClickEvent(new ClickEvent(clickMethod.GetClickScreenPosition()));
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.Log("touch");
                return new ClickByTouch();
            }
            else
            {
                Debug.Log("mouse");
                return new ClickByMouse();
            }
        }
    }

    // Find SoundEffectPlayer component and register event
    // so that sound effect gets played whenever a card is selected.
    private void RegisterSoundEffectHandler()
    {
        var soundPlayer = GameObject.FindGameObjectWithTag("SoundEffectPlayer").GetComponent<SoundEffectPlayer>();
        OnCardSelect += (sender, arg) => soundPlayer.Play(SoundEffect.CardSelect);
    }

    // Update is called once per frame
    void Update()
    {
        if (clickMethod.ClickHappened())
        {
            var card = GetSelectedCard(clickMethod.GetClickScreenPosition());
            if (card != null)
            {
                OnCardSelect.Invoke(this, card);
            }
        }
    }

    // Try to find a card instance under clicked screen position.
    // Returns null if nothing was clicked.
    private Card GetSelectedCard(Vector2 screenPosition)
    {
        var ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out var hitinfo))
        {
            var obj = hitinfo.collider.gameObject;
            if (obj.CompareTag("Card"))
            {
                return obj.GetComponent<Card>();
            }
        }

        return null;
    }
}
