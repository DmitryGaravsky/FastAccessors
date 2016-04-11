# FastAccessors
Fast Accessors Library for .Net.  
Allows you to access any members of your .Net classes as fast as you want.


##Usage

Let the `Foo` class is defined as follows:
```cs
class Foo {
    string private_Name; 
    int private_Field; 
    static int static_Field; 
}
```
If you want to get access to it's fields you can use the "by-name" accessors:

```cs
using FastAccessors;
//...
Foo foo = new Foo()
// access to the specific private field by field-name
string _name = foo.@ƒ("private_Name") as string;
```
It's quite fast. But, if you want to be faster, you can use "field-keys":

```cs
using FastAccessors;
// Field-Keys initialization
int fldKey_private_Field = "private_Field".@ƒRegister(typeof(Foo));
int fldKey_static_Field = "static_Field".@ƒsRegister(typeof(Foo));
//...
Foo foo = new Foo()
// access to the specific private field by field-key
int _field = (int)foo.@ƒ(fldKey_private_Field);
// access to the specific static field by field-key
int _s_field = (int)typeof(Foo).@ƒs(fldKey_Static_Field);
```
To be **fastest** use the "default-fields":

```cs
using FastAccessors;
// default-fields initialization
"private_Name".@ƒRegister(typeof(Foo), true); // default instance-field
"static_Field".@ƒsRegister(typeof(Foo), true); // default static field
//...
Foo foo = new Foo()
// access to the default-fields
string _name = foo.@ƒDefault() as string;
int _s_Field = (int)foo.@ƒsDefault();
```
##Benchmarks

```ini
BenchmarkDotNet=v0.9.4.0
OS=Microsoft Windows NT 6.2.9200.0
Processor=Intel(R) Core(TM) i7-4702HQ CPU @ 2.20GHz, ProcessorCount=8
Frequency=2143485 ticks, Resolution=466.5300 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1055.0

Type=Benchmarks  Mode=Throughput  

                                  Method |      Median |    StdDev |
---------------------------------------- |------------ |---------- |
               1.1. Reflection(Instance) | 171.1189 ns | 4.6809 ns |
                 1.2. Reflection(Static) | 162.7281 ns | 4.2497 ns |
                 1.3. Reflection(Public) | 171.0062 ns | 2.4226 ns |
               1.4. Reflection(Readonly) | 169.5342 ns | 4.0005 ns |
             2.1 FastAccessors(Instance) |  89.8821 ns | 1.4286 ns |
               2.2 FastAccessors(Static) |  87.9811 ns | 0.9167 ns |
               2.3 FastAccessors(Public) |  88.2568 ns | 0.7475 ns |
             2.4 FastAccessors(Readonly) |  91.0912 ns | 0.9119 ns |
              2.5 FastAccessors(Generic) |  52.1873 ns | 1.1102 ns |
         3.1 FastAccessors(Instance,Key) |  18.1572 ns | 0.1741 ns |
           3.2 FastAccessors(Static,Key) |  16.7747 ns | 0.1925 ns |
          3.3 FastAccessors(Generic,Key) |  32.3943 ns | 0.2694 ns |
 4.1 FastAccessors(Generic,DefaultField) |   6.8535 ns | 0.1283 ns |
  4.2 FastAccessors(Static,DefaultField) |   3.6290 ns | 0.0805 ns |
```
