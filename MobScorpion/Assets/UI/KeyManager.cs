using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement p1Movement;
    [SerializeField] private PlayerMovement p2Movement;
    [SerializeField] private SwordSwing p1SwordSwing;
    [SerializeField] private SwordSwing p2SwordSwing;
    [SerializeField] private ArrowShooter p1ArrowShoot;
    [SerializeField] private ArrowShooter p2ArrowShoot;
    [SerializeField] private ShieldSummon p1ShieldSummon;
    [SerializeField] private ShieldSummon p2ShieldSummon;
    [SerializeField] private List<TextMeshProUGUI> texts;

    private List<string> playerPrefs = new List<string> {"up1", "up2", "down1", "down2", "right1", "right2", "left1", "left2", "sword1", "sword2", "bow1", "bow2", "shield1", "shield2"};
    
    private List<KeyCode> keyBindings = new List<KeyCode> ();
    private int selectedUIKey = -1;
    private KeyCode[] keyCodes = new KeyCode[]
    {
        KeyCode.None,
        KeyCode.Backspace,
        KeyCode.Delete,
        KeyCode.Tab,
        KeyCode.Clear,
        KeyCode.Return,
        KeyCode.Pause,
        KeyCode.Escape,
        KeyCode.Space,
        KeyCode.Keypad0,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9,
        KeyCode.KeypadPeriod,
        KeyCode.KeypadDivide,
        KeyCode.KeypadMultiply,
        KeyCode.KeypadMinus,
        KeyCode.KeypadPlus,
        KeyCode.KeypadEnter,
        KeyCode.KeypadEquals,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.Insert,
        KeyCode.Home,
        KeyCode.End,
        KeyCode.PageUp,
        KeyCode.PageDown,
        KeyCode.F1,
        KeyCode.F2,
        KeyCode.F3,
        KeyCode.F4,
        KeyCode.F5,
        KeyCode.F6,
        KeyCode.F7,
        KeyCode.F8,
        KeyCode.F9,
        KeyCode.F10,
        KeyCode.F11,
        KeyCode.F12,
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Exclaim,
        KeyCode.DoubleQuote,
        KeyCode.Hash,
        KeyCode.Dollar,
        KeyCode.Percent,
        KeyCode.Ampersand,
        KeyCode.Quote,
        KeyCode.LeftParen,
        KeyCode.RightParen,
        KeyCode.Asterisk,
        KeyCode.Plus,
        KeyCode.Comma,
        KeyCode.Minus,
        KeyCode.Period,
        KeyCode.Slash,
        KeyCode.Colon,
        KeyCode.Semicolon,
        KeyCode.Less,
        KeyCode.Equals,
        KeyCode.Greater,
        KeyCode.Question,
        KeyCode.At,
        KeyCode.LeftBracket,
        KeyCode.Backslash,
        KeyCode.RightBracket,
        KeyCode.Caret,
        KeyCode.Underscore,
        KeyCode.BackQuote,
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z,
        KeyCode.LeftCurlyBracket,
        KeyCode.Pipe,
        KeyCode.RightCurlyBracket,
        KeyCode.Tilde,
        KeyCode.Numlock,
        KeyCode.CapsLock,
        KeyCode.ScrollLock,
        KeyCode.RightShift,
        KeyCode.LeftShift,
        KeyCode.RightControl,
        KeyCode.LeftControl,
        KeyCode.RightAlt,
        KeyCode.LeftAlt,
        KeyCode.LeftMeta,
        KeyCode.RightMeta,
        KeyCode.Help,
        KeyCode.Print,
        KeyCode.Break,
        KeyCode.Menu
    };


    void Start()
    {
        // get default / user set key binds
        foreach (string p in playerPrefs)
        {
            if (PlayerPrefs.HasKey(p))
            {
                string key = PlayerPrefs.GetString(p);
                KeyCode keyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), key);
                SetPlayerScriptKey(p, keyCode);
                keyBindings.Add(keyCode);
            }
            else
            {
                if (p == "up1") {
                    PlayerPrefs.SetString(p, p1Movement.upButton.ToString());
                    keyBindings.Add(p1Movement.upButton);
                } 
                else if (p == "up2") {
                    PlayerPrefs.SetString(p, p2Movement.upButton.ToString());
                    keyBindings.Add(p2Movement.upButton);
                } 
                else if (p == "down1") {
                    PlayerPrefs.SetString(p, p1Movement.downButton.ToString());
                    keyBindings.Add(p1Movement.downButton);
                } 
                else if (p == "down2") {
                    PlayerPrefs.SetString(p, p2Movement.downButton.ToString());
                    keyBindings.Add(p2Movement.downButton);
                } 
                else if (p == "right1") {
                    PlayerPrefs.SetString(p, p1Movement.rightButton.ToString());
                    keyBindings.Add(p1Movement.rightButton);
                } 
                else if (p == "right2") {
                    PlayerPrefs.SetString(p, p2Movement.rightButton.ToString());
                    keyBindings.Add(p2Movement.rightButton);
                } 
                else if (p == "left1") {
                    PlayerPrefs.SetString(p, p1Movement.leftButton.ToString());
                    keyBindings.Add(p1Movement.leftButton);
                } 
                else if (p == "left2") {
                    PlayerPrefs.SetString(p, p2Movement.leftButton.ToString());
                    keyBindings.Add(p2Movement.leftButton);
                } 
                else if (p == "sword1") {
                    PlayerPrefs.SetString(p, p1SwordSwing.swingButton.ToString());
                    keyBindings.Add(p1SwordSwing.swingButton);
                } 
                else if (p == "sword2") {
                    PlayerPrefs.SetString(p, p2SwordSwing.swingButton.ToString());
                    keyBindings.Add(p2SwordSwing.swingButton);
                } 
                else if (p == "bow1") {
                    PlayerPrefs.SetString(p, p1ArrowShoot.shootButton.ToString());
                    keyBindings.Add(p1ArrowShoot.shootButton);
                } 
                else if (p == "bow2") {
                    PlayerPrefs.SetString(p, p2ArrowShoot.shootButton.ToString());
                    keyBindings.Add(p2ArrowShoot.shootButton);
                } 
                else if (p == "shield1") {
                    PlayerPrefs.SetString(p, p1ShieldSummon.shieldButton.ToString());
                    keyBindings.Add(p1ShieldSummon.shieldButton);
                } 
                else if (p == "shield2") {
                    PlayerPrefs.SetString(p, p2ShieldSummon.shieldButton.ToString());
                    keyBindings.Add(p2ShieldSummon.shieldButton);
                }
            }
        }

        // update UI to display key binds
        for (int i = 0; i < texts.Count; ++i)
        {
            texts[i].text = StringOfKeyCode(keyBindings[i]);
        }
    }

    void Update()
    {
        if (selectedUIKey != -1)
        {
            foreach (KeyCode code in keyCodes)
            {
                if (Input.GetKey(code))
                {
                    if (code == KeyCode.Space)
                    {
                        selectedUIKey = -1;
                        break;
                    }
                    // a key was pressed
                    if (keyBindings.Contains(code))
                    {
                        // swap two key bindings
                        print("swapping two key bindings");
                        int otherUIKey = keyBindings.IndexOf (code);
                        KeyCode selectedUIKeyCode = keyBindings[selectedUIKey];
                        keyBindings[otherUIKey] = selectedUIKeyCode;
                        texts[otherUIKey].text = StringOfKeyCode(selectedUIKeyCode);
                        PlayerPrefs.SetString(playerPrefs[otherUIKey], selectedUIKeyCode.ToString ());
                        SetPlayerScriptKey(playerPrefs[otherUIKey], selectedUIKeyCode);
                    }
                    // substitute one key
                    print("substituting out one key");
                    keyBindings[selectedUIKey] = code;
                    texts[selectedUIKey].text = StringOfKeyCode(code);
                    PlayerPrefs.SetString(playerPrefs[selectedUIKey], code.ToString ());
                    SetPlayerScriptKey(playerPrefs[selectedUIKey], code);
                    break;
                }
            }
        }
    }

    public void SetSelectedUIKey (int uiKey)
    {
        selectedUIKey = uiKey;
    }

    private void SetPlayerScriptKey (string playerPrefKey, KeyCode newKey)
    {
        if (playerPrefKey == "up1") {
            p1Movement.upButton = newKey;
        } 
        else if (playerPrefKey == "up2") {
            p2Movement.upButton = newKey;
        } 
        else if (playerPrefKey == "down1") {
            p1Movement.downButton = newKey;
        } 
        else if (playerPrefKey == "down2") {
            p2Movement.downButton = newKey;
        } 
        else if (playerPrefKey == "right1") {
            p1Movement.rightButton = newKey;
        } 
        else if (playerPrefKey == "right2") {
            p2Movement.rightButton = newKey;
        } 
        else if (playerPrefKey == "left1") {
            p1Movement.leftButton = newKey;
        } 
        else if (playerPrefKey == "left2") {
            p2Movement.leftButton = newKey;
        } 
        else if (playerPrefKey == "sword1") {
            p1SwordSwing.swingButton = newKey;
        } 
        else if (playerPrefKey == "sword2") {
            p2SwordSwing.swingButton = newKey;
        } 
        else if (playerPrefKey == "bow1") {
            p1ArrowShoot.shootButton = newKey;
        } 
        else if (playerPrefKey == "bow2") {
            p2ArrowShoot.shootButton = newKey;
        } 
        else if (playerPrefKey == "shield1") {
            p1ShieldSummon.shieldButton = newKey;
        } 
        else if (playerPrefKey == "shield2") {
            p2ShieldSummon.shieldButton = newKey;
        }
    }

    public string StringOfKeyCode (KeyCode key)
    {
        switch (key)
        {
            case KeyCode.None: return "Not assigned";
            case KeyCode.Backspace: return "◀";
            case KeyCode.Delete: return "Delete";
            case KeyCode.Tab: return "Tab";
            case KeyCode.Clear: return "Clear";
            case KeyCode.Return: return "Return";
            case KeyCode.Pause: return "Pause";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Space: return "Space";
            case KeyCode.Keypad0: return "0";
            case KeyCode.Keypad1: return "1";
            case KeyCode.Keypad2: return "2";
            case KeyCode.Keypad3: return "3";
            case KeyCode.Keypad4: return "4";
            case KeyCode.Keypad5: return "5";
            case KeyCode.Keypad6: return "6";
            case KeyCode.Keypad7: return "7";
            case KeyCode.Keypad8: return "8";
            case KeyCode.Keypad9: return "9";
            case KeyCode.KeypadPeriod: return ".";
            case KeyCode.KeypadDivide: return "/";
            case KeyCode.KeypadMultiply: return "*";
            case KeyCode.KeypadMinus: return "-";
            case KeyCode.KeypadPlus: return "+";
            case KeyCode.KeypadEnter: return "Enter";
            case KeyCode.KeypadEquals: return "=";
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.RightArrow: return "→";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.Insert: return "Insert";
            case KeyCode.Home: return "Home";
            case KeyCode.End: return "End";
            case KeyCode.PageUp: return "Page Up";
            case KeyCode.PageDown: return "Page Down";
            case KeyCode.F1: return "F1";
            case KeyCode.F2: return "F2";
            case KeyCode.F3: return "F3";
            case KeyCode.F4: return "F4";
            case KeyCode.F5: return "F5";
            case KeyCode.F6: return "F6";
            case KeyCode.F7: return "F7";
            case KeyCode.F8: return "F8";
            case KeyCode.F9: return "F9";
            case KeyCode.F10: return "F10";
            case KeyCode.F11: return "F11";
            case KeyCode.F12: return "F12";
            case KeyCode.Alpha0: return "0";
            case KeyCode.Alpha1: return "1";
            case KeyCode.Alpha2: return "2";
            case KeyCode.Alpha3: return "3";
            case KeyCode.Alpha4: return "4";
            case KeyCode.Alpha5: return "5";
            case KeyCode.Alpha6: return "6";
            case KeyCode.Alpha7: return "7";
            case KeyCode.Alpha8: return "8";
            case KeyCode.Alpha9: return "9";
            case KeyCode.Exclaim: return "!";
            case KeyCode.DoubleQuote: return "\"";
            case KeyCode.Hash: return "#";
            case KeyCode.Dollar: return "$";
            case KeyCode.Percent: return "%";
            case KeyCode.Ampersand: return "&";
            case KeyCode.Quote: return "'";
            case KeyCode.LeftParen: return "(";
            case KeyCode.RightParen: return ")";
            case KeyCode.Asterisk: return "*";
            case KeyCode.Plus: return "+";
            case KeyCode.Comma: return ",";
            case KeyCode.Minus: return "-";
            case KeyCode.Period: return ".";
            case KeyCode.Slash: return "/";
            case KeyCode.Colon: return ":";
            case KeyCode.Semicolon: return ";";
            case KeyCode.Less: return "<";
            case KeyCode.Equals: return "=";
            case KeyCode.Greater: return ">";
            case KeyCode.Question: return "?";
            case KeyCode.At: return "@";
            case KeyCode.LeftBracket: return "[";
            case KeyCode.Backslash: return "\\";
            case KeyCode.RightBracket: return "]";
            case KeyCode.Caret: return "^";
            case KeyCode.Underscore: return "_";
            case KeyCode.BackQuote: return "`";
            case KeyCode.A: return "a";
            case KeyCode.B: return "b";
            case KeyCode.C: return "c";
            case KeyCode.D: return "d";
            case KeyCode.E: return "e";
            case KeyCode.F: return "f";
            case KeyCode.G: return "g";
            case KeyCode.H: return "h";
            case KeyCode.I: return "i";
            case KeyCode.J: return "j";
            case KeyCode.K: return "k";
            case KeyCode.L: return "l";
            case KeyCode.M: return "m";
            case KeyCode.N: return "n";
            case KeyCode.O: return "o";
            case KeyCode.P: return "p";
            case KeyCode.Q: return "q";
            case KeyCode.R: return "r";
            case KeyCode.S: return "s";
            case KeyCode.T: return "t";
            case KeyCode.U: return "u";
            case KeyCode.V: return "v";
            case KeyCode.W: return "w";
            case KeyCode.X: return "x";
            case KeyCode.Y: return "y";
            case KeyCode.Z: return "z";
            case KeyCode.LeftCurlyBracket: return "{";
            case KeyCode.Pipe: return "|";
            case KeyCode.RightCurlyBracket: return "}";
            case KeyCode.Tilde: return "~";
            case KeyCode.Numlock: return "Numlock";
            case KeyCode.CapsLock: return "Caps Lock";
            case KeyCode.ScrollLock: return "Scroll Lock";
            case KeyCode.RightShift: return "Shift";
            case KeyCode.LeftShift: return "Shift";
            case KeyCode.RightControl: return "Ctrl";
            case KeyCode.LeftControl: return "Crtl";
            case KeyCode.RightAlt: return "Alt";
            case KeyCode.LeftAlt: return "Alt";
            case KeyCode.LeftMeta: return "Meta";
            case KeyCode.RightMeta: return "Meta";
            case KeyCode.Help: return "Help";
            case KeyCode.Print: return "Print";
            case KeyCode.Break: return "Break";
            case KeyCode.Menu: return "Menu";
            default: return "Unknown";
        }
    }
}
