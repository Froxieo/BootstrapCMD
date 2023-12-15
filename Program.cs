using System.Diagnostics;
using System.IO.Compression;
using System.Net;

string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string path = home+"\\AppData\\Roaming\\.FLauncher\\Launcher\\";
int passed = 0;
void Main()
{
    createFileAndDir();
    Thread.Sleep(1500);
    VerifyIfAlreadyInstall();
    return;

}

//Téléchargement des requis
void DownloadLauncher()
{
    if (File.Exists(path+"Launcher.jar"))
    {
        File.Delete(path+"Launcher.jar");
    }
    WebClient web = new WebClient();
    Console.WriteLine("Telechargement du launcher ...");
    web.DownloadFile("http://129.151.254.89/update/Launcher-JDK/javafx-launcher-1.0.0-all.jar", path+"Launcher.jar");
    Thread.Sleep(500);
    File.Delete(path+"launcher-version.txt");
    Thread.Sleep(500);
    GetLauncherVersion();
    Thread.Sleep(500);
    passed++;
    return;
}
void DownloadJDK()
{
    if (Directory.Exists(path+"JDK"))
    {
        Directory.Delete(path+"JDK", true);
    }
    WebClient web = new WebClient();
    Console.WriteLine("Telechargement du JDK, cela peut prendre du temps ...");
    web.DownloadFile("https://download.oracle.com/java/17/latest/jdk-17_windows-x64_bin.zip", path+"jdk.zip");
    Thread.Sleep(500);
    File.Delete(path+"jdk-version.txt");
    Thread.Sleep(500);
    Console.WriteLine("Extraction du JDK");
    ZipFile.ExtractToDirectory(path+"jdk.zip", path+"JDK");
    File.Delete(path+"jdk.zip");
    Thread.Sleep(2000);
    GetJDKVersion();
    Console.WriteLine("JDK and Launcher is download and install !");
    Thread.Sleep(6000);
    start();
    passed++;
    return;
}

//Verification des requis
void VerifyIfAlreadyInstall()
{
    void Launcher()
    {
        if (File.Exists(path+"Launcher.jar"))
        {
            Console.WriteLine("Le Launcher est deja installé, skipping...");
            passed++;
        }
        else
        {
            DownloadLauncher();
        }
        return;
    }
    void JDK()
    {
        if (Directory.Exists(path+"JDK"))
        {
            Console.WriteLine("Java est deja installé, skipping...");
            passed++;
        }
        else
        {
            DownloadJDK();
        }
        return;
    }
    if (File.Exists(path+"Launcher.jar") && Directory.Exists(path+"JDK"))
    {
        VersionManager();
    }
    Launcher();
    JDK();
    return;
}
//Creation du repertoir Launcher et des fichiers de sauvgarde
void createFileAndDir()
{
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    return;
}

void VersionManager()
{
    //Get online info
    WebClient web = new WebClient();

    string OnlineLauncherInfo = web.DownloadString("http://129.151.254.89/update/Launcher-JDK/launcher-version.txt");
    string OnlineJDKInfo = web.DownloadString("http://129.151.254.89/update/Launcher-JDK/jdk-version.txt");
    //Get disk info
    StreamReader GetdiskLauncherInfo = new StreamReader(path+"launcher-version.txt");
    string diskLauncherInfo = GetdiskLauncherInfo.ReadToEnd();
    GetdiskLauncherInfo.Close();
    StreamReader GetdiskJDKInfo = new StreamReader(path+"jdk-version.txt");
    string diskJDKIngo = GetdiskJDKInfo.ReadToEnd();
    Thread.Sleep(2000);

    if (diskLauncherInfo != OnlineLauncherInfo)
    {
        DownloadLauncher();
    }
    if (diskJDKIngo != OnlineJDKInfo)
    {
        DownloadJDK();
    }
    if ((diskLauncherInfo == OnlineLauncherInfo) && (diskJDKIngo == OnlineJDKInfo))
    {
        Console.WriteLine("Tout est a jour !");
        start();
    }
    return;
}
void GetLauncherVersion()
{
    WebClient getlauncherversion = new WebClient();
    getlauncherversion.DownloadFile("http://129.151.254.89/update/Launcher-JDK/launcher-version.txt", path+"launcher-version.txt");
    return;
}
void GetJDKVersion()
{
    WebClient getjdkversion = new WebClient();
    getjdkversion.DownloadFile("http://129.151.254.89/update/Launcher-JDK/jdk-version.txt", path+"jdk-version.txt");
    return;

}
void start()
{
    string execLauncher = path+"JDK\\jdk-17.0.9\\bin\\java.exe -jar "+path+"Launcher.jar";
    Process.Start("powershell.exe", execLauncher);
}


Main();