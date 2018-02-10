Module Enums
    Public Enum Packet As Byte
        LoginAcc = 1
        FailedLoginAcc = 2

        AccountBanned = 3
        AccountChatBanned = 4
        AccountNotActive = 5

        ClientDisconnected = 6
        AdminDisconnected = 7

        AdminBannedClient = 8

        Ping = 16
        Key = 17
        refreshed = 21
        hackinfo = 22

        hardwarebanned = 23
        IPbanned = 24
    End Enum
    Public Enum AccountInfo As Byte
        Normal = 1
        EventWinner = 2
        Donator_green_colored = 3
        Donate_blue_colored = 4
        Donator_purple_colored = 5
        ChatBanned = 104
        TGM = 252
        Banned = 253
        Developer_GM = 254
        Administrator = 255
        NotActive = 0
        FailedLogging = 200
        Firstrun = 201
    End Enum

    Public Enum AdminPacketCommand As Byte
        AccountBan = 9
        IPBan = 10
        ClientBan = 11
        ClientDisconnect = 12
        AddConnectedPlayer = 13
        RemoveConnectedPlayer = 14
        RefreshPlayerList = 15
        UnbanIP = 18
        UnbanAccount = 19
        UnbanClient = 20
    End Enum
End Module
