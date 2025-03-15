using System.Windows.Forms;

class Program
{
    static void Main()
    {
        Member.Init();
        MainForm mf = new MainForm();
        Application.Run(mf);
    }
}