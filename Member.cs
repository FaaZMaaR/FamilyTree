using System;
using System.Collections;
using System.Drawing;

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
    public static Image imgMale = Image.FromFile("Images/male_frame.png");
    public static Image imgFemale = Image.FromFile("Images/female_frame.png");
    public static Image imgMale2 = Image.FromFile("Images/male_photo.png");
    public static Image imgFemale2 = Image.FromFile("Images/female_photo.png");

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

    public void Draw(Graphics g, float[] map_coords, DateTime date)
    {
        if (sex == 'm')
        {
            g.DrawImage(imgMale2, coords[0] - 25 + map_coords[0], coords[1] - 25 + map_coords[1]);
        }
        else
        {
            g.DrawImage(imgFemale2, coords[0] - 25 + map_coords[0], coords[1] - 25 + map_coords[1]);
        }
        g.DrawString(name + ", " + Get_Age(date), new Font("Arial", 10, FontStyle.Bold), new SolidBrush(Color.Black),
                    coords[0] - (name + ", " + Get_Age(date)).Length / 2 * 8 + map_coords[0], coords[1] + 25 + map_coords[1]);
    }

    public void DrawBond(Graphics g, float[] map_coords)
    {
        float x1 = 0;
        float x2 = 0;
        if (kin == 1)
        {
            if (partner != null)
            {
                g.DrawLine(new Pen(Color.Red, 6), coords[0] + map_coords[0], coords[1] - 3 + map_coords[1],
                            partner.coords[0] + map_coords[0], partner.coords[1] - 3 + map_coords[1]);
            }
            if (children.Count != 0)
            {
                g.DrawLine(new Pen(Color.Red, 6), coords[0] + 56 + map_coords[0], coords[1] + map_coords[1],
                            coords[0] + 56 + map_coords[0], coords[1] + 60 + map_coords[1]);
                if (((Member)children[0]).partner != null) x1 = ((Member)children[0]).coords[0] + 60;
                else x1 = ((Member)children[0]).coords[0];
                if (((Member)children[children.Count - 1]).partner != null) x2 = ((Member)children[children.Count - 1]).coords[0] + 60;
                else x2 = ((Member)children[children.Count - 1]).coords[0];
                g.DrawLine(new Pen(Color.Red, 6), x1 - 7 + map_coords[0], coords[1] + 60 + map_coords[1],
                            x2 - 1 + map_coords[0], coords[1] + 60 + map_coords[1]);
                foreach (Member v in children)
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