namespace abbStatus
{
    enum found
    {
        net = 0,
        netSaveConn = 1,
        netSaveDiscon = 2,
        sim = 3,
        simSaveConn = 4,
        simSaveDisconn = 5
    };

    enum conn
    {
        notVisible = -2,
        notAvailable = -1,
        disconn = 0,
        connOK = 1
    };
}