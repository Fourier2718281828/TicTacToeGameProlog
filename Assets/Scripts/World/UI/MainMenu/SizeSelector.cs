using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SizeSelector : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField widthInputField;
    [SerializeField]
    private TMP_InputField heightInputField;

    private void Start()
    {
        widthInputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
        heightInputField.characterValidation = TMP_InputField.CharacterValidation.CustomValidator;
        widthInputField.inputValidator = new MyInputValidator();
        heightInputField.inputValidator = new MyInputValidator();
    }
    public void StartGame()
    {
        if(!string.IsNullOrEmpty(widthInputField.text) && !string.IsNullOrEmpty(heightInputField.text) && int.Parse(widthInputField.text) > 2 && int.Parse(heightInputField.text) > 2)
        {
            PlayerPrefs.SetInt("cols", int.Parse(widthInputField.text));
            PlayerPrefs.SetInt("rows", int.Parse(heightInputField.text));
            SceneManager.LoadScene("SampleScene");
        }
    }

    private class MyInputValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (ch >= '0' && ch <= '9' && ValidateNumber(text + ch))
            {
                text += ch;
                pos += 1;
                return ch;
            }
            return (char)0;
        }

        private bool ValidateNumber(string num)
        {
            return num.Length < 2 || (num.Length == 2 && num[0] == '1') || (num.Length == 2 && num[0] == '2' && num[1] == '0');
        }
    }
}
