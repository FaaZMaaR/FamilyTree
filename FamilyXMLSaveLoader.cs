using System;
using System.Xml;

class FamilyXMLSaveLoader
{
    public Family family;

    public FamilyXMLSaveLoader(Family family)
    {
        this.family = family;
    }

    public FamilyXMLSaveLoader() { }

    public XmlElement CreateXmlMember(XmlElement xElem, XmlDocument xDoc, Member member)
    {
        XmlElement xMember = xDoc.CreateElement("Member");
        xElem.AppendChild(xMember);
        XmlAttribute xaName = xDoc.CreateAttribute("name");
        xaName.Value = member.name;
        XmlAttribute xaSex = xDoc.CreateAttribute("sex");
        if (member.sex == 'm') xaSex.Value = "Male";
        else xaSex.Value = "Female";
        XmlAttribute xaBirth = xDoc.CreateAttribute("birth");
        xaBirth.Value = member.birthday.ToString("dd.MM.yyyy");
        XmlAttribute xaHeal = xDoc.CreateAttribute("health");
        xaHeal.Value = "" + member.health;
        XmlAttribute xaChar = xDoc.CreateAttribute("charisma");
        xaChar.Value = "" + member.charisma;
        XmlAttribute xaInt = xDoc.CreateAttribute("intelligence");
        xaInt.Value = "" + member.intelligence;
        XmlAttribute xaLuck = xDoc.CreateAttribute("luck");
        xaLuck.Value = "" + member.luck;
        XmlAttribute xaKin = xDoc.CreateAttribute("kin");
        xaKin.Value = "" + member.kin;
        XmlAttribute xaGen = xDoc.CreateAttribute("generation");
        xaGen.Value = "" + member.generation;
        XmlAttribute xaNum = xDoc.CreateAttribute("number");
        xaNum.Value = "" + member.number;
        XmlAttribute xaPED = xDoc.CreateAttribute("primary_ed");
        xaPED.Value = member.primary_ed;
        XmlAttribute xaSED = xDoc.CreateAttribute("secondary_ed");
        xaSED.Value = member.secondary_ed;
        XmlAttribute xaHED = xDoc.CreateAttribute("higher_ed");
        xaHED.Value = member.higher_ed;
        XmlAttribute xaSpec = xDoc.CreateAttribute("specialization");
        xaSpec.Value = member.specialization;
        XmlAttribute xaSpecL = xDoc.CreateAttribute("spec_level");
        xaSpecL.Value = "" + member.spec_level;
        XmlAttribute xaUSpecL = xDoc.CreateAttribute("unspec_level");
        xaUSpecL.Value = member.unspec_levels[0] + "." + member.unspec_levels[1] + "." + member.unspec_levels[2] + "." + member.unspec_levels[3] + "." + member.unspec_levels[4];
        XmlAttribute xaHStat = xDoc.CreateAttribute("health_status");
        xaHStat.Value = member.health_status;
        XmlAttribute xaCStat = xDoc.CreateAttribute("career_status");
        xaCStat.Value = member.career_status;
        XmlAttribute xaIncome = xDoc.CreateAttribute("income");
        xaIncome.Value = "" + member.income;
        XmlAttribute xaCosts = xDoc.CreateAttribute("costs");
        xaCosts.Value = "" + member.costs;
        xMember.Attributes.Append(xaName);
        xMember.Attributes.Append(xaSex);
        xMember.Attributes.Append(xaBirth);
        xMember.Attributes.Append(xaHeal);
        xMember.Attributes.Append(xaChar);
        xMember.Attributes.Append(xaInt);
        xMember.Attributes.Append(xaLuck);
        xMember.Attributes.Append(xaKin);
        xMember.Attributes.Append(xaGen);
        xMember.Attributes.Append(xaNum);
        xMember.Attributes.Append(xaPED);
        xMember.Attributes.Append(xaSED);
        xMember.Attributes.Append(xaHED);
        xMember.Attributes.Append(xaSpec);
        xMember.Attributes.Append(xaSpecL);
        xMember.Attributes.Append(xaUSpecL);
        xMember.Attributes.Append(xaHStat);
        xMember.Attributes.Append(xaCStat);
        xMember.Attributes.Append(xaIncome);
        xMember.Attributes.Append(xaCosts);
        return xMember;
    }

    public void CreateXmlChildren(XmlElement xElem, XmlDocument xDoc, Member member)
    {
        foreach (Member v in member.children)
        {
            XmlElement xMember = CreateXmlMember(xElem, xDoc, v);
            if (v.partner != null)
            {
                XmlElement xP = xDoc.CreateElement("Partner");
                xMember.AppendChild(xP);
                CreateXmlMember(xP, xDoc, v.partner);
            }
            if (v.children.Count != 0)
            {
                XmlElement xC = xDoc.CreateElement("Children");
                xMember.AppendChild(xC);
                CreateXmlChildren(xC, xDoc, v);
            }
        }
    }

    public void CreateMemberFromXml(Member member, XmlElement xElem)
    {
        XmlAttributeCollection xA = xElem.Attributes;
        member.name = xA[0].Value;
        if (xA[1].Value == "Male") member.sex = 'm';
        else member.sex = 'f';
        member.birthday = new DateTime(Int32.Parse(xA[2].Value.Split('.')[2]), Int32.Parse(xA[2].Value.Split('.')[1]), Int32.Parse(xA[2].Value.Split('.')[0]));
        member.health = Int32.Parse(xA[3].Value);
        member.charisma = Int32.Parse(xA[4].Value);
        member.intelligence = Int32.Parse(xA[5].Value);
        member.luck = Int32.Parse(xA[6].Value);
        member.kin = Int32.Parse(xA[7].Value);
        member.generation = Int32.Parse(xA[8].Value);
        member.number = Int32.Parse(xA[9].Value);
        member.primary_ed = xA[10].Value;
        member.secondary_ed = xA[11].Value;
        member.higher_ed = xA[12].Value;
        member.specialization = xA[13].Value;
        member.spec_level = Int32.Parse(xA[14].Value);
        for (int i = 0; i < 5; i++)
        {
            member.unspec_levels[i] = Int32.Parse(xA[15].Value.Split('.')[i]);
        }
        member.health_status = xA[16].Value;
        member.career_status = xA[17].Value;
        member.income = Single.Parse(xA[18].Value);
        member.costs = Single.Parse(xA[19].Value);
        family.register.Add(member);
        member.id = family.register.Count - 1;
    }

    public void CreateChildrenFromXml(Member member, XmlNode xNode)
    {
        Member chm;
        foreach (XmlElement e in xNode)
        {
            chm = new Member();
            CreateMemberFromXml(chm, e);
            member.children.Add(chm);
            if (e.HasChildNodes)
            {
                if (e.FirstChild.Name == "Partner")
                {
                    chm.partner = new Member();
                    CreateMemberFromXml(chm.partner, e.FirstChild["Member"]);
                }
                if (e.LastChild.Name == "Children")
                {
                    CreateChildrenFromXml(chm, e.LastChild);
                }
            }
        }
    }

    public void Save(string dir)
    {
        if (family == null) return;
        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement("Family");
        doc.AppendChild(root);
        XmlElement xDate = doc.CreateElement("Date");
        xDate.InnerText = family.date.ToString("dd.MM.yyyy");
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
        doc.Save(dir + "/" + family.surname + ".xml");
    }

    public void Load(string file)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(file);
        XmlElement root = doc.DocumentElement;
        XmlElement dt = root["Date"];
        XmlElement sn = root["Surname"];
        XmlElement bg = root["Budget"];
        DateTime date = new DateTime(Int32.Parse(dt.InnerText.Split('.')[2]),
            Int32.Parse(dt.InnerText.Split('.')[1]),
            Int32.Parse(dt.InnerText.Split('.')[0]));
        family = new Family(sn.InnerText, Single.Parse(bg.InnerText), date);
        XmlElement cm = root["Core_member"];
        XmlElement cmm = cm["Member"];
        family.core_member = new Member();
        CreateMemberFromXml(family.core_member, cmm);
        family.core_member.coords[0] = 0;
        family.core_member.coords[1] = 100;
        if (cmm.HasChildNodes)
        {
            if (cmm.FirstChild.Name == "Partner")
            {
                family.core_member.partner = new Member();
                CreateMemberFromXml(family.core_member.partner, cmm.FirstChild["Member"]);
                family.core_member.coords[0] -= 60;
                family.core_member.partner.coords[0] = family.core_member.coords[0] + 120;
                family.core_member.partner.coords[1] = family.core_member.coords[1];
            }
            if (cmm.LastChild.Name == "Children")
            {
                CreateChildrenFromXml(family.core_member, cmm.LastChild);
                family.SetPositions();
            }
        }
    }
}
