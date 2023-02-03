// See https://aka.ms/new-console-template for more information
using Mapster;

Console.WriteLine("Hello, World!");

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