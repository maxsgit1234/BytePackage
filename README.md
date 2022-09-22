# BytePack
 *Light-weight, manual serialization.*

BytePack is a simple light-weight tool that allows you to manually serialize and deserialize your .NET objects. 

## Quick Start

To **serialize** objects, simply "Add" your objects one-by-one to a new `BytePack` and then "Pack" them into a single `byte[]`:
    
    BytePackage.BytePack pack = new BytePackage.BytePack();
    pack.AddInt(123);
    pack.AddString("hello");
    pack.AddDoubles(new[] { 1.2, 1.3, 1.4 });
    byte[] bytes = pack.Pack();

To **deserialize** objects, simply "Read" the objects from a `BytePack` ***in the same order they were originally added during serialization***:

    BytePackage.BytePack read = BytePackage.BytePack.Read(bytes);
    int i = read.ReadInt();
    string s = read.ReadString();
    double[] d = read.ReadDoubles();
    
    Console.WriteLine("int is: " + i);
    Console.WriteLine("string is: " + s);
    Console.WriteLine("doubles are: " + d[0] + ", " + d[1] + ", " + d[2]);


## Installing
BytePack is available as a NuGet package: https://www.nuget.org/packages/BytePack/


## How it works
BytePack binary-serializes the objects that you add based on their type using built-in .NET methods (e.g. `System.BitConverter`) and keeps them in an ordered list. When you call `BytePack.Pack()`, these individual arrays are concatenated to a single larger one.

## Usage
There are many different tools available for object-serialization in .NET. BytePack has some advantages and disadvantages.

### Advantages
 - It is extremely simple and your code is easy-to-read and easy-to-understand - there is no magic happening via Reflection or any other non-obvious means.
 - Serialized objects are typically very small compared to text-based serialization, such as JSON.
 - Serialization and deserialization are relatively fast operations.
 - It is easy and obvious how you can customize the serialization of your objects however you like.

### Disadvantages
 - Serialized objects are not human-readable.
 - Serialized objects contain no metadata with instructions for deserialization. You have to know how they were serialized in order to properly deserialize them.
 - Less open-minded colleagues of yours may criticize you for not using something more "standard" like `protocol buffers`.
