using Keystroke.API.CallbackObjects;
using Keystroke.API.Helpers;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Keystroke.API
{
    public class KeystrokeAPI : IDisposable
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private IntPtr globalKeyboardHookId;
        private IntPtr globalMouseHookId;
        private IntPtr currentModuleId;
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_SYSKEYDOWN = 0x104;
        private User32.LowLevelHook HookKeyboardDelegate; //We need to have this delegate as a private field so the GC doesn't collect it
        private User32.LowLevelHook HookMouseDelegate; //We need to have this delegate as a private field so the GC doesn't collect it
        private Action<KeyPressed> keyPressedCallback;
        private Action<MousePressed> mousePressedCallback;

        public KeystrokeAPI()
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModudle = currentProcess.MainModule;
            this.currentModuleId = User32.GetModuleHandle(currentModudle.ModuleName);
        }

        public void CreateKeyboardHook(Action<KeyPressed> keyPressedCallback)
        {
            this.keyPressedCallback = keyPressedCallback;
            this.HookKeyboardDelegate = HookKeyboardCallbackImplementation;
            this.globalKeyboardHookId = User32.SetWindowsHookEx(WH_KEYBOARD_LL, this.HookKeyboardDelegate, this.currentModuleId, 0);
        }

        public void CreateMouseHook(Action<MousePressed> mousePressedCallback)
        {
            this.mousePressedCallback = mousePressedCallback;
            HookMouseDelegate = HookMouseCallbackImplementation;
            globalMouseHookId = User32.SetWindowsHookEx(WH_MOUSE_LL, HookMouseDelegate, currentModuleId, 0);
        }

        public void RemoveKeyboardHook()
        {
            if (globalKeyboardHookId != IntPtr.Zero)
                User32.UnhookWindowsHookEx(globalKeyboardHookId);
        }

        public void RemoveMouseHook()
        {
            if (globalMouseHookId != IntPtr.Zero)
                User32.UnhookWindowsHookEx(globalMouseHookId);
        }

        private IntPtr HookKeyboardCallbackImplementation(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int wParamAsInt = wParam.ToInt32();

            if (nCode >= 0 && (wParamAsInt == WM_KEYDOWN || wParamAsInt == WM_SYSKEYDOWN))
            {
                bool shiftPressed = false;
                bool capsLockActive = false;

                var shiftKeyState = User32.GetAsyncKeyState(KeyCode.ShiftKey);
                if (FirstBitIsTurnedOn(shiftKeyState))
                    shiftPressed = true;

                //We need to use GetKeyState to verify if CapsLock is "TOGGLED" 
                //because GetAsyncKeyState only verifies if it is "PRESSED" at the moment
                if (User32.GetKeyState(KeyCode.Capital) == 1)
                    capsLockActive = true;

                KeyParser(wParam, lParam, shiftPressed, capsLockActive);
            }

            //Chain to the next hook. Otherwise other applications that 
            //are listening to this hook will not get notificied
            return User32.CallNextHookEx(globalKeyboardHookId, nCode, wParam, lParam);
        }

        private IntPtr HookMouseCallbackImplementation(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var mouseBtnCode = (MouseButtonCode)wParam;
            if (nCode >= 0 && (MouseButtonCode.WM_LBUTTONDOWN == mouseBtnCode || MouseButtonCode.WM_RBUTTONDOWN == mouseBtnCode))
            {
                var hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                var mouse = new MousePressed
                {
                    ButtonCode = mouseBtnCode,
                    CurrentWindow = new Window().CurrentWindowTitle(),
                    X = hookStruct.pt.x,
                    Y = hookStruct.pt.y
                };

                mousePressedCallback.Invoke(mouse);
            }

            return User32.CallNextHookEx(globalMouseHookId, nCode, wParam, lParam);
        }

        private bool FirstBitIsTurnedOn(short value)
        {
            //0x8000 == 1000 0000 0000 0000			
            return Convert.ToBoolean(value & 0x8000);
        }

        private void KeyParser(IntPtr wParam, IntPtr lParam, bool shiftPressed, bool capsLockPressed)
        {
            var keyValue = (KeyCode)Marshal.ReadInt32(lParam);

            var keyboardLayout = new KeyboardLayout().GetCurrentKeyboardLayout();
            var windowTitle = new Window().CurrentWindowTitle();

            var key = new KeyPressed(keyValue, shiftPressed, capsLockPressed, windowTitle, keyboardLayout.ToString());

            keyPressedCallback.Invoke(key);
        }

        public void Dispose()
        {
            RemoveKeyboardHook();
        }
    }
}
