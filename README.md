# XorEncryptionVB
Xor Encryption VB.NET

## About This Sample
This sample is an example of encrypting a string (byte array) using XOR.  
The default encoding is Unicode to support multi-byte strings such as Japanese.
Encoding can be changed in the property.  
This Program uses "BitArray" to perform on XOR.
This is to support Bit-Merge &c. in the future.  
Although it is written in Shared Function, it is also possible to copy and paste it and use it as a normal Function in a module.  
Encrypted strings are handled by Base64.  


[Special functions of this sample]  
1) If there is no space in the input-text because the string length is not inferred, the encrypted string is the passcode length.
On the other hand, if the input-text contains a space, the encrypted string is the input-text string length. （Binary Mode Only）  
2) A pass-stream for encryption can be cut from the inside of a specific file and used as a byte array.
The file used as pass-codes should be compressed files like 7zip, as they are better inside.


このサンプルは、XORを用いて文字列（バイト配列）を暗号化するサンプルです。  
日本語などのマルチバイト文字列に対応するため、デフォルトエンコードはUnicodeです。
エンコードはプロパティで変更できます。  
XORを実行するルーチンはBitArrayを使っています。
将来、Bit-Mergeなどに対応するためです。  
Shared Functionで書かれていますが、コピー＆ペーストして、モジュール内で通常のFunctionとして利用することも可能です。  
暗号化された文字列はBase64で扱っています。  


[このサンプルの独自機能]  
1) 文字列長を推測されないために、平文にスペースがない場合は、暗号化文字列はパスコードの長さになります。
一方、平文がスペースを含む場合は、暗号化文字列は、平文の文字列長になります。(バイナリモードのみ)  
2) 暗号化するためのパスストリームを特定のファイル内部から切り取ってバイト配列として使用することができます。
パスコードとして利用するファイルは、中がゴチャゴチャのほうがいいので、7zipのような圧縮ファイルがいいでしょう。


## Example
You can see the working code in Module1.vb.  
Following List is a usage sample.  
```
Imports XorEncryptionVB.XorEncryption

Module Module1
    Sub Main()
        Dim EncodedString As String
        Dim DecodedString As String
        Dim p As Byte()

        ' Test Sample with Text mode
        ' EncodedString Length will be same as "MyPassLogEnough" Length
        EncodedString  = PasswdEncode("MyText", "MyPassLogEnough")
        DecodedString  = PasswdDecode(EncodedString, "MyPassLogEnough")
        Console.WriteLine("Result: " & ("MyText" = DecodedString).ToString)
        'true

        ' Test Sample with Binary mode : Not Include Space in BaseText
        p = XorEncryption.Encoding.GetBytes("MyPass")
        EncodedString = PasswdEncode("MyText", p)
        DecodedString = PasswdDecode(EncodedString, p)
        Console.WriteLine("Result: " & ("MyText" = DecodedString).ToString)
        'true

        ' Test Sample with Binary mode: Include Space in BaseText
        ' EncodedString Length will be same as "MyText With Spaces" Length
        p = XorEncryption.Encoding.GetBytes("MyPass")
        EncodedString  = PasswdEncode("MyText With Spaces", p, True)
        DecodedString  = PasswdDecode(EncodedString, p, True)
        Console.WriteLine("Result: " & ("MyText" = DecodedString).ToString)
        'true

        ' Test Sample with File Pass Stream
        p = GetBytesFromFile("C:\FileName.7z", 50, 24)
        EncodedString = PasswdEncode("MyText", p)
        DecodedString = PasswdDecode(EncodedString, p)
        Console.WriteLine("Result: " & (MyText = DecodedString).ToString & vbNewLine)
        'true

        ' Test Sample with Text mode with ASCII Encoding
        ' MultiByte String will NOT work well
        XorEncryption.Encoding = System.Text.Encoding.ASCII
        EncodedString = PasswdEncode("MyText", "MyPass")
        DecodedString = PasswdDecode(EncodedString, "MyPass")
        Console.WriteLine("Result: " & (MyText = DecodedString).ToString & vbNewLine)
        'true
        EncodedString = PasswdEncode("まいてきすと", "MyPass")
        DecodedString = PasswdDecode(EncodedString, "MyPass")
        Console.WriteLine("Result: " & (MyText = DecodedString).ToString & vbNewLine)
        'false
    End Sub
End Module
```

## XorEncryption Class
### Propaties and Methos
    Shared Property Encoding As Encoding
    Shared Function GetBytesFromFile(FileName As String, Optional StartPos As Integer = 0, Optional ByteLength As Integer = 24) As Byte()
    Shared Function BytesXor(a1 As Byte(), a2 As Byte()) As Byte()
    Shared Function PasswdEncode(Body As String, Optional Password As String = "") As Base64_String
    Shared Function PasswdEncode(Body As String, PassStream As Byte(), Optional IncludesSpace As Boolean = False) As Base64_String
    Shared Function PasswdDecode(Encoded As Base64_String, Optional Password As String = "") As String
    Shared Function PasswdDecode(Encoded As Base64_String, PassStream As Byte(), Optional IncludesSpace As Boolean = False) As String

