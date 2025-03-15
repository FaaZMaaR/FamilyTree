using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

class EventMaster
{
    public static Random rnd = new Random();
    public static float num = 0;
    public static float marriage_chance = 0.001F;
    public class EDialog : Form
    {
        public Label lText;
        public Button bOne, bTwo, bThree, bFour;
        public EDialog() : base()
        {
            Size = new Size(400, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Meeting a partner";
            Font = new Font("Arial", 12, FontStyle.Regular);
            lText = new Label();
            lText.SetBounds(10, 10, ClientSize.Width - 20, 150);
            lText.TextAlign = ContentAlignment.MiddleCenter;
            lText.Text = "Message";
            Controls.Add(lText);
            bOne = new Button();
            bOne.Left = ClientSize.Width / 2 - 100;
            bOne.Top = lText.Bottom + 10;
            bOne.Size = new Size(200, 30);
            bOne.Text = "First option";
            Controls.Add(bOne);
            bTwo = new Button();
            bTwo.Left = bOne.Left;
            bTwo.Top = bOne.Bottom + 10;
            bTwo.Size = bOne.Size;
            bTwo.Text = "Second option";
            Controls.Add(bTwo);
        }
    }
    public static EDialog dialog;
    public static bool exit = false;
    public static void PlayMarriage(Member m, ArrayList reg, DateTime d)
    {
        exit = false;
        if (d.Day == 14 && m.Get_Age(d) >= 25 && m.partner == null && m.health_status == "Healthy")
        {
            num = (float)rnd.Next(1001) / 1000;
            if (num < marriage_chance + (float)m.health / 1000 + (float)m.charisma / 1000 + (float)m.intelligence / 1000 + (float)m.luck / 1000)
            {
                dialog = new EDialog();
                dialog.lText.Text = m.name + ", " + m.Get_Age(d) + " found love! Arrange a wedding? (" + num + "|" + (marriage_chance + (float)m.health / 1000 + (float)m.charisma / 1000 + (float)m.intelligence / 1000 + (float)m.luck / 1000) + ")";
                dialog.bOne.Text = "Yes";
                dialog.bTwo.Text = "No";
                dialog.bOne.Click += (a, b) => {
                    m.Get_Married(reg);
                    m.coords[0] -= 60;
                    m.partner.coords[0] = m.coords[0] + 120;
                    m.partner.coords[1] = m.coords[1];
                    dialog.Close();
                    exit = true;
                };
                dialog.bTwo.Click += (a, b) => {
                    dialog.Close();
                    exit = true;
                };
                dialog.ShowDialog();
                while (!exit) { }
            }
        }
    }
}