using System.Media;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CodeSyncio {
class Outro {
    static void Main(string[] args) {
        string dir = System.IO.Directory.GetCurrentDirectory();
        DownloadAudio().Wait();
        Console.Clear();
        MainCode();
    }
    static async Task DownloadAudio() {
        if (File.Exists("outrosound.wav")) {
            Console.WriteLine(
                "Song has already been downloaded, skipping download...");
        } else {
            Console.WriteLine(
                "audio file not found or corrupt, starting download...");
            using(var client = new System.Net.Http.HttpClient()) {
                var uri = new Uri(
                    "https://github.com/CodeSyncio/computer-outro-downloadables/blob/main/outro.wav?raw=true");
                using(var s = await client.GetStreamAsync(uri)) {
                    var fileName = @"outrosound.wav";
                    using(var fs =new FileStream(fileName, FileMode.CreateNew)) {
                        await s.CopyToAsync(fs);
                    }
                }
            }
            Console.WriteLine("Done downloading audiofile");
        }
    }
    static async Task DownloadHTA() {
        if (File.Exists("screen.hta")) {
            return;
        } else {
            using(var client = new System.Net.Http.HttpClient()) {
                var uri = new Uri(
                    "https://github.com/CodeSyncio/computer-outro-downloadables/blob/main/fakebsod.hta?raw=true");
                using(var s = await client.GetStreamAsync(uri)) {
                    var fileName = @"screen.hta";
                    using(var fs =new FileStream(fileName, FileMode.CreateNew)) {
                        await s.CopyToAsync(fs);
                    }
                }
            }
        }
    }
    static void MainCode() {
        string choise;
        Console.Write(
            "Please choose an option:\n[1] Real BSOD\n[2] Fake BSOD\n[3] Shutdown\n");
        string ? v = Console.ReadLine();
        if (v == "") {
            choise = "3";
        } else {
            choise = v;
        }
        if (choise == "1") {
            Console.Clear();
            sound();
            for (int i = 0; i < 10; i++) {
                Console.WriteLine("BSOD in " + (10 - i) + " seconds");
                Thread.Sleep(1000);
            }
            RealBSOD();
        }
        if (choise == "2") {
            DownloadHTA().Wait();
            Console.Clear();
            sound();
            for (int i = 0; i < 10; i++) {
                Console.WriteLine("Fake BSOD in " + (10 - i) + " seconds");
                Thread.Sleep(1000);
            }
            Process.Start("explorer.exe",Path.Combine(System.IO.Directory.GetCurrentDirectory(),"screen.hta"));
            if (choise == "3") {
                Console.Clear();
                sound();
                for (int i = 0; i < 10; i++) {
                    Console.WriteLine("Shutting down in " + (10 - i) +" seconds");
                    Thread.Sleep(1000);
                }
                Process.Start("shutdown", "/s /t 0");
            }
        }
        static void RealBSOD() {
            [DllImport("ntdll.dll")]
            static extern uint RtlAdjustPrivilege(
                int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege,
                out bool PreviousValue);
            [DllImport("ntdll.dll")]
            static extern uint NtRaiseHardError(
                uint ErrorStatus, uint NumberOfParameters,
                uint UnicodeStringParameterMask, IntPtr Parameters,
                uint ValidResponseOption, out uint Response);
            Boolean t1;
            uint t2;
            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
        }
        static void sound() {
            const string soundLocation = "outrosound.wav";
            System.Media.SoundPlayer player = new SoundPlayer(soundLocation);
            player.Play();
            }
        }
    }
}