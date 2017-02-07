namespace abbStatus
{
    public enum found
    {
        net = 0,
        netSaveConn = 1,
        netSaveDiscon = 2,
        sim = 3,
        simSaveConn = 4,
        simSaveDisconn = 5
    };

    public enum conn
    {
        notVisible = -2,
        notAvailable = -1,
        disconnOK = 0,
        connOK = 1
    };

    public enum mail
    {
        exception = -1,
        closeApp = 0,
        openApp = 1
    };
}