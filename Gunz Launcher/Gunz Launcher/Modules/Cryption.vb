Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Module Cryption
    Function _Decrypt(ByVal strText As String, ByVal sDecrKey As String) As String
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Dim inputByteArray(strText.Length) As Byte
        Try
            Dim byKey() As Byte = System.Text.Encoding.UTF8.GetBytes(Left(sDecrKey, 8))
            Dim des As New DESCryptoServiceProvider
            inputByteArray = Convert.FromBase64String(strText)
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Function _Encrypt(ByVal strText As String, ByVal strEncrKey As String) As String
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
        Try
            Dim bykey() As Byte = System.Text.Encoding.UTF8.GetBytes(Left(strEncrKey, 8))
            Dim InputByteArray() As Byte = System.Text.Encoding.UTF8.GetBytes(strText)
            Dim des As New DESCryptoServiceProvider
            Dim ms As New MemoryStream
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(bykey, IV), CryptoStreamMode.Write)
            cs.Write(InputByteArray, 0, InputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Function Decrypt(ByVal cipherText As String) _
                          As String
        Try
            Dim passPhrase As String = "DemoAntiCheat"
            Dim saltValue As String = "iwanna"
            Dim hashAlgorithm As String = "SHA1"
            Dim passwordIterations As Integer = 12
            Dim initVector As String = "@1B2c3D4e5F6g7H8"
            Dim keysize As Integer = 256

            Dim initVectorBytes As Byte()
            initVectorBytes = Encoding.ASCII.GetBytes(initVector)

            Dim saltValueBytes As Byte()
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

            Dim cipherTextBytes As Byte()
            cipherTextBytes = Convert.FromBase64String(cipherText)

            Dim password As PasswordDeriveBytes
            password = New PasswordDeriveBytes(passPhrase, _
                                               saltValueBytes, _
                                               hashAlgorithm, _
                                               passwordIterations)

            Dim keyBytes As Byte()
            keyBytes = password.GetBytes(keysize / 8)

            Dim symmetricKey As RijndaelManaged
            symmetricKey = New RijndaelManaged()

            symmetricKey.Mode = CipherMode.CBC

            Dim decryptor As ICryptoTransform
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

            Dim memoryStream As MemoryStream
            memoryStream = New MemoryStream(cipherTextBytes)

            Dim cryptoStream As CryptoStream
            cryptoStream = New CryptoStream(memoryStream, _
                                            decryptor, _
                                            CryptoStreamMode.Read)

            Dim plainTextBytes As Byte()
            ReDim plainTextBytes(cipherTextBytes.Length)

            Dim decryptedByteCount As Integer
            decryptedByteCount = cryptoStream.Read(plainTextBytes, _
                                                   0, _
                                                   plainTextBytes.Length)

            memoryStream.Close()
            cryptoStream.Close()

            Dim plainText As String
            plainText = Encoding.UTF8.GetString(plainTextBytes, _
                                                0, _
                                                decryptedByteCount)

            Return plainText
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Module
