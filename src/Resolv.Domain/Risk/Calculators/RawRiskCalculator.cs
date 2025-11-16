namespace Resolv.Domain.Risk.Calculators;

public class RawRiskCalculator : IRawRiskCalculator
{
    public int GetRawRisk(Exposure exposureId, int exposurePoint)
    {
        if (exposureId == Exposure.Extensive)
            return CheckExtensive(exposurePoint);

        if (exposureId == Exposure.Widespread)
            return CheckWidespread(exposurePoint);

        if (exposureId == Exposure.Significant)
            return CheckSignificant(exposurePoint);

        if (exposureId == Exposure.Restricted)
            return CheckRestricted(exposurePoint);

        if (exposureId == Exposure.Negligible)
            return CheckNegligible(exposurePoint);

        return 0;
    }

    private static int CheckNegligible(int exposurePoint)
    {
        switch (exposurePoint)
        {
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 4;
            case 4:
                return 7;
            case 5:
                return 11;
            case 6:
                return 16;
            case 7:
                return 21;
            case 8:
                return 26;
            case 9:
                return 31;
            case 10:
                return 36;
            case 11:
                return 41;
            case 12:
                return 46;
            case 13:
                return 51;
            case 14:
                return 56;
            case 15:
                return 61;
            case 16:
                return 66;
            case 17:
                return 71;
            case 18:
                return 76;
            case 19:
                return 81;
            case 20:
                return 86;
            case 21:
                return 91;
            case 22:
                return 96;
            case 23:
                return 101;
            case 24:
                return 106;
            case 25:
                return 111;
            default:
                break;
        }

        return 0;
    }

    private static int CheckRestricted(int exposurePoint)
    {
        switch (exposurePoint)
        {
            case 1:
               return 3;
            case 2:
                return 5;
            case 3:
                return 8;
            case 4:
                return 12;
            case 5:
                return 17;
            case 6:
                return 22;
            case 7:
                return 27;
            case 8:
                return 32;
            case 9:
                return 37;
            case 10:
                return 42;
            case 11:
                return 47;
            case 12:
                return 52;
            case 13:
                return 57;
            case 14:
                return 62;
            case 15:
                return 67;
            case 16:
                return 72;
            case 17:
                return 77;
            case 18:
                return 82;
            case 19:
                return 87;
            case 20:
                return 92;
            case 21:
                return 97;
            case 22:
                return 102;
            case 23:
                return 107;
            case 24:
                return 112;
            case 25:
                return 116;
            default:
                break;
        }

        return 0;
    }

    private static int CheckSignificant(int exposurePoint)
    {
        switch (exposurePoint)
        {
            case 1:
                return 6;
            case 2:
                return 9;
            case 3:
                return 13;
            case 4:
                return 18;
            case 5:
                return 23;
            case 6:
                return 28;
            case 7:
                return 33;
            case 8:
                return 38;
            case 9:
                return 43;
            case 10:
                return 48;
            case 11:
                return 53;
            case 12:
                return 58;
            case 13:
                return 63;
            case 14:
                return 68;
            case 15:
                return 73;
            case 16:
                return 78;
            case 17:
                return 83;
            case 18:
                return 88;
            case 19:
                return 93;
            case 20:
                return 98;
            case 21:
                return 103;
            case 22:
                return 108;
            case 23:
                return 113;
            case 24:
                return 117;
            case 25:
                return 120;
            default:
                break;
        }

        return 0;
    }

    private static int CheckWidespread(int exposurePoint)
    {
        switch (exposurePoint)
        {
            case 1:
                return 10;
            case 2:
                return 14;
            case 3:
                return 19;
            case 4:
                return 24;
            case 5:
                return 29;
            case 6:
                return 34;
            case 7:
                return 39;
            case 8:
                return 44;
            case 9:
                return 49;
            case 10:
                return 54;
            case 11:
                return 59;
            case 12:
                return 64;
            case 13:
                return 69;
            case 14:
                return 74;
            case 15:
                return 79;
            case 16:
                return 84;
            case 17:
                return 89;
            case 18:
                return 94;
            case 19:
                return 99;
            case 20:
                return 104;
            case 21:
                return 109;
            case 22:
                return 114;
            case 23:
                return 118;
            case 24:
                return 121;
            case 25:
                return 123;
            default:
                break;
        }

        return 0;
    }

    private static int CheckExtensive(int exposurePoint)
    {
        switch (exposurePoint)
        {
            case 1:
                return 15;
            case 2:
                return 20;
            case 3:
                return 25;
            case 4:
                return 30;
            case 5:
                return 35;
            case 6:
                return 40;
            case 7:
                return 45;
            case 8:
                return 50;
            case 9:
                return 55;
            case 10:
                return 60;
            case 11:
                return 65;
            case 12:
                return 70;
            case 13:
                return 75;
            case 14:
                return 80;
            case 15:
                return 85;
            case 16:
                return 90;
            case 17:
                return 95;
            case 18:
                return 100;
            case 19:
                return 105;
            case 20:
                return 110;
            case 21:
                return 115;
            case 22:
                return 119;
            case 23:
                return 122;
            case 24:
                return 124;
            case 25:
                return 125;
            default:
                break;
        }

        return 0;
    }
}
