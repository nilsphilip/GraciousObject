# Introduction

Provides C# class `GraciousObject`.
A `GraciousObject` represents an object whose members can be dynamically added at run time, similar to [ExpandoObject](https://learn.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject).
If an unset value is accessed, the returned value will be false.
The `GraciousObject` does not throw an `Exception`.
It extends class [DynamicObject](https://learn.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject) and implements [INotifyPropertyChanged](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged).

The package is available on [nuget.org](https://www.nuget.org/packages/GraciousObject/1.0.0). It targets netstandard2.0.

# Code Example

```
using GlassBee.Utilities;

dynamic graciousObject = new GraciousObject();

graciousObject.Version = "1.0";

graciousObject.Customer = new GraciousObject();
graciousObject.Customer.Name = "Dan";
graciousObject.Customer.Age = 27;

if (graciousObject.Version is not false)
{
  Console.WriteLine(graciousObject.Version);
}

if (graciousObject.Customer.Name is string name)
{
  Console.WriteLine($"Name is {name}");
}

if (graciousObject.OutputDirectory is false)
{
  Console.WriteLine("OutputDirectory is not set!");
}

// Output:
// 1.0
// Name is Dan
// OutputDirectory is not set!
```

