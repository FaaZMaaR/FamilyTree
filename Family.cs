using System;
using System.Collections;

class Family
{
    public string surname;
    public Member core_member;
    public float budget;
    public DateTime date;
    public ArrayList register = new ArrayList();
    public ArrayList descendants = new ArrayList();
    public Family(string srn, float bgt, DateTime dt)
    {
        surname = srn;
        budget = bgt;
        date = dt;
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
                v  .coords[1] = core_member.coords[1] + 120 * (v.generation - 1);
                v.partner.coords[0] = v.coords[0] + 120;
                v.partner.coords[1] = v.coords[1];
                SetParentsCoords(v);
            }
        }
    }

    public void Marry(Member m)
    {
        m.Get_Married(register);
        m.coords[0] -= 60;
        m.partner.coords[0] = m.coords[0] + 120;
        m.partner.coords[1] = m.coords[1];
    }

    public void GiveBirth(Member m)
    {
        m.Give_Birth(date, register);
        SetPositions();
    }

    public void SetPositions()
    {
        descendants.Clear();
        DefineDescendants(core_member);
        foreach (Member v in descendants)
        {
            v.coords[0] = core_member.coords[0] + 60 - 120 * (descendants.Count - 1) + 240 * (descendants.IndexOf(v));
            v.coords[1] = core_member.coords[1] + 120 * (v.generation - 1);
            if (v.partner != null)
            {
                v.coords[0] -= 60;
                v.partner.coords[0] = v.coords[0] + 120;
                v.partner.coords[1] = v.coords[1];
            }
        }
        SetParentsCoords(core_member);
    }
}
