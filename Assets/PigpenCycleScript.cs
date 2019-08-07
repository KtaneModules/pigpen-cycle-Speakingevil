using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PigpenCycleScript : MonoBehaviour
{
    public KMAudio Audio;
    public List<KMSelectable> keys;
    public GameObject[] dials;
    public TextMesh[] dialText;
    public TextMesh disp;

    private string[] message = new string[100] { "ADVANCED", "ADDITION", "ALLOTYPE", "ALLOTTED", "BINARIES", "BILLIONS", "BULLHORN", "BULWARKS", "CIPHERED", "CIRCUITS", "COMMANDO", "COMPILER", "DECRYPTS", "DIVISION", "DISPATCH", "DISCRETE", "ENCIPHER", "ENTRANCE", "EQUATORS", "EQUALISE", "FINISHED", "FINDINGS", "FORMULAE", "FORTUNES", "GAUNTLET", "GAMBLING", "GATEPOST", "GATEWAYS", "HAZARDED", "HAZINESS", "HUNTRESS", "HUNGRIER", "INDICATE", "INDIGOES", "ILLUSORY", "ILLUDING", "JIGSAWED", "JIMMYING", "JUNKYARD", "JUNCTURE", "KILOWATT", "KINETICS", "KNOCKING", "KNOWABLE", "LIMITING", "LINEARLY", "LINKWORK", "LINGERED", "MONOGRAM", "MONOTONE", "MULTITON", "MULCTING", "NANOGRAM", "NANOTUBE", "NUMEROUS", "NUMERATE", "OCTANGLE", "OCTUPLES", "OBSTRUCT", "OBSTACLE", "PROGRESS", "PROJECTS", "POSTSYNC", "POSITRON", "QUADRANT", "QUADRICS", "QUIRKISH", "QUITTERS", "REVERSED", "REVOLVED", "ROTATORS", "RELATIVE", "STARTING", "STANDARD", "STOCKADE", "STOCCATA", "TRIGGERS", "TRIANGLE", "TOMOGRAM", "TOMAHAWK", "UNDERRUN", "UNDERLIE", "ULTERIOR", "ULTRAHOT", "VICINITY", "VICELESS", "VOLITION", "VOLUMING", "WINGDING", "WINNABLE", "WHATNESS", "WHATSITS", "YELLOWED", "YEASAYER", "YOKOZUNA", "YOURSELF", "ZIPPERED", "ZIGZAGGY", "ZYMOLOGY", "ZYMOGENE" };
    private string[] response = new string[100] { "ENTRANCE", "KILOWATT", "YOKOZUNA", "STOCCATA", "POSITRON", "TRIGGERS", "EQUALISE", "VICELESS", "FORMULAE", "BINARIES", "POSTSYNC", "TOMAHAWK", "HAZINESS", "VOLITION", "KNOCKING", "ZIGZAGGY", "FINDINGS", "ROTATORS", "KINETICS", "ILLUSORY", "RELATIVE", "DISPATCH", "STANDARD", "DIVISION", "PROGRESS", "OBSTACLE", "JIMMYING", "ULTERIOR", "DISCRETE", "BULWARKS", "GATEWAYS", "FINISHED", "JUNKYARD", "WHATSITS", "JIGSAWED", "PROJECTS", "OCTUPLES", "NUMEROUS", "BILLIONS", "UNDERLIE", "JUNCTURE", "EQUATORS", "NUMERATE", "YOURSELF", "TRIANGLE", "UNDERRUN", "REVOLVED", "GATEPOST", "QUIRKISH", "QUITTERS", "LIMITING", "MONOGRAM", "WHATNESS", "ZIPPERED", "LINGERED", "ENCIPHER", "REVERSED", "MULTITON", "COMPILER", "MULCTING", "ZYMOGENE", "STARTING", "VOLUMING", "TOMOGRAM", "HAZARDED", "ALLOTYPE", "GAMBLING", "QUADRICS", "OBSTRUCT", "BULLHORN", "ZYMOLOGY", "NANOTUBE", "INDICATE", "CIPHERED", "OCTANGLE", "STOCKADE", "DECRYPTS", "ADDITION", "COMMANDO", "HUNTRESS", "WINGDING", "MONOTONE", "KNOWABLE", "VICINITY", "ILLUDING", "CIRCUITS", "GAUNTLET", "WINNABLE", "YEASAYER", "FORTUNES", "INDIGOES", "NANOGRAM", "LINKWORK", "QUADRANT", "HUNGRIER", "LINEARLY", "ADVANCED", "ALLOTTED", "YELLOWED", "ULTRAHOT" };
    private string[] pigpens = new string[4] { "ASCUIVGT", "BKFOHQDM", "JWLYRZPX", "EN"};
    private string[] ciphertext = new string[2];
    private string answer;
    private int[][] rot = new int[2][] { new int[8], new int[8] };
    private int pressCount;
    private bool moduleSolved;

    //Logging
    static int moduleCounter = 1;
    int moduleID;

    private void Awake()
    {
        moduleID = moduleCounter++;
        foreach (KMSelectable key in keys)
        {
            int k = keys.IndexOf(key);
            key.OnInteract += delegate () { KeyPress(k); return false; };
        }
    }

    void Start()
    {
        Reset();
    }

    private void KeyPress(int k)
    {
        keys[k].AddInteractionPunch(0.125f);
        if (moduleSolved == false)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (k == 26)
            {
                pressCount = 0;
                answer = string.Empty;
            }
            else
            {
                pressCount++;
                answer = answer + "QWERTYUIOPASDFGHJKLZXCVBNM"[k];
            }
            disp.text = answer;
            if (pressCount == 8)
            {
                if (answer == ciphertext[1])
                {
                    moduleSolved = true;
                    Audio.PlaySoundAtTransform("InputCorrect", transform);
                    disp.color = new Color32(0, 255, 0, 255);
                }
                else
                {
                    GetComponent<KMBombModule>().HandleStrike();
                    disp.color = new Color32(255, 0, 0, 255);
                    Debug.LogFormat("[Pigpen Cycle #{0}]The submitted response was {1}: Resetting", moduleID, answer);
                }
                Reset();
            }
        }
    }

    private void Reset()
    {

        StopAllCoroutines();
        if (moduleSolved == false)
        {
            pressCount = 0;
            answer = string.Empty;
            int r = Random.Range(0, 100);
            string[] roh = new string[8];
            List<string>[] ciph = new List<string>[] { new List<string> { }, new List<string> { } };
            for (int i = 0; i < 8; i++)
            {
                dialText[i].text = string.Empty;
                rot[1][i] = rot[0][i];
                rot[0][i] = Random.Range(0, 8);
                roh[i] = rot[0][i].ToString();
                for(int j = 0; j < 4; j++)
                {
                    if (pigpens[j].Contains(message[r][i].ToString()))
                    {
                        if(j == 3)
                        {
                            ciph[0].Add(pigpens[3][(pigpens[3].IndexOf(message[r][i]) + rot[0][i]) % 2].ToString());
                        }
                        else
                        {
                            ciph[0].Add(pigpens[j % 3][(pigpens[j].IndexOf(message[r][i]) + rot[0][i]) % 8].ToString());
                        }
                    }
                    if (pigpens[j].Contains(response[r][i].ToString()))
                    {
                        if (j == 3)
                        {
                            ciph[1].Add(pigpens[3][(pigpens[3].IndexOf(response[r][i]) + rot[0][i]) % 2].ToString());
                        }
                        else
                        {
                            ciph[1].Add(pigpens[j % 3][(pigpens[j].IndexOf(response[r][i]) + rot[0][i]) % 8].ToString());
                        }
                    }
                }              
            }
            ciphertext[0] = string.Join(string.Empty, ciph[0].ToArray());
            ciphertext[1] = string.Join(string.Empty, ciph[1].ToArray());
            Debug.LogFormat("[Pigpen Cycle #{0}]The encrypted message was {1}", moduleID, ciphertext[0]);
            Debug.LogFormat("[Pigpen Cycle #{0}]The dial rotations were {1}", moduleID, string.Join(", ", roh));
            Debug.LogFormat("[Pigpen Cycle #{0}]The deciphered message was {1}", moduleID, message[r]);
            Debug.LogFormat("[Pigpen Cycle #{0}]The response word was {1}", moduleID, response[r]);
            Debug.LogFormat("[Pigpen Cycle #{0}]The correct response was {1}", moduleID, ciphertext[1]);
        }
        StartCoroutine(DialSet());
    }

    private IEnumerator DialSet()
    {
        int[] spin = new int[8];
        bool[] set = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            if (moduleSolved == false)
            {
                spin[i] = rot[0][i] - rot[1][i];
            }
            else
            {
                spin[i] = -rot[0][i];
            }
            if (spin[i] < 0)
            {
                spin[i] += 8;
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (spin[j] == 0)
                {
                    if (set[j] == false)
                    {
                        set[j] = true;
                        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);
                        if (moduleSolved == false)
                        {
                            dialText[j].text = ciphertext[0][j].ToString();
                        }
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                    dialText[j].text = "W";
                                    break;
                                case 2:
                                case 3:
                                    dialText[j].text = "L";
                                    break;
                                case 4:
                                    dialText[j].text = "D";
                                    break;
                                case 5:
                                    dialText[j].text = "O";
                                    break;
                                case 6:
                                    dialText[j].text = "N";
                                    break;
                                default:
                                    dialText[j].text = "E";
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    dials[j].transform.localEulerAngles += new Vector3(0, 0, 45);
                    spin[j]--;
                }
            }
            if (i < 3)
            {
                yield return new WaitForSeconds(0.25f);
            }
        }
        if (moduleSolved == true)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            GetComponent<KMBombModule>().HandlePass();
        }
        disp.text = string.Empty;
        disp.color = new Color32(255, 255, 255, 255);
        yield return null;
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "!{0} QWERTYUI [Inputs letters] | !{0} cancel [Deletes inputs]";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.ToLowerInvariant() == "cancel")
        {
            KeyPress(26);
            yield return null;
        }
        else
        {
            command = command.ToUpperInvariant();
            var word = Regex.Match(command, @"^\s*([A-Z\-]+)\s*$");
            if (!word.Success)
            {
                yield break;
            }
            command = command.Replace(" ", string.Empty);
            foreach (char letter in command)
            {
                KeyPress("QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(letter));
                yield return new WaitForSeconds(0.125f);
            }
            yield return null;
        }
    }
}
