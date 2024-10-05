using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Threading;
using System.Xml;
using System.IO;

class Member
{
    public string name;
    public DateTime birthday, deathday;
    public char sex;
    public int health, charisma, intelligence, luck;
    public float income = 0;
    public float costs = 0;
    public Member parent, partner;
    public ArrayList children = new ArrayList();
    public int kin, generation, number, id;
    public string primary_ed = "No";
    public string secondary_ed = "No";
    public string higher_ed = "No";
    public string specialization = "No";
    public int spec_level = 0;
    public int[] unspec_levels = new int[] { 0, 0, 0, 0, 0 };
    public string health_status = "Healthy";
    public string career_status = "Unemployed";

    public float[] coords = new float[] { 0, 0 };

    public static string[] m_names = new string[]{
        "William","Jacob","Oliver","James","Ethan",
        "Benjamin","Logan","Lucas","Daniel","Michael",
        "Henry","Jack","Joseph","Matthew","David",
        "Dylan","Thomas","John","Anthony","Ryan",
        "Andrew","Christopher","Leo","Nathan","Julian",
        "George","Charles","Jonathan","Adam","Theodore"
    };
    public static string[] f_names = new string[]{
        "Emma","Sophia","Olivia","Isabella","Mia",
        "Amelia","Charlotte","Elizabeth","Grace","Scarlett",
        "Lily","Victoria","Layla","Zoe","Mila",
        "Riley","Eleanor","Lucy","Anna","Violet",
        "Alice","Eva","Maya","Bella","Luna",
        "Samantha","Stella","Sarah","Gabriella","Aurora"
    };
    public static string[] spec_names = new string[]{"Journalism","Art design","Music","Economics","Law","Management",
        "Science","Engineering","Medicine","Programming","Cooking","Police","Soldiery"};
    public static Hashtable specialities = new Hashtable();
    public static float[] wages = new float[] { 150, 300, 600, 1200, 2500 };
    public static float[] base_costs = new float[] { 75, 120, 150, 100 };
    public static Random rnd = new Random();

    public Member(string nm, int brd, int brm, int bry, char sx, Member prnt, int kn, int gen, int num, int ind)
    {
        name = nm;
        birthday = new DateTime(bry, brm, brd);
        sex = sx;
        if (prnt != null)
        {
            health = rnd.Next(10, ((prnt.health + prnt.partner.health) / 2));
            charisma = rnd.Next(10, ((prnt.charisma + prnt.partner.charisma) / 2));
            intelligence = rnd.Next(10, ((prnt.intelligence + prnt.partner.intelligence / 2)));
        }
        else
        {
            health = rnd.Next(10, 101);
            charisma = rnd.Next(10, 101);
            intelligence = rnd.Next(10, 101);
        }
        luck = rnd.Next(10, 101);
        parent = prnt;
        partner = null;
        kin = kn;
        generation = gen;
        number = num;
        id = ind;
    }
    public Member() { }
    public override string ToString()
    {
        string txt = "Name: " + name;
        txt += "\nBirthday: " + birthday.ToString("dd.MM.yyyy");
        txt += "\nSex: ";
        if (sex == 'm') txt += "Male";
        else txt += "Female";
        txt += "\nHealth: " + health;
        txt += "\nCharisma: " + charisma;
        txt += "\nIntelligence: " + intelligence;
        txt += "\nLuck: " + luck;
        if (parent != null) txt += "\nParent: " + parent.name;
        if (partner != null)
        {
            if (partner.sex == 'f') txt += "\nWife: " + partner.name;
            else txt += "\nHusband: " + partner.name;
        }
        if (children.Count != 0)
        {
            if (((Member)children[0]).sex == 'm') txt += "\nSon: ";
            else txt += "\nDaughter: ";
            txt += ((Member)children[0]).name;
        }
        txt += "\nGeneration: " + generation;
        return txt;
    }
    public int Get_Age(DateTime d)
    {
        int age = d.Year - birthday.Year;
        if (d.Month - birthday.Month < 0) age -= 1;
        else if (d.Month - birthday.Month == 0)
        {
            if (d.Day - birthday.Day < 0) age -= 1;
        }
        return age;
    }
    public void Give_Birth(DateTime d, ArrayList reg)
    {
        char sx;
        string nm = "";
        if (rnd.Next(2) == 0)
        {
            sx = 'm';
            nm = m_names[rnd.Next(m_names.Length)];
        }
        else
        {
            sx = 'f';
            nm = f_names[rnd.Next(f_names.Length)];
        }
        int ind = reg.Count;
        Member member = new Member(nm, d.Day, d.Month, d.Year, sx, this, 1, generation + 1, children.Count + 1, ind);
        member.costs = base_costs[0];
        children.Add(member);
        partner.children.Add(member);
        reg.Add(member);
    }
    public void Get_Married(ArrayList reg)
    {
        char sx;
        string nm = "";
        if (sex == 'm')
        {
            sx = 'f';
            nm = f_names[rnd.Next(f_names.Length)];
        }
        else
        {
            sx = 'm';
            nm = m_names[rnd.Next(m_names.Length)];
        }
        int ind = reg.Count;
        DateTime birth = birthday.AddDays(rnd.Next(-2000, 2000));
        partner = new Member(nm, birth.Day, birth.Month, birth.Year, sx, null, 0, generation, number, ind);
        reg.Add(partner);
        string[] eds = new string[] { "Public", "Private" };
        int jobnum = rnd.Next(5);
        partner.primary_ed = eds[rnd.Next(1)];
        partner.secondary_ed = eds[rnd.Next(1)];
        partner.higher_ed = "No";
        partner.specialization = "No";
        partner.career_status = ((string[])specialities["General works"])[jobnum];
        partner.unspec_levels[jobnum] = rnd.Next(41);
        partner.income = 300 + (float)partner.unspec_levels[jobnum] * 300 / 100;
        partner.costs = base_costs[2];
        partner.partner = this;
    }
    public static void Init()
    {
        specialities.Add("Journalism", new string[] { "Trainee", "Reporter", "Reviewer", "Executive secretary", "Chief editor" });
        specialities.Add("Art design", new string[] { "Trainee", "Artist", "Lead artist", "Art director", "Creative director" });
        specialities.Add("Music", new string[] { "Trainee", "Musician", "Soundman", "Arranger", "Composer" });
        specialities.Add("Economics", new string[] { "Trainee", "Accountant", "Economist", "Lead economist", "Financial director" });
        specialities.Add("Law", new string[] { "Trainee", "Lawyer", "Consultant", "Notary", "Judge" });
        specialities.Add("Management", new string[] { "Cashier", "Manager", "Supervisor", "Top manager", "CEO" });
        specialities.Add("Science", new string[] { "Assistant", "Researcher", "Lead researcher", "Professor", "Rector" });
        specialities.Add("Engineering", new string[] { "Trainee", "Engineer", "Lead engineer", "Head engineer", "Technical director" });
        specialities.Add("Medicine", new string[] { "Interne", "Doctor", "Lead doctor", "Chief doctor", "Hospital director" });
        specialities.Add("Programming", new string[] { "Trainee", "Junior", "Middle", "Senior", "IT director" });
        specialities.Add("Cooking", new string[] { "Trainee", "Cook", "Head cook", "Sous-chef", "Chef" });
        specialities.Add("Police", new string[] { "Trainee", "Patrolman", "Detective", "Captain", "Chief" });
        specialities.Add("Soldiery", new string[] { "Soldier", "Sergeant", "Officer", "Commander", "General" });
        specialities.Add("General works", new string[] { "Janitor", "Plumber", "Electrician", "Builder", "Driver" });
    }
}

class Family
{
    public string surname;
    public Member core_member;
    public float budget;
    public ArrayList register = new ArrayList();
    public ArrayList descendants = new ArrayList();
    public Family(string srn, float bgt)
    {
        surname = srn;
        budget = bgt;
    }
    public void DefineDescendants(Member m)
    {
        if (m.children.Count == 0)
        {
            descendants.Add(m);
        }
        else
        {
            foreach (Member v in m.children)
            {
                DefineDescendants(v);
            }
        }
    }
    public void SetParentsCoords(Member m)
    {
        float x1 = 0;
        float x2 = 0;
        Member current;
        foreach (Member v in m.children)
        {
            if (!descendants.Contains(v))
            {
                current = v;
                while (current.children.Count != 0)
                {
                    current = (Member)current.children[0];
                }
                x1 = current.coords[0];
                if (current.partner != null) x1 += 60;
                current = v;
                while (current.children.Count != 0)
                {
                    current = (Member)current.children[current.children.Count - 1];
                }
                x2 = current.coords[0];
                if (current.partner != null) x2 += 60;
                v.coords[0] = (x1 + x2) / 2 - 60;
                v.coords[1] = core_member.coords[1] + 120 * (v.generation - 1);
                v.partner.coords[0] = v.coords[0] + 120;
                v.partner.coords[1] = v.coords[1];
                SetParentsCoords(v);
            }
        }
    }
}

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

class MyForm : Form
{
    public MainMenu mMain;
    public MenuItem mProg;
    public MenuItem mFamily;

    public Panel panGraphics;
    public Panel panBio;
    public Panel panSettings;
    public Panel panButtons;
    public Panel panDate;
    public Panel panBudget;
    public Panel panZoom;

    public Button bPause;
    public Button bPlay;
    public Button bX2;
    public Button bX4;

    public Label lDate;
    public Label lPlay;
    public Label lBudget, lInc;
    public Label lZoom;
    public Label lPhoto;
    public Label lNameAge;
    public Label lSex;
    public Label lBirth, lDeath;
    public Label lHealth, lCharisma, lIntel, lLuck;
    public Label lParents, lPartner, lChildren;
    public Label lPEd, lSEd, lHEd, lSpec;
    public Label lHStat, lCStat;
    public Label lIncome;
    public Label lGeneration;

    public PictureBox picbox;

    public TrackBar tbarZoom;

    public Font fBio = new Font("Arial", 11, FontStyle.Regular);
    public DateTime date = new DateTime(2000, 1, 1);
    public int timer = 0;
    public Family family = null;
    public Member cur_mbr = null;
    public float[] map_coords = new float[] { 0, 0 };
    public Thread thread;

    public bool mouse_clicked = false;
    public float[] mouse_coords = new float[] { 0, 0 };
    public float[] old_coords = new float[] { 0, 0 };

    public Image imgMale = Image.FromFile("Images/male_frame.png");
    public Image imgFemale = Image.FromFile("Images/female_frame.png");
    public Image imgMale2 = Image.FromFile("Images/male_photo.png");
    public Image imgFemale2 = Image.FromFile("Images/female_photo.png");

    public MyForm() : base()
    {
        Size = new Size(1400, 800);
        FormBorderStyle = FormBorderStyle.Fixed3D;
        StartPosition = FormStartPosition.CenterScreen;
        MaximizeBox = false;
        Text = "Member Tree";
        Font = new Font("Arial", 14, FontStyle.Bold);

        mMain = new MainMenu();
        mProg = new MenuItem("Программа");
        mProg.MenuItems.Add("О программе", ShowMessage);
        mProg.MenuItems.Add("Выход", CloseApp);
        mMain.MenuItems.Add(mProg);
        mFamily = new MenuItem("Семья");
        mFamily.MenuItems.Add("Создать", CreateFamily);
        mFamily.MenuItems.Add("Сохранить", SaveFamily);
        mFamily.MenuItems.Add("Загрузить", LoadFamily);
        mMain.MenuItems.Add(mFamily);
        Menu = mMain;

        panGraphics = new Panel();
        panGraphics.SetBounds(5, 5, ClientSize.Width * 15 / 20, ClientSize.Height - 110);
        panGraphics.BorderStyle = BorderStyle.Fixed3D;
        Controls.Add(panGraphics);
        picbox = new PictureBox();
        picbox.SetBounds(0, 0, panGraphics.Width, panGraphics.Height);
        panGraphics.Controls.Add(picbox);

        panBio = new Panel();
        panBio.SetBounds(panGraphics.Right + 5, panGraphics.Top, ClientSize.Width - panGraphics.Width - 10, ClientSize.Height - 25);
        panBio.BorderStyle = BorderStyle.Fixed3D;
        Controls.Add(panBio);
        lPhoto = new Label();
        lPhoto.SetBounds(panBio.Width / 2 - 50, 15, 100, 100);
        lPhoto.Image = imgMale;
        panBio.Controls.Add(lPhoto);
        lNameAge = new Label();
        lNameAge.Left = 0;
        lNameAge.Top = lPhoto.Bottom + 10;
        lNameAge.Width = panBio.Width;
        lNameAge.Text = "Timothy, 26";
        lNameAge.TextAlign = ContentAlignment.MiddleCenter;
        panBio.Controls.Add(lNameAge);
        lSex = new Label();
        lSex.Left = 10;
        lSex.Top = lNameAge.Bottom + 50;
        lSex.Width = panBio.Width - 20;
        lSex.Text = "Sex: Male";
        lSex.Font = fBio;
        panBio.Controls.Add(lSex);
        lBirth = new Label();
        lBirth.Left = lSex.Left;
        lBirth.Top = lSex.Bottom + 5;
        lBirth.Width = lSex.Width;
        lBirth.Text = "Birthday: 05.02.1998";
        lBirth.Font = fBio;
        panBio.Controls.Add(lBirth);
        lDeath = new Label();
        lDeath.Left = lSex.Left;
        lDeath.Top = lBirth.Bottom + 5;
        lDeath.Width = lSex.Width;
        lDeath.Text = "Date of death: 13.12.2077";
        lDeath.Font = fBio;
        panBio.Controls.Add(lDeath);
        lHealth = new Label();
        lHealth.Left = lSex.Left;
        lHealth.Top = lDeath.Bottom + 5;
        lHealth.Width = lSex.Width / 2;
        lHealth.Text = "Health: 100";
        lHealth.Font = fBio;
        lHealth.TextAlign = ContentAlignment.MiddleRight;
        panBio.Controls.Add(lHealth);
        lCharisma = new Label();
        lCharisma.Left = lHealth.Right;
        lCharisma.Top = lHealth.Top;
        lCharisma.Width = lHealth.Width;
        lCharisma.Text = "Charisma: 100";
        lCharisma.Font = fBio;
        lCharisma.TextAlign = ContentAlignment.MiddleRight;
        panBio.Controls.Add(lCharisma);
        lIntel = new Label();
        lIntel.Left = lSex.Left;
        lIntel.Top = lHealth.Bottom + 5;
        lIntel.Width = lHealth.Width;
        lIntel.Text = "Intelligence: 100";
        lIntel.Font = fBio;
        lIntel.TextAlign = ContentAlignment.MiddleRight;
        panBio.Controls.Add(lIntel);
        lLuck = new Label();
        lLuck.Left = lIntel.Right;
        lLuck.Top = lIntel.Top;
        lLuck.Width = lHealth.Width;
        lLuck.Text = "Luck: 100";
        lLuck.Font = fBio;
        lLuck.TextAlign = ContentAlignment.MiddleRight;
        panBio.Controls.Add(lLuck);
        lParents = new Label();
        lParents.Left = lSex.Left;
        lParents.Top = lIntel.Bottom + 5;
        lParents.Width = lSex.Width;
        lParents.Text = "Parents: Alla, 52 Alex, 47";
        lParents.Font = fBio;
        panBio.Controls.Add(lParents);
        lPartner = new Label();
        lPartner.Left = lSex.Left;
        lPartner.Top = lParents.Bottom + 5;
        lPartner.Width = lSex.Width;
        lPartner.Text = "Wife: Lilya, 29";
        lPartner.Font = fBio;
        panBio.Controls.Add(lPartner);
        lChildren = new Label();
        lChildren.Left = lSex.Left;
        lChildren.Top = lPartner.Bottom + 5;
        lChildren.Width = lSex.Width;
        lChildren.Text = "Children: No";
        lChildren.Font = fBio;
        panBio.Controls.Add(lChildren);
        lPEd = new Label();
        lPEd.Left = lSex.Left;
        lPEd.Top = lChildren.Bottom + 5;
        lPEd.Width = lSex.Width;
        lPEd.Text = "Primary education: Public, 2005-2009";
        lPEd.Font = fBio;
        panBio.Controls.Add(lPEd);
        lSEd = new Label();
        lSEd.Left = lSex.Left;
        lSEd.Top = lPEd.Bottom + 5;
        lSEd.Width = lSex.Width;
        lSEd.Text = "Secondary education: Public, 2009-2016";
        lSEd.Font = fBio;
        panBio.Controls.Add(lSEd);
        lHEd = new Label();
        lHEd.Left = lSex.Left;
        lHEd.Top = lSEd.Bottom + 5;
        lHEd.Width = lSex.Width;
        lHEd.Text = "Higher education: Public, 2016-2022";
        lHEd.Font = fBio;
        panBio.Controls.Add(lHEd);
        lSpec = new Label();
        lSpec.Left = lSex.Left;
        lSpec.Top = lHEd.Bottom + 5;
        lSpec.Width = lSex.Width;
        lSpec.Text = "Specialization: Engineering - 25";
        lSpec.Font = fBio;
        panBio.Controls.Add(lSpec);
        lCStat = new Label();
        lCStat.Left = lSex.Left;
        lCStat.Top = lSpec.Bottom + 5;
        lCStat.Width = lSex.Width;
        lCStat.Text = "Occupation: Engineer";
        lCStat.Font = fBio;
        panBio.Controls.Add(lCStat);
        lIncome = new Label();
        lIncome.Left = lSex.Left;
        lIncome.Top = lCStat.Bottom + 5;
        lIncome.Width = lSex.Width;
        lIncome.Text = "Income: +$555.55";
        lIncome.Font = fBio;
        panBio.Controls.Add(lIncome);
        lHStat = new Label();
        lHStat.Left = lSex.Left;
        lHStat.Top = lIncome.Bottom + 5;
        lHStat.Width = lSex.Width;
        lHStat.Text = "Status: Healthy";
        lHStat.Font = fBio;
        panBio.Controls.Add(lHStat);
        lGeneration = new Label();
        lGeneration.Left = lSex.Left;
        lGeneration.Top = lHStat.Bottom + 5;
        lGeneration.Width = lSex.Width;
        lGeneration.Text = "Generation: 1";
        lGeneration.Font = fBio;
        panBio.Controls.Add(lGeneration);

        panSettings = new Panel();
        panSettings.SetBounds(panGraphics.Left, panGraphics.Bottom + 5, panGraphics.Width, ClientSize.Height - panGraphics.Height - 30);
        panSettings.BorderStyle = BorderStyle.Fixed3D;
        Controls.Add(panSettings);
        panButtons = new Panel();
        panButtons.SetBounds(5, 5, 230, panSettings.Height - 15);
        panButtons.BorderStyle = BorderStyle.Fixed3D;
        panSettings.Controls.Add(panButtons);
        bPause = new Button();
        bPause.SetBounds(5, 5, 50, 50);
        bPause.Text = "| |";
        panButtons.Controls.Add(bPause);
        bPlay = new Button();
        bPlay.SetBounds(bPause.Right + 5, bPause.Top, bPause.Width, bPause.Height);
        bPlay.Text = ">";
        panButtons.Controls.Add(bPlay);
        bX2 = new Button();
        bX2.SetBounds(bPlay.Right + 5, bPause.Top, bPause.Width, bPause.Height);
        bX2.Text = ">>";
        panButtons.Controls.Add(bX2);
        bX4 = new Button();
        bX4.SetBounds(bX2.Right + 5, bPause.Top, bPause.Width, bPause.Height);
        bX4.Text = ">>>";
        panButtons.Controls.Add(bX4);
        panDate = new Panel();
        panDate.SetBounds(panButtons.Right + 5, panButtons.Top, 200, panButtons.Height);
        panDate.BorderStyle = BorderStyle.Fixed3D;
        panSettings.Controls.Add(panDate);
        lPlay = new Label();
        lPlay.Left = 20;
        lPlay.Top = 15;
        lPlay.Width = 40;
        lPlay.Text = "| |";
        panDate.Controls.Add(lPlay);
        lDate = new Label();
        lDate.Left = lPlay.Right + 20;
        lDate.Top = lPlay.Top;
        lDate.Width = 150;
        lDate.Text = date.ToString("dd.MM.yyyy");
        panDate.Controls.Add(lDate);
        panBudget = new Panel();
        panBudget.SetBounds(panDate.Right + 5, panButtons.Top, 380, panButtons.Height);
        panBudget.BorderStyle = BorderStyle.Fixed3D;
        panSettings.Controls.Add(panBudget);
        lBudget = new Label();
        lBudget.Left = 10;
        lBudget.Top = 15;
        lBudget.Width = 120;
        lBudget.Text = "$5555.55M";
        panBudget.Controls.Add(lBudget);
        lInc = new Label();
        lInc.Left = lBudget.Right + 10;
        lInc.Top = lBudget.Top;
        lInc.Width = 120;
        lInc.Text = "+$5555.55k";
        panBudget.Controls.Add(lInc);
        panZoom = new Panel();
        panZoom.SetBounds(panBudget.Right + 5, panButtons.Top, 200, panButtons.Height);
        panZoom.BorderStyle = BorderStyle.Fixed3D;
        panSettings.Controls.Add(panZoom);
        tbarZoom = new TrackBar();
        tbarZoom.Left = 5;
        tbarZoom.Top = 15;
        tbarZoom.Width = 130;
        tbarZoom.Minimum = 50;
        tbarZoom.Maximum = 200;
        tbarZoom.TickFrequency = 25;
        tbarZoom.LargeChange = 25;
        tbarZoom.SmallChange = 25;
        tbarZoom.Value = 100;
        panZoom.Controls.Add(tbarZoom);
        lZoom = new Label();
        lZoom.Left = tbarZoom.Right + 5;
        lZoom.Top = tbarZoom.Top;
        lZoom.Text = "" + tbarZoom.Value + "%";
        panZoom.Controls.Add(lZoom);

        map_coords[0] = 500;
        map_coords[1] = 10;
        thread = new Thread(Advance);

        Load += (obj, ea) => {
            thread.Start();
        };
        FormClosing += (obj, ea) => {
            thread.Abort();
        };

        picbox.Paint += (obj, ea) => {
            Render(ea.Graphics);
        };
        picbox.MouseMove += (obj, ea) => {
            if (ea.Button == MouseButtons.Left)
            {
                if (!mouse_clicked)
                {
                    mouse_clicked = true;
                    mouse_coords[0] = ea.X;
                    mouse_coords[1] = ea.Y;
                    old_coords[0] = map_coords[0];
                    old_coords[1] = map_coords[1];
                }
                else
                {
                    map_coords[0] = ea.X - mouse_coords[0] + old_coords[0];
                    map_coords[1] = ea.Y - mouse_coords[1] + old_coords[1];
                }
            }
            else mouse_clicked = false;
        };
        picbox.MouseDown += (obj, ea) => {
            Member m = null;
            float s = (float)tbarZoom.Value / 100;
            ContextMenuStrip cmenu = new ContextMenuStrip();
            foreach (Member v in family.register)
            {
                if ((ea.X - map_coords[0] * s >= (v.coords[0] - 25) * s && ea.X - map_coords[0] * s <= (v.coords[0] + 25) * s) &&
                    (ea.Y - map_coords[1] * s >= (v.coords[1] - 25) * s && ea.Y - map_coords[1] * s <= (v.coords[1] + 25) * s))
                {
                    m = v;
                    break;
                }
            }
            if (m != null) cur_mbr = m;
            if (ea.Button == MouseButtons.Left && m != null)
            {
                FillBio();
            }
            if (ea.Button == MouseButtons.Right && (m != null && m.kin != 0))
            {
                ToolStripMenuItem m1 = new ToolStripMenuItem("Marry " + cur_mbr.name);
                ToolStripMenuItem m2 = new ToolStripMenuItem("Give birth by " + cur_mbr.name);
                if (cur_mbr.partner != null) m1.Enabled = false;
                else m2.Enabled = false;
                m1.Click += (a, b) => {
                    Marry();
                };
                m2.Click += (a, b) => {
                    GiveBirth();
                };
                cmenu.Items.Add(m1);
                cmenu.Items.Add(m2);
                cmenu.Show(picbox, ea.X, ea.Y);
            }
        };

        bPause.Click += (obj, ea) => {
            lPlay.Text = "| |";
        };
        bPlay.Click += (obj, ea) => {
            lPlay.Text = "x1";
        };
        bX2.Click += (obj, ea) => {
            lPlay.Text = "x4";
        };
        bX4.Click += (obj, ea) => {
            lPlay.Text = "x16";
        };

        tbarZoom.Scroll += (obj, ea) => {
            tbarZoom.Value = tbarZoom.Value / 25 * 25;
            lZoom.Text = "" + tbarZoom.Value + "%";
        };
    }

    public void Advance()
    {
        while (true)
        {
            switch (lPlay.Text)
            {
                case "x1":
                    timer += 1;
                    break;
                case "x4":
                    timer += 4;
                    break;
                case "x16":
                    timer += 16;
                    break;
            }
            if (timer >= 40)
            {
                float inc = 0;
                string budg = "";
                timer = 0;
                date = date.AddDays(1);
                lDate.Text = date.ToString("dd.MM.yyyy");
                if (family != null)
                {
                    foreach (Member v in family.register)
                    {
                        inc += v.income;
                        inc -= v.costs;
                        EventMaster.PlayMarriage(v, family.register, date);
                    }
                    if (inc >= 0)
                    {
                        lInc.Text = "+";
                        lInc.ForeColor = Color.Green;
                    }
                    else
                    {
                        lInc.Text = "-";
                        lInc.ForeColor = Color.Red;
                    }
                    lInc.Text += "$" + Math.Abs(inc);
                    family.budget += inc;
                    budg = "$" + Math.Abs(family.budget);
                    if (family.budget > 9999) budg = "$" + Math.Abs(Math.Round(family.budget / 1000, 2)) + "k";
                    if (family.budget / 1000 > 9999) budg = "$" + Math.Abs(Math.Round(family.budget / 1000000, 2)) + "M";
                    if (family.budget / 1000000 > 9999) budg = "$" + Math.Abs(Math.Round(family.budget / 1000000000, 2)) + "B";
                    if (family.budget < 0) lBudget.Text = "-" + budg;
                    else lBudget.Text = budg;
                }
            }
            picbox.Invalidate();
            Thread.Sleep(20);
        }
    }
    public void Render(Graphics g)
    {
        g.Clear(Color.White);
        System.Drawing.Drawing2D.GraphicsState state = g.Save();
        g.ScaleTransform((float)tbarZoom.Value / 100, (float)tbarZoom.Value / 100);
        if (family != null)
        {
            g.DrawString(family.surname + " Family", new Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Blue),
                map_coords[0] - (family.surname + " Family").Length / 2 * 12, map_coords[1]);
            for (int i = 0; i < family.register.Count; i++)
            {
                Member m = (Member)family.register[i];
                DrawBond(g, m);
                DrawMember(g, m);
            }
        }
        g.Restore(state);
    }
    public void FillBio()
    {
        if (cur_mbr.sex == 'm') lPhoto.Image = imgMale;
        else lPhoto.Image = imgFemale;
        lNameAge.Text = cur_mbr.name + ", " + cur_mbr.Get_Age(date);
        if (cur_mbr.sex == 'm') lSex.Text = "Sex: Male";
        else lSex.Text = "Sex: Female";
        lBirth.Text = "Date of birth: " + cur_mbr.birthday.ToString("dd.MM.yyyy");
        lDeath.Text = "Date of death: ";
        if (cur_mbr.health_status == "Dead") lDeath.Text += cur_mbr.deathday.ToString("dd.MM.yyyy");
        else lDeath.Text += "-";
        lHealth.Text = "Health: " + cur_mbr.health;
        lCharisma.Text = "Charisma: " + cur_mbr.charisma;
        lIntel.Text = "Intelligence: " + cur_mbr.intelligence;
        lLuck.Text = "Luck: " + cur_mbr.luck;
        if (cur_mbr.parent != null)
        {
            lParents.Text = "Parents: " + cur_mbr.parent.name + ", " + cur_mbr.parent.Get_Age(date);
            lParents.Text += " " + cur_mbr.parent.partner.name + ", " + cur_mbr.parent.partner.Get_Age(date);
        }
        else lParents.Text = "Parents: No";
        if (cur_mbr.partner != null)
        {
            if (cur_mbr.sex == 'm') lPartner.Text = "Wife: " + cur_mbr.partner.name + ", " + cur_mbr.partner.Get_Age(date);
            else lPartner.Text = "Husband: " + cur_mbr.partner.name + ", " + cur_mbr.partner.Get_Age(date);
        }
        else lPartner.Text = "Partner: No";
        lChildren.Text = "Children: ";
        if (cur_mbr.children.Count != 0)
        {
            foreach (Member v in cur_mbr.children)
            {
                lChildren.Text += v.name + ", " + v.Get_Age(date) + " ";
            }
        }
        else lChildren.Text += "No";
        lPEd.Text = "Primary education: " + cur_mbr.primary_ed;
        if (cur_mbr.primary_ed != "No") lPEd.Text += ", " + (cur_mbr.birthday.Year + 7) + "-" + (cur_mbr.birthday.Year + 11);
        lSEd.Text = "Secondary education: " + cur_mbr.secondary_ed;
        if (cur_mbr.secondary_ed != "No") lSEd.Text += ", " + (cur_mbr.birthday.Year + 11) + "-" + (cur_mbr.birthday.Year + 18);
        lHEd.Text = "Higher education: " + cur_mbr.higher_ed;
        if (cur_mbr.higher_ed != "No") lHEd.Text += ", " + (cur_mbr.birthday.Year + 18) + "-" + (cur_mbr.birthday.Year + 24);
        if (cur_mbr.specialization == "No") lSpec.Text = "Specialization: No";
        else lSpec.Text = "Specialization(Level): " + cur_mbr.specialization + "(" + cur_mbr.spec_level + ")";
        lCStat.Text = "Occupation: " + cur_mbr.career_status;
        lIncome.Text = "Income: ";
        if (cur_mbr.income - cur_mbr.costs >= 0) lIncome.Text += "+";
        else lIncome.Text += "-";
        lIncome.Text += "$" + Math.Abs(cur_mbr.income - cur_mbr.costs);
        lHStat.Text = "Status: " + cur_mbr.health_status;
        lGeneration.Text = "id: " + cur_mbr.kin + "." + cur_mbr.generation + "." + cur_mbr.number + "." + cur_mbr.id;
    }
    public void DrawMember(Graphics g, Member mbr)
    {
        if (mbr.sex == 'm')
        {
            g.DrawImage(imgMale2, mbr.coords[0] - 25 + map_coords[0], mbr.coords[1] - 25 + map_coords[1]);
        }
        else
        {
            g.DrawImage(imgFemale2, mbr.coords[0] - 25 + map_coords[0], mbr.coords[1] - 25 + map_coords[1]);
        }
        g.DrawString(mbr.name + ", " + mbr.Get_Age(date), new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.Black),
                    mbr.coords[0] - (mbr.name + ", " + mbr.Get_Age(date)).Length / 2 * 8 + map_coords[0], mbr.coords[1] + 25 + map_coords[1]);
    }
    public void DrawBond(Graphics g, Member m)
    {
        float x1 = 0;
        float x2 = 0;
        if (m.kin == 1)
        {
            if (m.partner != null)
            {
                g.DrawLine(new Pen(Color.Red, 6), m.coords[0] + map_coords[0], m.coords[1] - 3 + map_coords[1],
                            m.partner.coords[0] + map_coords[0], m.partner.coords[1] - 3 + map_coords[1]);
            }
            if (m.children.Count != 0)
            {
                g.DrawLine(new Pen(Color.Red, 6), m.coords[0] + 56 + map_coords[0], m.coords[1] + map_coords[1],
                            m.coords[0] + 56 + map_coords[0], m.coords[1] + 60 + map_coords[1]);
                if (((Member)m.children[0]).partner != null) x1 = ((Member)m.children[0]).coords[0] + 60;
                else x1 = ((Member)m.children[0]).coords[0];
                if (((Member)m.children[m.children.Count - 1]).partner != null) x2 = ((Member)m.children[m.children.Count - 1]).coords[0] + 60;
                else x2 = ((Member)m.children[m.children.Count - 1]).coords[0];
                g.DrawLine(new Pen(Color.Red, 6), x1 - 7 + map_coords[0], m.coords[1] + 60 + map_coords[1],
                            x2 - 1 + map_coords[0], m.coords[1] + 60 + map_coords[1]);
                foreach (Member v in m.children)
                {
                    if (v.partner != null)
                    {
                        g.DrawLine(new Pen(Color.Red, 6), v.coords[0] + 60 - 4 + map_coords[0], v.coords[1] - 60 + map_coords[1],
                            v.coords[0] + 60 - 4 + map_coords[0], v.coords[1] + map_coords[1]);
                    }
                    else
                    {
                        g.DrawLine(new Pen(Color.Red, 6), v.coords[0] - 4 + map_coords[0], v.coords[1] - 60 + map_coords[1],
                            v.coords[0] - 4 + map_coords[0], v.coords[1] + map_coords[1]);
                    }
                }
            }
        }
    }
    public void Marry(Member m)
    {
        m.Get_Married(family.register);
        m.coords[0] -= 60;
        m.partner.coords[0] = m.coords[0] + 120;
        m.partner.coords[1] = m.coords[1];
    }
    public void Marry()
    {
        cur_mbr.Get_Married(family.register);
        cur_mbr.coords[0] -= 60;
        cur_mbr.partner.coords[0] = cur_mbr.coords[0] + 120;
        cur_mbr.partner.coords[1] = cur_mbr.coords[1];
    }
    public void GiveBirth()
    {
        cur_mbr.Give_Birth(date, family.register);
        SetPositions();
    }
    public void SetPositions()
    {
        family.descendants.Clear();
        family.DefineDescendants(family.core_member);
        foreach (Member v in family.descendants)
        {
            v.coords[0] = family.core_member.coords[0] + 60 - 120 * (family.descendants.Count - 1) + 240 * (family.descendants.IndexOf(v));
            v.coords[1] = family.core_member.coords[1] + 120 * (v.generation - 1);
            if (v.partner != null)
            {
                v.coords[0] -= 60;
                v.partner.coords[0] = v.coords[0] + 120;
                v.partner.coords[1] = v.coords[1];
            }
        }
        family.SetParentsCoords(family.core_member);
    }
    public void CreateFamily(object obj, EventArgs ea)
    {
        bPause.PerformClick();
        Form form = new Form();
        form.Size = new Size(400, 250);
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = "Создание семьи";
        form.Font = new Font("Arial", 12, FontStyle.Regular);
        Label lSrn = new Label();
        lSrn.SetBounds(10, 10, 80, 20);
        lSrn.Text = "Surname:";
        form.Controls.Add(lSrn);
        Label lNm = new Label();
        lNm.Left = lSrn.Left;
        lNm.Top = lSrn.Bottom + 15;
        lNm.Size = lSrn.Size;
        lNm.Text = "Name:";
        form.Controls.Add(lNm);
        Label lS = new Label();
        lS.Left = lSrn.Left;
        lS.Top = lNm.Bottom + 15;
        lS.Size = lSrn.Size;
        lS.Text = "Sex:";
        form.Controls.Add(lS);
        Label lB = new Label();
        lB.Left = lSrn.Left;
        lB.Top = lS.Bottom + 15;
        lB.Size = lSrn.Size;
        lB.Text = "Birthday:";
        form.Controls.Add(lB);
        TextBox tbSrn = new TextBox();
        tbSrn.Left = lSrn.Right + 10;
        tbSrn.Top = lSrn.Top;
        tbSrn.Width = form.ClientSize.Width - lSrn.Width - 30;
        tbSrn.Height = lSrn.Height;
        form.Controls.Add(tbSrn);
        TextBox tbNm = new TextBox();
        tbNm.Left = tbSrn.Left;
        tbNm.Top = lNm.Top;
        tbNm.Size = tbSrn.Size;
        form.Controls.Add(tbNm);
        ComboBox cbSex = new ComboBox();
        cbSex.Left = tbSrn.Left;
        cbSex.Top = lS.Top;
        cbSex.DropDownStyle = ComboBoxStyle.DropDownList;
        cbSex.Items.Add("Male");
        cbSex.Items.Add("Female");
        form.Controls.Add(cbSex);
        ComboBox cbMonth = new ComboBox();
        cbMonth.Left = tbSrn.Left;
        cbMonth.Top = lB.Top;
        cbMonth.DropDownStyle = ComboBoxStyle.DropDownList;
        cbMonth.Items.AddRange(new string[]{"January","February","March","April","May","June",
            "July","August","September","October","November","December"});
        form.Controls.Add(cbMonth);
        ComboBox cbDay = new ComboBox();
        cbDay.Left = cbMonth.Right + 10;
        cbDay.Top = lB.Top;
        cbDay.DropDownStyle = ComboBoxStyle.DropDownList;
        for (int i = 1; i < 32; i++)
        {
            cbDay.Items.Add(i);
        }
        form.Controls.Add(cbDay);
        Button bConf = new Button();
        bConf.Left = form.ClientSize.Width / 2 - 85;
        bConf.Top = lB.Bottom + 35;
        bConf.Width = 80;
        bConf.Height = 40;
        bConf.Text = "Confirm";
        form.Controls.Add(bConf);
        Button bCanc = new Button();
        bCanc.Left = bConf.Right + 10;
        bCanc.Top = bConf.Top;
        bCanc.Size = bConf.Size;
        bCanc.Text = "Cancel";
        form.Controls.Add(bCanc);
        cbMonth.SelectedIndexChanged += (a, b) => {
            int days = 0;
            if ((string)cbMonth.SelectedItem == "February") days = 28;
            else if ((string)cbMonth.SelectedItem == "April" || (string)cbMonth.SelectedItem == "June"
                || (string)cbMonth.SelectedItem == "September" || (string)cbMonth.SelectedItem == "November") days = 30;
            else days = 31;
            cbDay.Items.Clear();
            for (int i = 1; i <= days; i++)
            {
                cbDay.Items.Add(i);
            }
        };
        bConf.Click += (a, b) => {
            if (tbSrn.Text == "" || tbNm.Text == "" || cbSex.SelectedItem == null
                || cbDay.SelectedItem == null || cbMonth.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "ВНИМАНИЕ!",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                char x = 'm';
                if ((string)cbSex.SelectedItem == "Female") x = 'f';
                family = new Family(tbSrn.Text, 1000);
                family.core_member = new Member(tbNm.Text, (int)cbDay.SelectedItem, (int)cbMonth.SelectedIndex + 1, 1975, x, null, 1, 1, 1, 0);
                family.register.Add(family.core_member);
                family.core_member.coords[0] = 0;
                family.core_member.coords[1] = 100;
                family.core_member.primary_ed = "Public";
                family.core_member.secondary_ed = "Public";
                family.core_member.higher_ed = "Public";
                family.core_member.specialization = Member.spec_names[Member.rnd.Next(Member.spec_names.Length)];
                family.core_member.spec_level = 30;
                family.core_member.career_status = ((string[])Member.specialities[family.core_member.specialization])[1];
                family.core_member.income = Member.wages[1];
                family.core_member.costs = Member.base_costs[2];
                cur_mbr = family.core_member;
                FillBio();
                lBudget.Text = "$" + family.budget;
                lInc.Text = "+$0";
                date = new DateTime(2000, 1, 1);
                lDate.Text = date.ToString("dd.MM.yyyy");
                form.Close();
            }
        };
        bCanc.Click += (a, b) => {
            form.Close();
        };
        form.ShowDialog();
    }
    public void SaveFamily(object obj, EventArgs ea)
    {
        //SaveFileDialog sd=new SaveFileDialog();
        //sd.Filter="XAML files (*.xml)|*.xml";
        //if(sd.ShowDialog()!=DialogResult.OK) return;
        //string fname=sd.FileName;
        //Console.WriteLine(fname);

        if (family == null) return;
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("Family");
        doc.AppendChild(root);
        XmlElement xDate = doc.CreateElement("Date");
        xDate.InnerText = date.ToString("dd.MM.yyyy");
        root.AppendChild(xDate);
        XmlElement xSrn = doc.CreateElement("Surname");
        xSrn.InnerText = family.surname;
        root.AppendChild(xSrn);
        XmlElement xBudg = doc.CreateElement("Budget");
        xBudg.InnerText = "" + family.budget;
        root.AppendChild(xBudg);
        XmlElement xCore = doc.CreateElement("Core_member");
        root.AppendChild(xCore);
        XmlElement xMem = CreateXmlMember(xCore, doc, family.core_member);
        if (family.core_member.partner != null)
        {
            XmlElement xPart = doc.CreateElement("Partner");
            xMem.AppendChild(xPart);
            CreateXmlMember(xPart, doc, family.core_member.partner);
        }
        if (family.core_member.children.Count != 0)
        {
            XmlElement xChildren = doc.CreateElement("Children");
            xMem.AppendChild(xChildren);
            CreateXmlChildren(xChildren, doc, family.core_member);
        }
        doc.Save("Save Files/" + family.surname + ".xml");

    }
    public void LoadFamily(object obj, EventArgs ea)
    {
        string[] files = Directory.GetFileSystemEntries("Save Files");
        Form form = new Form();
        form.Size = new Size(300, 250);
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.StartPosition = FormStartPosition.CenterScreen;
        form.Text = "Загрузка семьи";
        form.Font = new Font("Arial", 12, FontStyle.Regular);
        ListBox lb = new ListBox();
        lb.SetBounds(10, 10, form.ClientSize.Width - 10, 150);
        lb.Items.AddRange(files);
        form.Controls.Add(lb);
        Button bChoose = new Button();
        bChoose.Left = form.ClientSize.Width / 2 - 100;
        bChoose.Top = lb.Bottom + 10;
        bChoose.Width = 90;
        bChoose.Height = 30;
        bChoose.Text = "Выбрать";
        form.Controls.Add(bChoose);
        Button bCancel = new Button();
        bCancel.Left = bChoose.Right + 20;
        bCancel.Top = bChoose.Top;
        bCancel.Size = bChoose.Size;
        bCancel.Text = "Отмена";
        form.Controls.Add(bCancel);
        bChoose.Click += (a, b) => {
            if (lb.SelectedItem != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(lb.SelectedItem.ToString());
                XmlElement root = doc.DocumentElement;
                XmlElement dt = root["Date"];
                XmlElement sn = root["Surname"];
                XmlElement bg = root["Budget"];
                date = new DateTime(Int32.Parse(dt.InnerText.Split('.')[2]),
                    Int32.Parse(dt.InnerText.Split('.')[1]),
                    Int32.Parse(dt.InnerText.Split('.')[0]));
                family = new Family(sn.InnerText, Single.Parse(bg.InnerText));
                XmlElement cm = root["Core_member"];
                XmlElement cmm = cm["Member"];
                family.core_member = new Member();
                CreateMemberFromXml(family.core_member, cmm, family.register);
                family.core_member.coords[0] = 0;
                family.core_member.coords[1] = 100;
                if (cmm.HasChildNodes)
                {
                    if (cmm.FirstChild.Name == "Partner")
                    {
                        family.core_member.partner = new Member();
                        CreateMemberFromXml(family.core_member.partner, cmm.FirstChild["Member"], family.register);
                        family.core_member.coords[0] -= 60;
                        family.core_member.partner.coords[0] = family.core_member.coords[0] + 120;
                        family.core_member.partner.coords[1] = family.core_member.coords[1];
                    }
                    if (cmm.LastChild.Name == "Children")
                    {
                        CreateChildrenFromXml(family.core_member, cmm.LastChild, family.register);
                        SetPositions();
                    }
                }
                cur_mbr = family.core_member;
                FillBio();
                form.Close();
            }

        };
        bCancel.Click += (a, b) => {
            form.Close();
        };
        form.ShowDialog();
    }
    public void ShowMessage(object obj, EventArgs ea)
    {
        MessageBox.Show("Версия: 1.0.0\nДата изменения: 21.06.2024\nАвтор: Тимофей \"FaaZMaaR\" Волхонский", "О программе",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    public void CloseApp(object obj, EventArgs ea)
    {
        Application.Exit();
    }
    public XmlElement CreateXmlMember(XmlElement xE, XmlDocument xDoc, Member m)
    {
        XmlElement xM = xDoc.CreateElement("Member");
        xE.AppendChild(xM);
        XmlAttribute xaName = xDoc.CreateAttribute("name");
        xaName.Value = m.name;
        XmlAttribute xaSex = xDoc.CreateAttribute("sex");
        if (m.sex == 'm') xaSex.Value = "Male";
        else xaSex.Value = "Female";
        XmlAttribute xaBirth = xDoc.CreateAttribute("birth");
        xaBirth.Value = m.birthday.ToString("dd.MM.yyyy");
        XmlAttribute xaHeal = xDoc.CreateAttribute("health");
        xaHeal.Value = "" + m.health;
        XmlAttribute xaChar = xDoc.CreateAttribute("charisma");
        xaChar.Value = "" + m.charisma;
        XmlAttribute xaInt = xDoc.CreateAttribute("intelligence");
        xaInt.Value = "" + m.intelligence;
        XmlAttribute xaLuck = xDoc.CreateAttribute("luck");
        xaLuck.Value = "" + m.luck;
        XmlAttribute xaKin = xDoc.CreateAttribute("kin");
        xaKin.Value = "" + m.kin;
        XmlAttribute xaGen = xDoc.CreateAttribute("generation");
        xaGen.Value = "" + m.generation;
        XmlAttribute xaNum = xDoc.CreateAttribute("number");
        xaNum.Value = "" + m.number;
        XmlAttribute xaPED = xDoc.CreateAttribute("primary_ed");
        xaPED.Value = m.primary_ed;
        XmlAttribute xaSED = xDoc.CreateAttribute("secondary_ed");
        xaSED.Value = m.secondary_ed;
        XmlAttribute xaHED = xDoc.CreateAttribute("higher_ed");
        xaHED.Value = m.higher_ed;
        XmlAttribute xaSpec = xDoc.CreateAttribute("specialization");
        xaSpec.Value = m.specialization;
        XmlAttribute xaSpecL = xDoc.CreateAttribute("spec_level");
        xaSpecL.Value = "" + m.spec_level;
        XmlAttribute xaUSpecL = xDoc.CreateAttribute("unspec_level");
        xaUSpecL.Value = m.unspec_levels[0] + "." + m.unspec_levels[1] + "." + m.unspec_levels[2] + "." + m.unspec_levels[3] + "." + m.unspec_levels[4];
        XmlAttribute xaHStat = xDoc.CreateAttribute("health_status");
        xaHStat.Value = m.health_status;
        XmlAttribute xaCStat = xDoc.CreateAttribute("career_status");
        xaCStat.Value = m.career_status;
        XmlAttribute xaIncome = xDoc.CreateAttribute("income");
        xaIncome.Value = "" + m.income;
        XmlAttribute xaCosts = xDoc.CreateAttribute("costs");
        xaCosts.Value = "" + m.costs;
        xM.Attributes.Append(xaName);
        xM.Attributes.Append(xaSex);
        xM.Attributes.Append(xaBirth);
        xM.Attributes.Append(xaHeal);
        xM.Attributes.Append(xaChar);
        xM.Attributes.Append(xaInt);
        xM.Attributes.Append(xaLuck);
        xM.Attributes.Append(xaKin);
        xM.Attributes.Append(xaGen);
        xM.Attributes.Append(xaNum);
        xM.Attributes.Append(xaPED);
        xM.Attributes.Append(xaSED);
        xM.Attributes.Append(xaHED);
        xM.Attributes.Append(xaSpec);
        xM.Attributes.Append(xaSpecL);
        xM.Attributes.Append(xaUSpecL);
        xM.Attributes.Append(xaHStat);
        xM.Attributes.Append(xaCStat);
        xM.Attributes.Append(xaIncome);
        xM.Attributes.Append(xaCosts);
        return xM;
    }
    public void CreateXmlChildren(XmlElement xE, XmlDocument xDoc, Member m)
    {
        foreach (Member v in m.children)
        {
            XmlElement xM = CreateXmlMember(xE, xDoc, v);
            if (v.partner != null)
            {
                XmlElement xP = xDoc.CreateElement("Partner");
                xM.AppendChild(xP);
                CreateXmlMember(xP, xDoc, v.partner);
            }
            if (v.children.Count != 0)
            {
                XmlElement xC = xDoc.CreateElement("Children");
                xM.AppendChild(xC);
                CreateXmlChildren(xC, xDoc, v);
            }
        }
    }
    public void CreateMemberFromXml(Member m, XmlElement xE, ArrayList reg)
    {
        XmlAttributeCollection xA = xE.Attributes;
        m.name = xA[0].Value;
        if (xA[1].Value == "Male") m.sex = 'm';
        else m.sex = 'f';
        m.birthday = new DateTime(Int32.Parse(xA[2].Value.Split('.')[2]), Int32.Parse(xA[2].Value.Split('.')[1]), Int32.Parse(xA[2].Value.Split('.')[0]));
        m.health = Int32.Parse(xA[3].Value);
        m.charisma = Int32.Parse(xA[4].Value);
        m.intelligence = Int32.Parse(xA[5].Value);
        m.luck = Int32.Parse(xA[6].Value);
        m.kin = Int32.Parse(xA[7].Value);
        m.generation = Int32.Parse(xA[8].Value);
        m.number = Int32.Parse(xA[9].Value);
        m.primary_ed = xA[10].Value;
        m.secondary_ed = xA[11].Value;
        m.higher_ed = xA[12].Value;
        m.specialization = xA[13].Value;
        m.spec_level = Int32.Parse(xA[14].Value);
        for (int i = 0; i < 5; i++)
        {
            m.unspec_levels[i] = Int32.Parse(xA[15].Value.Split('.')[i]);
        }
        m.health_status = xA[16].Value;
        m.career_status = xA[17].Value;
        m.income = Single.Parse(xA[18].Value);
        m.costs = Single.Parse(xA[19].Value);
        reg.Add(m);
        m.id = reg.Count - 1;
    }
    public void CreateChildrenFromXml(Member m, XmlNode xN, ArrayList reg)
    {
        Member chm;
        foreach (XmlElement e in xN)
        {
            chm = new Member();
            CreateMemberFromXml(chm, e, reg);
            m.children.Add(chm);
            if (e.HasChildNodes)
            {
                if (e.FirstChild.Name == "Partner")
                {
                    chm.partner = new Member();
                    CreateMemberFromXml(chm.partner, e.FirstChild["Member"], reg);
                }
                if (e.LastChild.Name == "Children")
                {
                    CreateChildrenFromXml(chm, e.LastChild, reg);
                }
            }
        }
    }
}

class MainClass
{
    static void Main()
    {
        Member.Init();
        MyForm mf = new MyForm();
        Application.Run(mf);
    }
}