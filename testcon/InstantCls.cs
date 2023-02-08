using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testcon;

public interface  Icls
{
    public void Test1();
}
public class InstantCls1 : Icls
{
    public void Test1()
    {
        Console.WriteLine("1");
    }
}

public class InstantCls2 :Icls
{
    public void Test1()
    {
        Console.WriteLine("2");
    }
}

