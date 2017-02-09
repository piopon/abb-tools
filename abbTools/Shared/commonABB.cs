namespace abbStatus
{
    public enum found
    {
        net = 0,
        netSaveAvail = 1,
        netSaveConn = 2,
        netSaveDiscon = 3,
        sim = 4,
        simSaveAvail = 5,
        simSaveConn = 6,
        simSaveDisconn = 7
    };

    public enum conn
    {
        notVisible = -2,
        notAvailable = -1,
        disconnOK = 0,
        available = 1,
        connOK = 2
    };

    public enum mail
    {
        exception = -1,
        closeApp = 0,
        openApp = 1
    };
}