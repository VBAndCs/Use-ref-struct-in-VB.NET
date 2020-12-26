How to use C# ref struct in VB.NET

# Alert:
The workaround I am using here seems upsetting one of Roslyn team, so, he filed [this issue]( https://github.com/dotnet/roslyn/issues/50118) to ask to prevent this worarount and deny VB any access to ref structs, which makes VB unable to use important classes and libraries in .NET Core such as System.Text.Json. So, I think it's time for VB community to make the team hear their voice. Please, fight against killing your favorite language.
After Cyrus closed the discussion in Roslyn to supresss our voice, I published [an issue in developers community](https://developercommunity.visualstudio.com/content/problem/1296735/vbnet-can-actually-use-ref-structs-but-there-is-a.html), , so, please vote for it.

# What is a ref struct?
C# introduced 'ref structs' to represent a `stack-only Structures`, which can perform better (faster) than normal structures and classes, because they are allocated in a part of the memory called `the stack` and are never allowed to be moved to another part called `the heap`, which has less performance (slower). To ensure that, there are many limitation in using ref structs, to prevent them from being boxed directly or indirectly, so:
-	you can't assign/convert them to `Object`.
-	you can't create an array of a ref struct.
-	you can't have a ref strict field in a class or a normal structure. ref strict fields are only allowed insdied another ref struct.
and other rules alike.
For such many restrictions that need a lot of work to modify the compiler, MS decided to not support them in VB.NET. 

# The problem
The problem is that ref structs found their way to .NET Core to benefit from their performance, so, they are used to create many types (like `Span(Of T)`) that are used with many important classes like string and file classes, and more recently, the `System.Text.Json library`. But VB.NET can't use any of that, which mean that VB will be out of the game over time as it can't use more and more APIs of .NET Core!
But, I found a workaround, and it works just fine!
Let's try a simple code:
```VB.NET
Sub main()
   Dim s As New Span(Of Integer)({1, 2, 3})
   Console.WriteLine(s(1))
End Sub
```

if you tried this, the code editor will show an error line  under 'Span(Of Integer)`, with the message:
> Span(Of Integer)' is obsolete. Types with embedded references are not supported in this version of your compiler.

# The solution
I tried to workaround this until I found a very simple solution: Just mark the method (or the whole module/calss) with the Obsolete attribute, and pass false to its second parameter:
```VB.NET
<Obsolete("Allow ref structs", False)>
Sub main()
   Dim s As New Span(Of Integer)({1, 2, 3})
   Console.WriteLine(s(1))
End Sub
```

Now this code will compile and run successfully. 


# But be aware 
VB knows nothing about ref structs, and think they are just normal structs, so, it will not prevent you from boding them. Try this:
```VB.NET
Dim s As New Span(Of Integer)({1, 2, 3})
Dim o As Object = s
```

VB will not object boxing the span into an object, and the code will compile, but the .NET RunTime will not allow this, and will give you a runtime error:
> System.InvalidProgramException: 'Cannot create boxed ByRef-like values.'

Also, you must know that this error is external to VB.NET, and will crash your app. Ypu can't even handle this error probably by a Try Catch block as it will not prevent the crash. If you used this code:
```VB.NET
        Dim s As New Span(Of Integer)({1, 2, 3})
        Try
            Dim o As Object = s
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
```

The `Catch` statement will catch the error and the error message will be printed to the consol, but the app will still crash and shut down.
So, you must use ref structs with cautious, and don't try to violate their rules.




 


