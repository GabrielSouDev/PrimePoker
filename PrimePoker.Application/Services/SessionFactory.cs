using PrimePoker.Domain.Player;
using PrimePoker.Domain.Session;
using PrimePoker.Domain.Session.Enums;

namespace PrimePoker.Application.Services;

public static class SessionFactory
{
    public static SessionConfiguration CreateConfiguration(this SessionRequest sessionRequest)
    {
        SessionConfiguration configuration = new();
        configuration.Set(sessionRequest);

        return configuration;
    }

    public static void Set(this SessionConfiguration configuration, SessionRequest sessionRequest)
    {
        configuration.MaxPlayer = (int)sessionRequest.SessionType;
        configuration.TimeType = sessionRequest.SessionTimeType.ToString();

        switch (sessionRequest.SessionTimeType)
        {
            case SessionTimeType.Normal:
                configuration.InitialChips = 50000;
                configuration.InitialTimeBank = 60;
                configuration.ActionTime = 5;
                configuration.IntialBigBlind = 300;
                configuration.IncreseBigBlindPorcent = 1;
                configuration.SecondsToIncreseBigBlind = 60 * 3;
                break;

            case SessionTimeType.Turbo:
                configuration.InitialChips = 50000;
                configuration.InitialTimeBank = 60;
                configuration.ActionTime = 5;
                configuration.IntialBigBlind = 300;
                configuration.IncreseBigBlindPorcent = 1;
                configuration.SecondsToIncreseBigBlind = 60 * 3;
                break;

            default:
                throw new Exception("Falha na configuração de Session!");
        }
    }
}


