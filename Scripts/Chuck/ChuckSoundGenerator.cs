using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimbreSelect
{
    brass, voice, clarinet
}

public class ChuckSoundGenerator : MonoBehaviour {

    public TimbreSelect timbreSelect;

    public Data dataPoints;

    private float freq;

    private GameObject mainCamera;

    private GameObject CasA;

    private float minDensityRange, maxDensityRange, freqRangeMin, freqRangeMax;

    public bool testing;

    // Use this for initialization
    void Start () {

        // getting the headfollow
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (testing) {
            minDensityRange = -20000f;
            maxDensityRange = 20000f;
        }

        //CasA = GameObject.FindGameObjectWithTag("SuperNova");


        if (timbreSelect == TimbreSelect.brass)
        {
            freqRangeMin = 261.63f;
            freqRangeMax = 1046.50f;

            GetComponent<ChuckSubInstance>().RunCode(@"
            fun void playImpact( float freq )
            {
                // connecting to dac with osc type
                SinOsc foo => Envelope e => dac;

                // lowering the gain
                .5 => foo.gain;
                
                // setting the envelope time
                500::ms => dur t => e.duration;

                // running the envelope with the frequency
                freq => foo.freq;
                e.keyOn();
                1500::ms => now;
                e.keyOff();
                500::ms => now;
            }
   
            global float impactFreq;
            global Event impactHappened;

            while( true )
                {
                    impactHappened => now;
                    spork ~ playImpact( impactFreq );
                }
        ");

        } else if(timbreSelect == TimbreSelect.voice)
        {
            freqRangeMin = 329.63f;
            freqRangeMax = 1318.51f;

            GetComponent<ChuckSubInstance>().RunCode(@"
            fun void playImpact( float freq )
            {
                // connecting to dac with osc type
                Saxofony foo => JCRev r => Envelope e => dac;

                // lowering the gain
                .5 => r.gain;
                .2 => r.mix;


                // setting params
                .8 => foo.stiffness;
                .4 => foo.aperture;
                .5 => foo.noiseGain;
                .7 => foo.blowPosition;
                4 => foo.vibratoFreq;
                .8 => foo.vibratoGain;
                .5 => foo.pressure;


                // setting the envelope time
                500::ms => dur t => e.duration;

                // running the envelope with the frequency
                freq => foo.freq;
                500::ms => now;
                1 => foo.noteOn;
                e.keyOn();
                1000::ms => now;
                e.keyOff();
                1 => foo.noteOff;
                500::ms => now;
            }
   
            global float impactFreq;
            global Event impactHappened;

            while( true )
                {
                    impactHappened => now;
                    spork ~ playImpact( impactFreq );
                }
        ");

        } else
        {
            freqRangeMin = 392.00f;
            freqRangeMax = 783.99f;

            GetComponent<ChuckSubInstance>().RunCode(@"
            fun void playImpact( float freq )
            {
                // connecting to dac with osc type
                PulseOsc foo => Envelope e => dac;

                // lowering the gain
                .5 => foo.gain;
                
                // setting the envelope time
                500::ms => dur t => e.duration;

                // running the envelope with the frequency
                freq => foo.freq;
                1000::ms => now;
                e.keyOn();
                500::ms => now;
                e.keyOff();
                500::ms => now;
            }
   
            global float impactFreq;
            global Event impactHappened;

            while( true )
                {
                    impactHappened => now;
                    spork ~ playImpact( impactFreq );
                }      
        ");

        }

        StartCoroutine("StartBang");
    }

    /**
    * a function to scale the abs position as you get further away from the origin
    **/
    private float AbsWeightFunction(float value) {
        return value * 2f;
    }

    private float GetFreq()
    {
        float returnFloat;
        Vector3 curPos;

        curPos = mainCamera.transform.position;

        Vector3 roundedPos = new Vector3(Mathf.Clamp(Mathf.RoundToInt(AbsWeightFunction(curPos.x)), -50, 50),
            Mathf.Clamp(Mathf.RoundToInt(AbsWeightFunction(curPos.y)), -50, 50),
            Mathf.Clamp(Mathf.RoundToInt(AbsWeightFunction(curPos.z)), -50, 50));
        Debug.Log(roundedPos);
        if (dataPoints.CheckPoint(roundedPos))
        {
            Debug.Log("Found A Point!");
            returnFloat = Scale(dataPoints.GetPoint(roundedPos), -20000f, 20000f, freqRangeMin, freqRangeMax);
        } else
        {
            Debug.Log("Doesnt exist :(");
            returnFloat = freqRangeMin;
        }

        return returnFloat;
    }


    // returns the density value in the range of the pitch capacity, -3 to 3.
    /**
     * @param: density- the input density value
     * @param: minRange- the minimum density value to satisfy being in the note range
     * @param: maxRange- the max density value that the point can be to be in the note range
     * @param: minScale- the minimum for what the scale will be.
     * @param: maxScale- the maximum for what the scale will be.
     * @return: the value of the density scaled to the minScale and maxScale
     **/
    private float Scale(float density, float minRange, float maxRange, float minScale, float maxScale)
    {
        return (((maxScale - minScale) * (density - minRange)) / (maxRange - minRange)) + minScale;
    }




    private IEnumerator StartBang()
    {
        while (true)
        {
            freq = GetFreq();
            GetComponent<ChuckSubInstance>().SetFloat("impactFreq", freq);
            GetComponent<ChuckSubInstance>().BroadcastEvent("impactHappened");
            yield return new WaitForSeconds(3f);
            Debug.Log("Making Bang!!!");
            yield return null;
        }
    }
}
