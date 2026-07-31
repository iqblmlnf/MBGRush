#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Events;
using UnityEngine.Events;

public class UIRebuilder : EditorWindow
{
    [MenuItem("Tools/MBGRush/Rebuild UI")]
    public static void RebuildUI()
    {
        // 1. Cari atau buat UIManager
        GameObject uiManagerObj = GameObject.Find("UIManager");
        if (uiManagerObj == null)
        {
            uiManagerObj = new GameObject("UIManager");
        }
        
        GameUIManager gameUI = uiManagerObj.GetComponent<GameUIManager>();
        if (gameUI == null)
        {
            gameUI = uiManagerObj.AddComponent<GameUIManager>();
        }

        // 2. Hapus Game Canvas lama jika ada agar bersih
        GameObject oldCanvas = GameObject.Find("GameCanvas");
        if (oldCanvas != null)
        {
            DestroyImmediate(oldCanvas);
        }
        
        // Hapus Canvas lain yang mungkin bernama default "Canvas" untuk menghindari kebingungan
        GameObject oldCanvasDefault = GameObject.Find("Canvas");
        if (oldCanvasDefault != null)
        {
            DestroyImmediate(oldCanvasDefault);
        }

        // 3. Buat GameCanvas baru
        GameObject canvasObj = new GameObject("GameCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // Buat EventSystem jika belum ada di scene
        if (GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
        }

        // 4. BUAT HUD (Heads Up Display) di dalam Canvas
        // Set raycastTarget ke false agar panel transparan ini tidak memblokir klik layar game
        GameObject hudPanel = CreatePanel(canvasObj, "HUDPanel", new Color(0, 0, 0, 0), false);
        hudPanel.GetComponent<RectTransform>().anchorMin = Vector2.zero;
        hudPanel.GetComponent<RectTransform>().anchorMax = Vector2.one;
        hudPanel.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        hudPanel.GetComponent<RectTransform>().offsetMax = Vector2.zero;

        // Score Text
        GameObject scoreTextObj = CreateText(hudPanel, "ScoreText", "Score : 0", 36, Color.white, TextAlignmentOptions.Left);
        SetRect(scoreTextObj, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -20f), new Vector2(300f, 50f));
        scoreTextObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);

        // Distance Text
        GameObject distanceTextObj = CreateText(hudPanel, "DistanceText", "0 m", 30, Color.yellow, TextAlignmentOptions.Left);
        SetRect(distanceTextObj, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -75f), new Vector2(300f, 50f));
        distanceTextObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);

        // Fuel Slider
        GameObject sliderObj = CreateSlider(hudPanel, "FuelSlider");
        SetRect(sliderObj, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -140f), new Vector2(250f, 25f));
        sliderObj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);

        // Pause Button on HUD
        GameObject pauseBtnObj = CreateButton(hudPanel, "PauseButton", "||", 24, Color.white, Color.white);
        SetRect(pauseBtnObj, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-20f, -20f), new Vector2(60f, 60f));
        pauseBtnObj.GetComponent<RectTransform>().pivot = new Vector2(1f, 1f);
        
        // Coba load sprite ikon pause kustom jika ada di aset
        Sprite pauseSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/pause (1).png");
        if (pauseSprite != null)
        {
            pauseBtnObj.GetComponent<Image>().sprite = pauseSprite;
            pauseBtnObj.GetComponent<Image>().color = Color.white;
            
            // Hapus teks placeholder "||"
            Transform textTrans = pauseBtnObj.transform.Find("Text (TMP)");
            if (textTrans != null)
            {
                DestroyImmediate(textTrans.gameObject);
            }
        }
        
        // Hubungkan tombol pause secara permanen di Editor
        UnityEventTools.AddPersistentListener(pauseBtnObj.GetComponent<Button>().onClick, gameUI.TogglePause);

        // --- TOMBOL VIRTUAL INPUT MOBILE ---
        // Tombol Rem (Mundur/Kiri) di kiri bawah
        GameObject brakeBtnObj = CreateButton(hudPanel, "BrakeButton", "REM", 28, new Color(0.3f, 0.3f, 0.3f, 0.7f), Color.white);
        SetRect(brakeBtnObj, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(150f, 150f), new Vector2(180f, 120f));
        brakeBtnObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        VirtualButton brakeVB = brakeBtnObj.AddComponent<VirtualButton>();
        brakeVB.buttonType = "Brake";

        // Tombol Gas (Maju/Kanan) di kanan bawah
        GameObject gasBtnObj = CreateButton(hudPanel, "GasButton", "GAS", 28, new Color(0.2f, 0.6f, 0.2f, 0.8f), Color.white);
        SetRect(gasBtnObj, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-150f, 150f), new Vector2(180f, 120f));
        gasBtnObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        VirtualButton gasVB = gasBtnObj.AddComponent<VirtualButton>();
        gasVB.buttonType = "Gas";

        // 5. BUAT GAME OVER PANEL
        GameObject goPanel = CreatePanel(canvasObj, "GameOverPanel", new Color(0.12f, 0.12f, 0.12f, 0.9f));
        SetRect(goPanel, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero); // Full Screen
        
        GameObject goTitle = CreateText(goPanel, "TitleText", "GAME OVER!", 72, Color.red, TextAlignmentOptions.Center);
        SetRect(goTitle, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -150f), new Vector2(800f, 100f));
        
        GameObject goScoreText = CreateText(goPanel, "GameOverScoreText", "Score: 0", 36, Color.white, TextAlignmentOptions.Center);
        SetRect(goScoreText, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -250f), new Vector2(500f, 50f));

        GameObject goDistanceText = CreateText(goPanel, "GameOverDistanceText", "Distance: 0 m", 30, Color.yellow, TextAlignmentOptions.Center);
        SetRect(goDistanceText, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -300f), new Vector2(500f, 50f));

        GameObject goContainer = CreateContainer(goPanel, "ButtonContainer", new Vector2(300f, 140f), new Vector2(0f, -100f));
        GameObject retryBtn = CreateButton(goContainer, "RetryButton", "Retry", 28, new Color(0.15f, 0.45f, 0.15f, 1f), Color.white);
        GameObject menuBtnGO = CreateButton(goContainer, "MainMenuButton", "Menu Utama", 28, new Color(0.45f, 0.15f, 0.15f, 1f), Color.white);
        
        // Hubungkan tombol Game Over secara permanen di Editor
        UnityEventTools.AddPersistentListener(retryBtn.GetComponent<Button>().onClick, gameUI.RestartLevel);
        UnityEventTools.AddPersistentListener(menuBtnGO.GetComponent<Button>().onClick, gameUI.GoToMainMenu);

        // 6. BUAT VICTORY PANEL
        GameObject vicPanel = CreatePanel(canvasObj, "VictoryPanel", new Color(0.12f, 0.12f, 0.12f, 0.9f));
        SetRect(vicPanel, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero); // Full Screen
        
        GameObject vicTitle = CreateText(vicPanel, "TitleText", "MISI SELESAI!", 72, Color.green, TextAlignmentOptions.Center);
        SetRect(vicTitle, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -150f), new Vector2(800f, 100f));

        GameObject vicScoreText = CreateText(vicPanel, "VictoryScoreText", "Final Score: 0", 36, Color.white, TextAlignmentOptions.Center);
        SetRect(vicScoreText, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -250f), new Vector2(500f, 50f));

        GameObject vicContainer = CreateContainer(vicPanel, "ButtonContainer", new Vector2(300f, 140f), new Vector2(0f, -100f));
        GameObject playAgainBtn = CreateButton(vicContainer, "PlayAgainButton", "Play Again", 28, new Color(0.15f, 0.45f, 0.15f, 1f), Color.white);
        GameObject menuBtnVic = CreateButton(vicContainer, "MainMenuButton", "Menu Utama", 28, new Color(0.45f, 0.15f, 0.15f, 1f), Color.white);

        // Hubungkan tombol Victory secara permanen di Editor
        UnityEventTools.AddPersistentListener(playAgainBtn.GetComponent<Button>().onClick, gameUI.RestartLevel);
        UnityEventTools.AddPersistentListener(menuBtnVic.GetComponent<Button>().onClick, gameUI.GoToMainMenu);

        // 7. BUAT PAUSE PANEL
        GameObject pPanel = CreatePanel(canvasObj, "PausePanel", new Color(0.1f, 0.1f, 0.1f, 0.85f));
        SetRect(pPanel, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero); // Full Screen
        
        GameObject pTitle = CreateText(pPanel, "TitleText", "PERMAINAN DIJEDA", 72, Color.white, TextAlignmentOptions.Center);
        SetRect(pTitle, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -150f), new Vector2(800f, 100f));

        GameObject pContainer = CreateContainer(pPanel, "ButtonContainer", new Vector2(300f, 200f), new Vector2(0f, -100f));
        GameObject resumeBtn = CreateButton(pContainer, "ResumeButton", "Resume", 26, new Color(0.2f, 0.4f, 0.6f, 1f), Color.white);
        GameObject restartBtnPause = CreateButton(pContainer, "RestartButton", "Restart", 26, new Color(0.25f, 0.25f, 0.25f, 1f), Color.white);
        GameObject menuBtnPause = CreateButton(pContainer, "MainMenuButton", "Main Menu", 26, new Color(0.45f, 0.15f, 0.15f, 1f), Color.white);

        // Hubungkan tombol Pause secara permanen di Editor
        UnityEventTools.AddPersistentListener(resumeBtn.GetComponent<Button>().onClick, gameUI.ResumeGame);
        UnityEventTools.AddPersistentListener(restartBtnPause.GetComponent<Button>().onClick, gameUI.RestartLevel);
        UnityEventTools.AddPersistentListener(menuBtnPause.GetComponent<Button>().onClick, gameUI.GoToMainMenu);

        // 8. HUBUNGKAN REFERENSI KE GameUIManager
        gameUI.gameOverPanel = goPanel;
        gameUI.victoryPanel = vicPanel;
        gameUI.pausePanel = pPanel;
        gameUI.victoryScoreText = vicScoreText.GetComponent<TMP_Text>();
        gameUI.gameOverScoreText = goScoreText.GetComponent<TMP_Text>();
        gameUI.gameOverDistanceText = goDistanceText.GetComponent<TMP_Text>();

        // 9. HUBUNGKAN REFERENSI KE GameManager
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.scoreText = scoreTextObj.GetComponent<TMP_Text>();
            gm.distanceText = distanceTextObj.GetComponent<TMP_Text>();
            
            // Link player transform to GameManager if missing
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                gm.playerTransform = playerObj.transform;
                
                // 10. HUBUNGKAN SLIDER BENSIN KE PlayerController
                PlayerController pc = playerObj.GetComponent<PlayerController>();
                if (pc != null)
                {
                    pc.fuelSlider = sliderObj.GetComponent<Slider>();
                }
            }
        }

        // Matikan panel-panel tersebut secara default agar siap main
        goPanel.SetActive(false);
        vicPanel.SetActive(false);
        pPanel.SetActive(false);

        // Simpan perubahan
        EditorUtility.SetDirty(uiManagerObj);
        EditorUtility.SetDirty(canvasObj);
        if (gm != null) EditorUtility.SetDirty(gm.gameObject);
        
        Debug.Log("🎉 [MBGRush] UI Berhasil Dibangun Ulang secara Sempurna!");
    }

    private static GameObject CreatePanel(GameObject parent, string name, Color color, bool raycastTarget = true)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        obj.transform.SetParent(parent.transform, false);
        Image img = obj.GetComponent<Image>();
        img.color = color;
        img.raycastTarget = raycastTarget;
        return obj;
    }

    private static GameObject CreateText(GameObject parent, string name, string content, int fontSize, Color color, TextAlignmentOptions alignment)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        obj.transform.SetParent(parent.transform, false);
        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.font = TMP_Settings.defaultFontAsset;
        tmp.raycastTarget = false; // optimasi agar teks tidak memblokir raycast klik
        return obj;
    }

    private static GameObject CreateContainer(GameObject parent, string name, Vector2 size, Vector2 pos)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(VerticalLayoutGroup));
        obj.transform.SetParent(parent.transform, false);
        
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = pos;

        VerticalLayoutGroup group = obj.GetComponent<VerticalLayoutGroup>();
        group.spacing = 15;
        group.childAlignment = TextAnchor.MiddleCenter;
        group.childControlWidth = true;
        group.childControlHeight = false;
        group.childForceExpandWidth = true;
        group.childForceExpandHeight = false;

        return obj;
    }

    private static GameObject CreateButton(GameObject parent, string name, string textContent, int fontSize, Color btnColor, Color textColor)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent.transform, false);
        
        // Ukuran default tombol
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 50f);
        
        // Background tombol
        obj.GetComponent<Image>().color = btnColor;
        obj.GetComponent<Image>().raycastTarget = true;
        
        // Teks di dalam tombol
        GameObject textObj = CreateText(obj, "Text (TMP)", textContent, fontSize, textColor, TextAlignmentOptions.Center);
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return obj;
    }

    private static GameObject CreateSlider(GameObject parent, string name)
    {
        // 1. Buat Slider GameObject
        GameObject sliderObj = new GameObject(name, typeof(RectTransform), typeof(Slider));
        sliderObj.transform.SetParent(parent.transform, false);
        Slider slider = sliderObj.GetComponent<Slider>();
        slider.interactable = false; // Matikan agar bensin hanya indikator (tidak bisa digeser player)

        // 2. Buat Background
        // Set raycastTarget ke false agar slider bensin tidak memblokir raycast
        GameObject bgObj = CreatePanel(sliderObj, "Background", new Color(0.3f, 0.3f, 0.3f, 0.5f), false);
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // 3. Buat Fill Area
        GameObject fillAreaObj = new GameObject("Fill Area", typeof(RectTransform));
        fillAreaObj.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillAreaObj.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = new Vector2(5, 5);
        fillAreaRect.offsetMax = new Vector2(-5, -5);

        // 4. Buat Fill Image
        GameObject fillObj = CreatePanel(fillAreaObj, "Fill", Color.green, false);
        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = new Vector2(0.5f, 1f); // default setengah
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        // 5. Hubungkan Slider referensi
        slider.fillRect = fillRect;
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 100;

        return sliderObj;
    }

    private static void SetRect(GameObject obj, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;
    }
}
#endif
