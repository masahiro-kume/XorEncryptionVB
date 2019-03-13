Option Explicit On
Option Strict On

Imports XorEncryptionVB.XorEncryption

Module Module1
    Sub Main()
        'TestFileFunction()
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

    Private Sub TestFileFunction()
        Dim p As Byte()
        Dim s As Integer
        Dim l As Integer
        Dim z As String = ""

        If Not System.IO.File.Exists("test.bin") Then
            Dim ba(&HFF) As Byte
            For i As Integer = &H0 To &HFF
                ba(i) = CByte(i)
            Next
            Using fs As System.IO.FileStream = System.IO.File.OpenWrite("test.bin")
                For j As Integer = 0 To &HFF
                    fs.WriteByte(ba(j))
                Next
                fs.Close()
            End Using
        End If

        Do While z <> "z"
            Console.Write("StartPos: ")
            s = CInt(Console.ReadLine())
            Console.WriteLine(s.ToString & vbNewLine)
            Console.Write("Length : ")
            l = CInt(Console.ReadLine())
            Console.WriteLine(l.ToString & vbNewLine)
            p = GetBytesFromFile("test.bin", s, l)
            Console.WriteLine("StartPosition: {1}{0}ByteLength   : {2}{0}BinStream    : {3}{0}", vbNewLine, s, l, BitConverter.ToString(p))
            z = Console.ReadLine()
        Loop
        End
    End Sub

End Module
