using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class consoleController : MonoBehaviour
{
    public GameObject console;
    public TMP_InputField inputField;
    public TextMeshProUGUI outputText;

    private Dictionary<string, System.Action<string[]>> commands;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outputText.fontSize = 34;
        outputText.lineSpacing = 21;

        commands = new Dictionary<string, System.Action<string[]>>()
        {
            { "godmode", ToggleGodModeCommand },
            { "fly", ToggleFlyCommand },
            { "help", ShowHelp }
        };

    }

    // Update is called once per frame
    void Update()
    {
        gamemanager.instance.ToggleConsole();
    }

    public void ProcessCommand(string input)
    {
        string[] parts = input.Trim().ToLower().Split(' ');
        string command = parts[0];
        string[] args = parts.Length > 1 ? parts[1..] : new string[0];

        if (commands.ContainsKey(command))
        {
            commands[command].Invoke(args);
        } else
        {
            LogOutput($"Unknown command: {command}");
        }

        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

    void LogOutput(string message)
    {
        outputText.text += message + "\n";
    }

    void ToggleGodModeCommand(string[] args)
    {
        bool enable = args.Length > 0 && args[0].ToLower() == "on";
        gamemanager.instance.ToggleGodMode(enable);

        LogOutput(enable ? "God mode enabled!" : "God mode disabled!");
    }

    void ToggleFlyCommand(string[] args)
    {
        bool enable = args.Length > 0 && args[0].ToLower() == "on";

        playerController player = GetComponent<playerController>();
        if (player != null)
        {
            player.ToggleFly(enable);
            LogOutput(enable ? "Fly mode enabled!" : "Fly mode disabled!");
        }
    }

    void ShowHelp(string[] args)
    {
        string helpText = "Available commands:\n";
        helpText += "- godmode [on/off] - Toggles god mode\n";
        helpText += "- help - Shows this help message\n";
        LogOutput("Available commands:\n- godmode [on/off]\n- help");
    }

    
}
