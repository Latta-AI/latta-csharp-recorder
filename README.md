To implement Latta into C# desktop / server application:

1. Install Latta via NuGet

```
dotnet add package LattaCsharp
```

2. Wrap whole application into this block:

```
LattaRecorder.LattaRecorder.RecordApplication("YOUR API KEY", () =>
{
    // YOUR APPLICATION CODE
});
```