using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class TestScenePassingSrc : MonoBehaviour
    {
        #region Inspector

        

        #endregion

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void OnClicked()
        {
            Context.Find(this).Get<SceneParametersBus>().Set("passed successfully!");
            SceneManager.LoadSceneAsync("InjectionTestPassDst");
        }
    }
}