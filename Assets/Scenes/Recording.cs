using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Threading;

public class Recording : MonoBehaviour
{
    [SerializeField] private GameObject[] pianoKeys;
    public ArrayList recordedKeys = new ArrayList();
    [SerializeField] private bool recording = false;
    private float time, delay;
    private string key;

    public void startRecording()
    {
        recording = true;
    }
    public void stopRecording()
    {
        recording = false;
    }
    public bool isRecording()
    {
        return recording;
    }
    public void resetRecording()
    {
        recordedKeys.Clear();
    }
    public void play()
    {
        playRecording();
        resetRecording();
    }
    public void addToRecording(string name)
    {
        recordedKeys.Add(string.Format("{0}|{1}", name, Time.time));
    }
    private void playSound(string soundName, float delay)
    {
        Debug.Log(soundName + " --- " + string.Format("{0}",delay));
    }
    public void playKey(string keyNo)
    {
        Debug.Log(keyNo);
    }
    private (string, float) parseElement(string element)
    {
        string[] values = element.Split("|"); 
        return (values[0],float.Parse(values[1]));
    }
    public void playRecording()
    {
        if (recordedKeys.Count <= 0)
            return;
        stopRecording();

        StopAllCoroutines();
        StartCoroutine(playRecordingCoroutine(recordedKeys));
        
        
        
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