Module Program

    <Obsolete("Allow ref structs in VB.NET", False)>
    Sub main()
        Dim s As New Span(Of Integer)({1, 2, 3})
        Console.WriteLine(s(1))

        ' This code will crash. Don't box ref structs 
        'Try
        '    Dim o As Object = s
        'Catch ex As Exception
        '    Console.WriteLine(ex.Message)
        'End Try
    End Sub

End Module


