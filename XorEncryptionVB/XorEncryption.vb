Option Explicit On
Option Strict On

Imports System.Text
Imports System.IO
Imports System.Collections

Public Class XorEncryption
    Private Shared MyEncoding As Encoding = Encoding.Unicode

    Protected Friend Shared Property Encoding As Encoding
        Get
            Return MyEncoding
        End Get
        Set(value As Encoding)
            MyEncoding = value
        End Set
    End Property

    Public Shared Function GetBytesFromFile(FileName As String, Optional StartPos As Integer = 0, Optional ByteLength As Integer = 24) As Byte()
        Dim ReturnValue(ByteLength - 1) As Byte
        If File.Exists(FileName) Then
            Try
                Using fs As FileStream = File.OpenRead(FileName)
                    Dim rda(StartPos + ByteLength - 1) As Byte
                    Dim len As Integer = fs.Read(rda, 0, StartPos + ByteLength)
                    If len < ByteLength Then
                        ReDim ReturnValue(len - 1)
                        ReturnValue = rda
                    Else
                        For i As Integer = 1 To ByteLength
                            ReturnValue(ByteLength - i) = rda(len - i)
                        Next
                    End If
                End Using
            Catch ex As Exception
                ReturnValue = Nothing
            End Try
        Else
            ReturnValue = Nothing
        End If
        Return ReturnValue
    End Function

    Public Shared Function BytesXor(a1 As Byte(), a2 As Byte()) As Byte()
        Dim ba1 As BitArray = New BitArray(a1)
        Dim ba2 As BitArray = New BitArray(a2)
        Dim intLength As Integer = System.Math.Min(a1.Length, a2.Length)
        Dim RetrunValue(intLength - 1) As Byte
        If ba1.Length > ba2.Length Then ba1.Length = ba2.Length
        If ba2.Length > ba1.Length Then ba2.Length = ba1.Length
        Dim ba3 As BitArray = ba1.Xor(ba2)
        Dim p As Integer = 0
        For i As Integer = 0 To intLength - 1
            Dim v As Integer = 0
            For j As Integer = 0 To 7
                If ba3.Get(p) Then
                    'BitArray(Byte()) sorts bits from lower to higher
                    '"BitArray to Byte" must be put by reverse order
                    v += 1 << j
                End If
                p += 1
            Next
            RetrunValue(i) = CByte(v)
        Next
        Return RetrunValue
    End Function

    Protected Friend Shared Function PasswdEncode(Body As String, PassStream As Byte(), Optional IncludesSpace As Boolean = False) As String
        Dim strReturn As String
        If IsNothing(PassStream) OrElse PassStream.Length = 0 Then
            strReturn = Convert.ToBase64String(MyEncoding.GetBytes(Body))
        Else
            If IncludesSpace Then
                Dim b As Byte() = MyEncoding.GetBytes(Body)
                If b.Length < PassStream.Length Then
                    strReturn = Convert.ToBase64String(BytesXor(b, PassStream))
                Else
                    Dim p(b.Length - 1) As Byte
                    For i As Integer = 0 To b.Length - 1
                        p(i) = PassStream(i Mod PassStream.Length)
                    Next
                    strReturn = Convert.ToBase64String(BytesXor(b, p))
                End If
            Else
                Dim nBody As String = Body & " "
                Dim b As Byte() = MyEncoding.GetBytes(nBody)
                Dim j As Integer = 0
                Do While b.Length < PassStream.Length
                    nBody &= nBody.Substring(j, 1)
                    b = MyEncoding.GetBytes(nBody)
                    j += 1
                Loop
                Dim p(b.Length - 1) As Byte
                For i As Integer = 0 To b.Length - 1
                    p(i) = PassStream(i Mod PassStream.Length)
                Next
                strReturn = Convert.ToBase64String(BytesXor(b, p))
            End If
        End If

        Return strReturn
    End Function

    Protected Friend Shared Function PasswdDecode(Encoded As String, PassStream As Byte(), Optional IncludesSpace As Boolean = False) As String
        Dim strReturn As String
        Dim b As Byte() = Convert.FromBase64String(Encoded)
        If IsNothing(PassStream) OrElse PassStream.Length = 0 Then
            strReturn = MyEncoding.GetString(b)
        Else
            Dim p(b.Length - 1) As Byte
            For i As Integer = 0 To b.Length - 1
                p(i) = PassStream(i Mod PassStream.Length)
            Next
            strReturn = MyEncoding.GetString(BytesXor(b, p))
            If Not IncludesSpace Then
                Try
                    strReturn = strReturn.Substring(0, InStr(strReturn, " ") - 1)
                Catch ex As Exception
                    strReturn = "#ERROR#"
                End Try
            End If
        End If

        Return strReturn

    End Function

    Protected Friend Shared Function PasswdEncode(Body As String, Optional Password As String = "") As String
        Dim strReturn As String

        If Password.Length = 0 Then
            strReturn = Convert.ToBase64String(MyEncoding.GetBytes(Body))
        Else
            Dim i As Integer = 0
            While Password.Length < Body.Length
                Password &= Password.Substring(i, 1)
                i += 1
            End While

            Dim p As Byte() = MyEncoding.GetBytes(Password)
            Dim b As Byte() = MyEncoding.GetBytes(Body)

            strReturn = Convert.ToBase64String(BytesXor(b, p))

        End If

        Return strReturn

    End Function

    Protected Friend Shared Function PasswdDecode(Encoded As String, Optional Password As String = "") As String
        Dim strReturn As String
        Dim b As Byte() = Convert.FromBase64String(Encoded)
        If Password.Length = 0 Then
            strReturn = MyEncoding.GetString(b)
        Else
            Dim i As Integer = 0
            While Password.Length < b.Length
                Password &= Password.Substring(i, 1)
                i += 1
            End While

            Dim p As Byte() = MyEncoding.GetBytes(Password)

            strReturn = MyEncoding.GetString(BytesXor(b, p))

        End If

        Return strReturn

    End Function

End Class
