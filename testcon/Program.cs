// See https://aka.ms/new-console-template for more information
using Mapster;
using MediatR.Courier;
using System.Diagnostics.Metrics;
using testcon;

Console.WriteLine("Hello, World!");
ICourier ic;

//ic.Subscribe<>

InstantCls1 i1 = new InstantCls1();
InstantCls2 i2 = new InstantCls2();

i1.Test1();
i2.Test1();

return;

T1 t1 = new T1()
{
    ID = 3, name = "아아아"
};
T2 t2 = new T2();

t2 = t1.Adapt<T2>();

Console.WriteLine($"{t2.name} {t2.Address}");



public class T1
{
    public int ID { get; set; }
    public string name { get; set; }
}
public class T2
{
    public int ID { get; set; }
    public string name { get; set; }
    public string Address { get; set; }
}