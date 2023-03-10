using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Threading;
using Unity.VisualScripting;

public class Recording : MonoBehaviour
{
    [SerializeField] private GameObject[] pianoKeys;
    public ArrayList recordedKeys = new ArrayList();
    [SerializeField] private bool recording = false;
    [SerializeField] private AudioClip[] pianoSounds;
    private AudioSource audioSource;
    private float time, delay;
    private string key;
    public bool turnedOn = false;
    [SerializeField] private GameObject rick, smallKeys;
    [SerializeField] private GameObject onLight, offLight, recordingOn, recordingOff;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    
    public void changeState()
    {
        turnedOn = !turnedOn;
        onLight.SetActive(!onLight.activeSelf);
        offLight.SetActive(!offLight.activeSelf);
    }

    public void startRecording()
    {
        if (turnedOn)
        {
            recordingOn.SetActive(true);
            recordingOff.SetActive(false);
            recording = true;
        }
    }
    public void stopRecording()
    {
        if (turnedOn)
        {
            if (recordingOn.activeSelf == false) 
            {
                StopAllCoroutines();
                resetRecording();
            }
            recordingOn.SetActive(false);
            recordingOff.SetActive(true);
            recording = false;
        }
    }
    public bool isRecording()
    {
        return recording;
    }
    public void resetRecording()
    {
        if (turnedOn) recordedKeys.Clear();
    }
    public void play()
    {
        if (turnedOn)
        {
            playRecording();
            resetRecording();
        }
    }
    public void addToRecording(string name)
    {
        if (turnedOn)
            recordedKeys.Add(string.Format("{0}|{1}", name, Time.time));
    }
    private void playSound(string soundName, float delay)
    {
        //Debug.Log(soundName + " --- " + string.Format("{0}",delay));
        if (turnedOn)
            playKey(soundName);
    }
    public void playKey(string keyNo)
    {
        if (turnedOn)
            audioSource.PlayOneShot(pianoSounds[int.Parse(keyNo) - 1]);
        //int index = int.Parse(keyNo) - 1;
        //Debug.Log(keyNo);
        //audioSource.PlayOneShot(pianoSounds[0]);

    }
    private (string, float) parseElement(string element)
    {
        string[] values = element.Split("|");
        return (values[0],float.Parse(values[1]));
    }
    public void playRecording()
    {
        if (turnedOn)
        {
            if (recordedKeys.Count <= 0)
            {
                if (rick)
                {
                    rick.gameObject.SetActive(true);
                    smallKeys.SetActive(false);
                }
                return;
            }

            string a = "";

            foreach (var thing in recordedKeys)
            {
                a = a + "   " + thing;
            }

            Debug.Log(a);

            StopAllCoroutines();
            StartCoroutine(playRecordingCoroutine(recordedKeys));
        }

        ///
        /// //has to happen for first element to have 0 delay
        /// /*(key, time) = parseElement(recordedKeys[0].ToString());
        ///playSound(key, 0);
        ///recordedKeys.RemoveAt(0);
        ///
        ///
        ///while (recordedKeys.Count > 0)
        ///{
        ///    delay = time;
        ///    (key, time) = parseElement(recordedKeys[0].ToString());
        ///    delay = time - delay;
        ///    timer = delay;
        ///
        ///    while (timer > 0)
        ///    {
        ///        timer -= Time.deltaTime;
        ///        //Debug.Log(timer);
        ///    }
        ///    //Debug.LogError("---");
        ///    playSound(key, delay);
        ///    recordedKeys.RemoveAt(0);
        ///}*/
        ///
    }
    /*public async void playRecordingAsync()
    {
        if (recordedKeys.Count <= 0)
            return;

        stopRecording();
        (key, time) = parseElement(recordedKeys[0].ToString());
        playSound(key, 0);
        recordedKeys.RemoveAt(0);

        while (recordedKeys.Count > 0)
        {
            delay = time;
            (key, time) = parseElement(recordedKeys[0].ToString());
            delay = time - delay;
            timer = delay;

            canContinue = false;
            StartCoroutine(addDelay(timer));
            while (canContinue == false)
            {}
            playSound(key, delay);
            recordedKeys.RemoveAt(0);
        }
    }*/

    public void destroyRick()
    {
        Destroy(rick);
    }

    private IEnumerator playRecordingCoroutine(ArrayList recKeys)
    {
        //has to happen for first element to have 0 delay
        (key, time) = parseElement(recKeys[0].ToString());
        playSound(key, 0);
        recKeys.RemoveAt(0);
        while (recKeys.Count > 0)
        {
            delay = time;
            (key, time) = parseElement(recKeys[0].ToString());
            delay = time - delay;

            yield return StartCoroutine(addDelay(delay));

            playSound(key, delay);

            recKeys.RemoveAt(0);
            yield return null;
        }

        yield return null;
    }
    private IEnumerator addDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
