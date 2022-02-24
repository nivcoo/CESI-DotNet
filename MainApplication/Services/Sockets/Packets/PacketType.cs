namespace MainApplication.Services.Sockets.Packets;

public enum PacketType
{
    Server_HomePage_GetConfig,
    Server_HomePage_ChangeMaxFileSize,
    Server_HomePage_ChangeSavesFileType,
    Server_HomePage_ChangeLogsFileType,
    Server_HomePage_ChangeCulture,


    Client_HomePage_SetConfig

}
