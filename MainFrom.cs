using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

class MainForm : Form
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

    public int timer = 0;

    public Family family = null;
    public Member cur_mbr = null;

    public float[] map_coords = new float[] { 0, 0 };
    public Thread thread;

    public bool mouse_clicked = false;
    public float[] mouse_coords = new float[] { 0, 0 };
    public float[] old_coords = new float[] { 0, 0 };

    public MainForm() : base()
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
        lPhoto.Image = Member.imgMale;
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
        lDate.Text = "dd.MM.yyyy";
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
                timer = 0;
                family.date = family.date.AddDays(1);
                if (family != null)
                {
                    foreach (Member v in family.register)
                    {
                        inc += v.income;
                        inc -= v.costs;
                        EventMaster.PlayMarriage(v, family.register, family.date);
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
                    FillDateBudget();
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
                m.DrawBond(g, map_coords);
                m.Draw(g, map_coords, family.date);
            }
        }
        g.Restore(state);
    }

    public void FillDateBudget()
    {
        lDate.Text = family.date.ToString("dd.MM.yyyy");
        string budg = "$" + Math.Abs(family.budget);
        if (family.budget > 9999) budg = "$" + Math.Abs(Math.Round(family.budget / 1000, 2)) + "k";
        if (family.budget / 1000 > 9999) budg = "$" + Math.Abs(Math.Round(family.budget / 1000000, 2)) + "M";
        if (family.budget / 1000000 > 9999) budg = "$" + Math.Abs(Math.Round(family.budget / 1000000000, 2)) + "B";
        if (family.budget < 0) lBudget.Text = "-" + budg;
        else lBudget.Text = budg;
    }

    public void FillBio()
    {
        if (cur_mbr.sex == 'm') lPhoto.Image = Member.imgMale;
        else lPhoto.Image = Member.imgFemale;
        lNameAge.Text = cur_mbr.name + ", " + cur_mbr.Get_Age(family.date);
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
            lParents.Text = "Parents: " + cur_mbr.parent.name + ", " + cur_mbr.parent.Get_Age(family.date);
            lParents.Text += " " + cur_mbr.parent.partner.name + ", " + cur_mbr.parent.partner.Get_Age(family.date);
        }
        else lParents.Text = "Parents: No";
        if (cur_mbr.partner != null)
        {
            if (cur_mbr.sex == 'm') lPartner.Text = "Wife: " + cur_mbr.partner.name + ", " + cur_mbr.partner.Get_Age(family.date);
            else lPartner.Text = "Husband: " + cur_mbr.partner.name + ", " + cur_mbr.partner.Get_Age(family.date);
        }
        else lPartner.Text = "Partner: No";
        lChildren.Text = "Children: ";
        if (cur_mbr.children.Count != 0)
        {
            foreach (Member v in cur_mbr.children)
            {
                lChildren.Text += v.name + ", " + v.Get_Age(family.date) + " ";
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
    public void Marry()
    {
        family.Marry(cur_mbr);
    }
    public void GiveBirth()
    {
        family.GiveBirth(cur_mbr);
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
                family = new Family(tbSrn.Text, 1000, new DateTime(2000, 1, 1));
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
                FillDateBudget();
                lInc.Text = "+$0";
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
        if (family == null) return;
        FamilyXMLSaveLoader saver =new FamilyXMLSaveLoader(family);
        saver.Save("Save Files");

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
                FamilyXMLSaveLoader loader = new FamilyXMLSaveLoader();
                loader.Load(lb.SelectedItem.ToString());
                family = loader.family;
                cur_mbr = family.core_member;
                FillBio();
                FillDateBudget();
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
        MessageBox.Show("Версия: 0.2.0\nДата изменения: 15.03.2025\nАвтор: Тимофей \"FaaZMaaR\" Волхонский", "О программе",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    public void CloseApp(object obj, EventArgs ea)
    {
        Application.Exit();
    }
}