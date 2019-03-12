Option Explicit On
Option Strict On

Imports XorEncryptionVB.XorEncryption

Module Module1
    Sub Main()
        Dim MyText As String
        Dim MyPass As String
        Dim FileName As String = "..\..\..\sample.7z"
        Dim IncludesSpace As Boolean = False
        Dim StartPos As Integer = 502
        Dim ByteLength As Integer = 36

        Console.Write(">> Enter Base Text   : ")
        MyText = Console.ReadLine

        Console.Write(">> Enter Pass String : ")
        MyPass = Console.ReadLine

        If InStr(MyText, " ") > 0 Then
            Console.WriteLine(vbNewLine & "## Base Text Contains Spaces..." & vbNewLine & "## Ecrypting SHOTER MODE..." & vbNewLine)
            IncludesSpace = True
        Else
            Console.WriteLine(vbNewLine & "## Base Text NOT Contains Spaces..." & vbNewLine & "## Ecrypting LONGER MODE..." & vbNewLine)
        End If

        Console.WriteLine("## Test Sample with Text mode")
        Dim EncodedString As String = PasswdEncode(MyText, MyPass)
        Dim DecodedString As String = PasswdDecode(EncodedString, MyPass)
        Console.WriteLine("Encoded           : " & EncodedString)
        Console.WriteLine("Decoded           : " & DecodedString)
        Console.WriteLine("Result            : " & (MyText = DecodedString).ToString & vbNewLine)

        Console.WriteLine("## Test Sample with Binary mode")
        Dim p As Byte() = XorEncryption.Encoding.GetBytes(MyPass)
        EncodedString = PasswdEncode(MyText, p, IncludesSpace)
        DecodedString = PasswdDecode(EncodedString, p, IncludesSpace)
        Console.WriteLine("Encoded           : " & EncodedString)
        Console.WriteLine("Decoded           : " & DecodedString)
        Console.WriteLine("Result            : " & (MyText = DecodedString).ToString & vbNewLine)

        If System.IO.File.Exists(FileName) Then
            Console.WriteLine(vbNewLine & "## Test Sample with File mode")
            p = GetBytesFromFile(FileName, StartPos, ByteLength)
            Console.WriteLine("FileName          : {1}{0}StartPosition     : {2}{0}ByteLength        : {3}{0}PassStream        : {4}",
                              vbNewLine, FileName, StartPos, ByteLength, BitConverter.ToString(p))
            EncodedString = PasswdEncode(MyText, p, IncludesSpace)
            DecodedString = PasswdDecode(EncodedString, p, IncludesSpace)
            Console.WriteLine("Encoded           : " & EncodedString)
            Console.WriteLine("Decoded           : " & DecodedString)
            Console.WriteLine("Result            : " & (MyText = DecodedString).ToString & vbNewLine)
        End If

        Console.WriteLine("## Test Sample with Text mode with ASCII Encoding")
        Console.WriteLine("## MultiByte String will NOT work well")
        XorEncryption.Encoding = System.Text.Encoding.ASCII
        EncodedString = PasswdEncode(MyText, MyPass)
        DecodedString = PasswdDecode(EncodedString, MyPass)
        Console.WriteLine("Encoded           : " & EncodedString)
        Console.WriteLine("Decoded           : " & DecodedString)
        Console.WriteLine("Result            : " & (MyText = DecodedString).ToString & vbNewLine)

        Console.Write(">> Hit Enter Key To Exit ")
        Console.Read()

    End Sub
End Module
