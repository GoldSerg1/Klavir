
using Klavir.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Klavir
{
    
    internal static class Program 
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mutex singleton = new Mutex(true, "KeyboardProg-Klavir-Highlander");

            if (!singleton.WaitOne(TimeSpan.Zero, true))
            {
                //MessageBox.Show("Ïðèëîæåíèå óæå çàïóùåíî!");
                singleton.Dispose();
                Application.Exit();
            }
            else
            {
                ApplicationConfiguration.Initialize();
                hookID = SetHook(procHook);
                Application.Run(new MyTrayApp());
                UnhookWindowsHookEx(hookID);
            }

        }

        // Ïðåîáðàçîâàíèå íàæàòîé êëàâèøè â òåêñò äëÿ îòîáðàæåíèÿ
        public static string Key2Str(KeyEventArgs e, bool kCtrl = false, bool kShift = false)
        {
            bool added = false;
            string res = "";

            if (e.Alt) 
            { 
                res += "Alt"; added = true;
            }
            if (e.Control || kCtrl)
            {
                if (added) res += "+";
                res += "Ctrl"; added = true;
            }
            if (e.Shift || kShift)
            {
                if (added) res += "+";
                res += "Shift"; added = true;
            }
            if (e.KeyCode != Keys.Menu && e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.ShiftKey)
            {
                if (added) res += "+";
                res += e.KeyCode.ToString();
            }

            return res;
        }


        // Äëÿ óñòàíîâêè êëàâèàòóðíîãî õóêà
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private static LowLevelKeyboardProc procHook = HookCallback;
        private static IntPtr hookID = IntPtr.Zero;

        private static bool isCtrl = false;   // true, åñëè áûëà íàæàòà (162=LCtrl,  163=RCtrl)
        private static bool isShift = false;  // true, åñëè áûëà íàæàòà (160=LShift, 161=RShift)

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule? curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int kCode;

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)  // Êëàâèøà îòïóùåíà
            {
                kCode = Marshal.ReadInt32(lParam);
                if (kCode == 162 || kCode == 163) isCtrl = false;
                else if (kCode == 160 || kCode == 161) isShift = false;
            }
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN) // Êëàâèøà íàæàòà
            {
                kCode = Marshal.ReadInt32(lParam);
                if (kCode == 162 || kCode == 163) isCtrl = true;
                else if (kCode == 160 || kCode == 161) isShift = true;
                KeyEventArgs k = new KeyEventArgs((Keys)kCode);
                string kCodeStr = Key2Str(k, isCtrl, isShift);
                //MyTrayApp.Save2Log(kCode.ToString() + "=" + kCodeStr);    // Log
                string? pathExe = MyTrayApp.SearchInKeylist(kCodeStr);
                if (pathExe != null) 
                {
                    Process.Start(pathExe);
                }
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }


    public class MyTrayApp : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public static string configName = "cfg.txt";
        public static string programName = "Klavir.exe";
        public static string logPath = "";

        public static Dictionary<string, string> keylist = new Dictionary<string, string>();


        public MyTrayApp()
        {
            // Èíèöèàëèçàöèÿ Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Items = { new ToolStripMenuItem("Íàñòðîéêè", null, Show), new ToolStripMenuItem("Âûõîä", null, Exit) }
                },
                Visible = true
            };

            // Çàãðóçèòü äàííûå èç ôàéëà
            var path = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(path, configName);

            if (File.Exists(configPath))
            {
                using (StreamReader reader = new StreamReader(configPath))
                {
                    string text = reader.ReadToEnd();
                    Str2keylist(text);
                }
            }

            // Ñôîðìèðîâàòü ïóòü äëÿ çàïèñè ëîã-ôàéëà
            logPath = Path.Combine(path, "log.txt");
        }


        // Ìåíþ Exit
        void Exit(object? sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        // Ìåíþ Show
        void Show(object? sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
        }


        // Îáðàáîò÷èê ùåë÷êà ëåâîé êíîïêîé ìûøè
        private void notifyIcon_Click(object sender, EventArgs e)
        {
            var eventArgs = e as MouseEventArgs;
            switch (eventArgs?.Button)
            {
                case MouseButtons.Left:
                    // 
                    break;
            }
        }

        // Ñ÷èòàííóþ ñòðîêó - â ñëîâàðü keylist
        public static void Str2keylist(string s)
        {
            if (s == null) return;
            if (s == "") return;

            keylist.Clear();

            string[] mStr = s.Replace("\r", "").Split("\n");

            foreach (string str in mStr)
            {
                if (str == "") continue;

                string[] mKey = str.Split(',');
                // mKey[0]  key
                // mKey[1]  path
                if (!keylist.ContainsKey(mKey[0])) keylist.Add(mKey[0], mKey[1]);
            }
        }


        // Âåðíóòü ïî ïîëó÷åííîé êîìáèíàöèè êëàâèø ëèáî ñòðîêó äëÿ çàïóñêà ëèáî null, åñëè íå íàéäåíî
        public static string? SearchInKeylist(string s)
        {
            string? res;

            keylist.TryGetValue(s, out res);

            return res;
        }


        // Çàïèñàòü ñòðîêó â ëîã ïî àäðåñó logPath, åñëè s != ""
        public static async void Save2Log(string s)
        {
            if (s != null)
            {
                if (s != "")
                {
                    // Çàïèñàòü ñòðîêó â ôàéë
                    using (StreamWriter writer = new StreamWriter(logPath, true))
                    {
                        await writer.WriteLineAsync(s);
                    }
                }
            }
        }



    }
}
