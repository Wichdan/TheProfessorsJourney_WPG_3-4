using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dlgReference;
    [SerializeField] Dialogue messageDialogue;
    [SerializeField] GameObject dialoguePanel, nameHolder, endDialogueIcon, skipDialogue;
    [SerializeField] Image dlgBG, dialogueHolderBG;
    [SerializeField] int index;
    [SerializeField] float textSpeed;
    [SerializeField] TextMeshProUGUI nameText, sentenceText;
    [SerializeField] Image leftPortrait, rightPortrait;
    [SerializeField] private TMP_Text m_textMeshPro;
    [SerializeField] bool isStartWhenPlay = false, isStartDialogue;
    Player player;

    #region Singleton
    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (isStartWhenPlay)
        {
            StartDialogue();
            player.isInteracting = true;
        }

        if (MusicManager.instance != null)
            MusicManager.instance.SetAndPlayBGM(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.isInteracting && !player.isInteract && isStartDialogue)
            NextDialogue();
    }

    public void SetDialogue(Dialogue dialogue) => dlgReference = dialogue;

    public void StartDialogue()
    {
        isStartDialogue = true;
        player.isInteracting = true;
        dialoguePanel.SetActive(true);
        StopAllCoroutines();

        if (index >= dlgReference.conversation.Count)
        {
            index = 0;
            player.isInteracting = false;
            dialoguePanel.SetActive(false);
            player.isCanInteract = false;
            isStartDialogue = false;

            if (dlgReference.isGetSomething)
            {
                MissionManager.instance.AddNPCCount(dlgReference.getSomething.updateMission);
            }
            //dialogueBG.SetActive(false);
            return;
        }

        nameText.text = dlgReference.conversation[index].charName;
        sentenceText.text = dlgReference.conversation[index].sentence;

        ShowLeftPortrait();
        ShowRightPortrait();
        ShowNarrator();
        ShowDialogueHolder(dlgReference.conversation[index].dontUseHolderBG);
        ShowDialogueBackground();

        bool showName = dlgReference.conversation[index].charName != "";
        nameHolder.SetActive(showName);

        skipDialogue.SetActive(dlgReference.isCanSkip);

        StartCoroutine(DialogueAnimation());
    }

    void ShowLeftPortrait()
    {
        if (dlgReference.conversation[index].leftPortrait != null)
        {
            leftPortrait.enabled = true;
            leftPortrait.sprite = dlgReference.conversation[index].leftPortrait;
        }
        else
            leftPortrait.enabled = false;
    }

    void ShowRightPortrait()
    {
        if (dlgReference.conversation[index].rightPortrait != null)
        {
            rightPortrait.enabled = true;
            rightPortrait.sprite = dlgReference.conversation[index].rightPortrait;
        }
        else
            rightPortrait.enabled = false;
    }

    void ShowNarrator()
    {
        if (dlgReference.conversation[index].isNarrator)
        {
            rightPortrait.enabled = false;
            leftPortrait.enabled = false;
        }
    }

    void ShowDialogueHolder(bool show) => dialogueHolderBG.enabled = !show;
    void ShowDialogueBackground()
    {
        if (dlgReference.conversation[index].dlgBGIndex >= 0 && dlgReference.dialogueBackgrounds != null)
        {
            dlgBG.enabled = true;
            dlgBG.sprite = dlgReference.dialogueBackgrounds[dlgReference.conversation[index].dlgBGIndex];
        }
        else
            dlgBG.enabled = false;
    }

    /*
    public void ShowMessageDialogue(string getSomething)
    {
        player.isInteracting = true;
        dialoguePanel.SetActive(true);
        StopAllCoroutines();

        if (index >= dlgReference.conversation.Count)
        {
            index = 0;
            player.isInteracting = false;
            dialoguePanel.SetActive(false);
            player.isCanInteract = false;

            if (dlgReference.isGetSomething)
            {
                MissionManager.instance.AddNPCCount(dlgReference.getSomething.updateMission);
            }
            //dialogueBG.SetActive(false);
            return;
        }

        nameText.text = messageDialogue.conversation[index].charName;
        sentenceText.text = messageDialogue.conversation[index].sentence + getSomething;

        ShowLeftPortrait();
        ShowRightPortrait();
        ShowNarrator();
        ShowDialogueHolder(dlgReference.conversation[index].dontUseHolderBG);
        ShowDialogueBackground();

        bool showName = dlgReference.conversation[index].charName != "";
        nameHolder.SetActive(showName);

        skipDialogue.SetActive(dlgReference.isCanSkip);
        StartCoroutine(DialogueAnimation());
    }
*/
    void NextDialogue()
    {
        index++;
        StartDialogue();
    }

    public void SkipDialogue()
    {
        index = dlgReference.conversation.Count;
        StartDialogue();
    }

    IEnumerator DialogueAnimation()
    {
        // Force and update of the mesh to get valid information.
        m_textMeshPro.ForceMeshUpdate();

        int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int visibleCount = 0;
        endDialogueIcon.SetActive(false);

        while (counter <= totalVisibleCharacters)
        {
            visibleCount = counter % (totalVisibleCharacters + 1);

            m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait 1.0 second and start over.
            if (visibleCount >= totalVisibleCharacters)
            {
                endDialogueIcon.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                m_textMeshPro.text = dlgReference.conversation[index].sentence;
                //yield return new WaitForSeconds(1.0f);
            }

            counter += 1;

            yield return new WaitForSeconds(textSpeed);
        }
        //Debug.Log("Done revealing the text.");
    }
}
